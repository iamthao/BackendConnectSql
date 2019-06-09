using System.Data.Entity.Core;
using System.Data.Entity.Migrations;
using System.Security.Claims;
using Framework.DomainModel;
using Framework.DomainModel.Entities;
using Framework.DomainModel.Entities.Mapping;
using Framework.DomainModel.Interfaces;
using Framework.Exceptions;
using Framework.Service.Diagnostics;
using Framework.Web;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web.Http;

namespace Database.Persistance.Tenants
{
    public class TenantDataContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<DocumentType> DocumentTypes { get; set; }
        public DbSet<GridConfig> GridConfigs { get; set; }
        public DbSet<SecurityOperation> SecurityOperations { get; set; }
        public DbSet<UserRoleFunction> UserRoleFunctions { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Module> Modules { get; set; }
        public DbSet<ModuleDocumentTypeOperation> ModuleDocumentTypeOperations { get; set; }
        public DbSet<FranchiseeModule> FranchiseeModules { get; set; }
        public DbSet<FranchiseeTenant> FranchiseeTenants { get; set; }


        public DbSet<AutomateSendRequest> AutomateSendRequests { get; set; }
        public DbSet<Courier> Couriers { get; set; }
        public DbSet<HoldingRequest> HoldingRequests { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<NoteRequest> NoteRequests { get; set; }
        public DbSet<Request> Requests { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<ServiceConfiguration> ServiceConfigurations { get; set; }
        public DbSet<State> States { get; set; }

        public DbSet<CountryOrRegion> CountryOrRegion { get; set; }
        public DbSet<Tracking> Trackings { get; set; }
        public DbSet<StaticValue> StaticValues { get; set; }
        public DbSet<FranchiseeConfiguration> FranchiseeConfigurations { get; set; }
        public DbSet<SystemEvent> SystemEvents { get; set; }
        public DbSet<RequestHistory> RequestHistories { get; set; }

        public DbSet<Industry> Industries { get; set; }
        public DbSet<PackageHistory> PackageHistories { get; set; }
        public DbSet<Register> Registers { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Template> Templates { get; set; }
        public DbSet<SystemConfiguration> SystemConfigurations { get; set; }
        public DbSet<TableVersion> TableVersions { get; set; }
        static TenantDataContext()
        {
            System.Data.Entity.Database.SetInitializer<TenantDataContext>(null);
        }

        public TenantDataContext()
        {
            System.Data.Entity.Database.SetInitializer<TenantDataContext>(null);
        }

        private static string NameOrConnectionString { get; set; }

        public ObjectContext ObjectContext { get; private set; }

        public TenantDataContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
            System.Data.Entity.Database.SetInitializer<TenantDataContext>(null);
            var objectContext = (this as IObjectContextAdapter).ObjectContext;
            // TODO: Need configuration . Sets the command timeout for all the commands
            objectContext.CommandTimeout = 360;
            ObjectContext = objectContext;
            NameOrConnectionString = nameOrConnectionString;
        }

        public User GetCurrentUser()
        {

            User currentUser = null;
            var context = GlobalConfiguration.Configuration.DependencyResolver.GetService(typeof(IQuickspatchHttpContext)) as IQuickspatchHttpContext;
            IQuickspatchPrincipal principal;
            //web
            if (context != null)
            {
                principal = context.User as IQuickspatchPrincipal;
            }
            else
            {
                principal = Thread.CurrentPrincipal as IQuickspatchPrincipal;
            }

            if (principal != null)
            {
                if(principal.User != null)
                    currentUser = Users.SingleOrDefault(x => x.Id == principal.User.Id);

            }
            //API
            else
            {
                ClaimsPrincipal userApiClaimPrincipal;
                if (context != null)
                {
                    userApiClaimPrincipal = context.User as ClaimsPrincipal;
                }
                else
                {
                    userApiClaimPrincipal = Thread.CurrentPrincipal as ClaimsPrincipal;
                }
                if (userApiClaimPrincipal != null)
                {
                    var userIdClaim = userApiClaimPrincipal.Claims.FirstOrDefault(c => c.Type == ClaimsDeclaration.IdClaim);
                    if (userIdClaim != null)
                    {
                        int userId;
                        if (Int32.TryParse(userIdClaim.Value, out userId))
                        {
                            currentUser = Users.SingleOrDefault(x => x.Id == userId);
                        }
                    }
                }
            }
            

            return currentUser;
        }

        public override int SaveChanges()
        {
            var currentUser = GetCurrentUser();

            var deletedItemList = new List<DbEntityEntry<Entity>>();

            foreach (var entry in ChangeTracker.Entries<Entity>())
            {
                if (entry.Entity.IsDeleted)
                {
                    entry.State = EntityState.Deleted;
                }

                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.SetCreatedOn(DateTime.UtcNow);

                        entry.Entity.SetCreatedBy(currentUser);
                        entry.Entity.SetLastUser(currentUser);
                        entry.Entity.SetLastModified(DateTime.UtcNow);
                        break;
                    case EntityState.Modified:
                        entry.Entity.SetLastModified(DateTime.UtcNow);
                        entry.Entity.SetLastUser(currentUser);
                        entry.OriginalValues["LastModified"] = entry.Entity.LastModified;
                        //ObjectContext.Refresh(RefreshMode.ClientWins, entry.Entity);
                        break;
                    default:
                        if (entry.State == EntityState.Deleted || entry.Entity.IsDeleted)
                        {
                            entry.State = EntityState.Deleted;
                            //entry.OriginalValues["LastModified"] = entry.Entity.LastModified;
                        }
                        break;
                }
                CreateVersion(entry);
            }

            int result;
            try
            {
                result = base.SaveChanges();
               
                
            }
            catch (DbEntityValidationException ex)
            {
                var sb = new StringBuilder();
                foreach (var failure in ex.EntityValidationErrors)
                {
                    sb.AppendFormat("{0} failed validation\n", failure.Entry.Entity.GetType());
                    foreach (var error in failure.ValidationErrors)
                    {
                        sb.AppendFormat("- {0} : {1}", error.PropertyName, error.ErrorMessage);
                        sb.AppendLine();
                    }
                }
                if (sb.Length != 0)
                {
                    var diagnosticService = GlobalConfiguration.Configuration.DependencyResolver.GetService(typeof(IDiagnosticService)) as IDiagnosticService;
                    if (diagnosticService != null)
                    {
                        diagnosticService.Error(sb.ToString());
                    }
                }
                throw new UserVisibleException("GeneralExceptionMessageText", ex);
            }
            catch (Exception ex)
            {
                var exceptionString = ex.ToString();
                var exceptionNameArray = exceptionString.Substring(0, exceptionString.IndexOf(':')).Split(new char[] { '.' });
                var actualName = string.Format("Database.Persistance.ExceptionHandler.{0}Handler", exceptionNameArray[exceptionNameArray.Length - 1]);
                var exceptionType = Type.GetType(actualName);
                var exceptionParams = new object[] { "GeneralExceptionMessageText", ex };

                if (exceptionType != null)
                    exceptionParams = (object[])exceptionType.GetMethod("Process").Invoke(null, new object[] { ex });

                throw new UserVisibleException(exceptionParams[0].ToString(), (Exception)exceptionParams[1]);
            }
            return result;
        }

        private void CreateVersion(DbEntityEntry<Entity> entry)
        {
            var entityType = entry.Entity.GetType();
            if (entityType.BaseType != null && (entityType.BaseType.Name.Equals("Location") || entityType.FullName.Equals("Framework.DomainModel.Entities.Location")))
            {
                var obj = TableVersions.FirstOrDefault(o => o.TableId == TableInfo.Location);
                if (obj != null)
                {
                    obj.Version = Guid.NewGuid().ToString("N");
                }
                else
                {
                    var tableVersion = new TableVersion
                    {
                        TableId = TableInfo.Location,
                        Version = Guid.NewGuid().ToString("N"),
                        CreatedById = 1,
                        LastUserId = 1,
                    };
                    tableVersion.SetCreatedOn(DateTime.UtcNow);
                    tableVersion.SetLastModified(DateTime.UtcNow);
                    TableVersions.Add(tableVersion);
                }
            }

            if (entityType.BaseType != null && (entityType.BaseType.Name.Equals("Courier") || entityType.FullName.Equals("Framework.DomainModel.Entities.Courier")))
            {
                var obj = TableVersions.FirstOrDefault(o => o.TableId == TableInfo.Courier);
                if (obj != null)
                {
                    obj.Version = Guid.NewGuid().ToString("N");
                }
                else
                {
                    var tableVersion = new TableVersion
                    {
                        TableId = TableInfo.Courier,
                        Version = Guid.NewGuid().ToString("N"),
                        CreatedById = 1,
                        LastUserId = 1,
                    };
                    tableVersion.SetCreatedOn(DateTime.UtcNow);
                    tableVersion.SetLastModified(DateTime.UtcNow);
                    TableVersions.Add(tableVersion);
                }
            }
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new UserMap());
            modelBuilder.Configurations.Add(new DocumentTypeMap());
            modelBuilder.Configurations.Add(new GridConfigMap());
            modelBuilder.Configurations.Add(new SecurityOperationMap());
            modelBuilder.Configurations.Add(new UserRoleFunctionMap());
            modelBuilder.Configurations.Add(new UserRoleMap());
            modelBuilder.Configurations.Add(new FranchiseeModuleMap());
            modelBuilder.Configurations.Add(new ModuleMap());
            modelBuilder.Configurations.Add(new FranchiseeTenantMap());
            modelBuilder.Configurations.Add(new ModuleDocumentTypeOperationMap());
            modelBuilder.Configurations.Add(new CustomerMap());
            modelBuilder.Configurations.Add(new AutomateSendRequestMap());
            modelBuilder.Configurations.Add(new CourierMap());
            modelBuilder.Configurations.Add(new HoldingRequestMap());
            modelBuilder.Configurations.Add(new LocationMap());
            modelBuilder.Configurations.Add(new NoteRequestMap());
            modelBuilder.Configurations.Add(new RequestMap());
            modelBuilder.Configurations.Add(new ScheduleMap());
            modelBuilder.Configurations.Add(new ServiceConfigurationMap());
            modelBuilder.Configurations.Add(new StateMap());
            modelBuilder.Configurations.Add(new TrackingMap());
            modelBuilder.Configurations.Add(new StaticValueMap());
            modelBuilder.Configurations.Add(new FranchiseeConfigurationMap());
            modelBuilder.Configurations.Add(new SystemEventMap());
            modelBuilder.Configurations.Add(new CountryOrRegionMap());
            modelBuilder.Configurations.Add(new RequestHistoryMap());
            modelBuilder.Configurations.Add(new IndustryMap());
            modelBuilder.Configurations.Add(new PackageHistoryMap());
            modelBuilder.Configurations.Add(new RegisterMap());
            modelBuilder.Configurations.Add(new ContactMap());
            modelBuilder.Configurations.Add(new TemplateMap());
            modelBuilder.Configurations.Add(new SystemConfigurationMap());
            modelBuilder.Configurations.Add(new TableVersionMap());
        }
    }
}