using System;
using System.Data.Entity;

namespace Database.Persistance
{
    public abstract class EntityFrameworkWorkspaceBase<TContext> : IWorkspace where TContext : DbContext
    {
        public EntityFrameworkWorkspaceBase(TContext context)
        {
            Context = context;
        }

        public TContext Context { get; private set; }

        public void Commit()
        {
            Context.SaveChanges();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public Action ReleaseCallback { get; set; }

        ~EntityFrameworkWorkspaceBase()
        {
            Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (ReleaseCallback != null)
                {
                    ReleaseCallback();
                }

                Context.Dispose();
            }
        }
    }
}