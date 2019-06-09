using System;
using System.ComponentModel;
using System.Diagnostics;
using System.ServiceProcess;

namespace QuickSpatchWindowsService
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : System.Configuration.Install.Installer
    {
        public ProjectInstaller()
        {
            const string serviceName = "QuickSpatchFranchiseeService";
            const string serviceDescription = "QuickSpatchFranchiseeService";

            serviceProcessInstaller1 = new ServiceProcessInstaller();
            serviceInstaller1 = new ServiceInstaller();
            //Create Instance of EventLogInstaller
            var myEventLogInstaller = new EventLogInstaller
            {
                Source = serviceName,
                Log = "Application"
            };

            //# Service Account Information
            serviceProcessInstaller1.Account = ServiceAccount.LocalSystem;
            serviceProcessInstaller1.Username = null;
            serviceProcessInstaller1.Password = null;

            //# Service Information
            serviceInstaller1.ServiceName = serviceName;
            serviceInstaller1.DisplayName = serviceName;
            serviceInstaller1.Description = serviceDescription;
            serviceInstaller1.StartType = ServiceStartMode.Automatic;

            Installers.Add(serviceProcessInstaller1);
            Installers.Add(serviceInstaller1);
            // Add myEventLogInstaller to the Installers Collection.
            Installers.Add(myEventLogInstaller);
        }

        private void RetrieveServiceName()
        {
            var serviceNameContext = this.Context.Parameters["servicename"];
            if (string.IsNullOrEmpty(serviceNameContext)) return;
            
            this.serviceInstaller1.ServiceName = serviceNameContext;
            this.serviceInstaller1.DisplayName = serviceNameContext;
        }

        public override void Install(System.Collections.IDictionary stateSaver)
        {
            RetrieveServiceName();
            base.Install(stateSaver);
        }

        public override void Uninstall(System.Collections.IDictionary savedState)
        {
            RetrieveServiceName();
            base.Uninstall(savedState);
        }
    }
}
