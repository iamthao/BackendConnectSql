using System;
using System.Collections.Generic;

namespace Database.Persistance
{
    public abstract class EntityFrameworkPersistenceServiceBase<TWorkspace> : IPersistenceService<TWorkspace>
        where TWorkspace : IWorkspace
    {
        [ThreadStatic]
        private static Stack<TWorkspace> _workspaces = new Stack<TWorkspace>();

        public Stack<TWorkspace> Workspaces
        {
            get { return _workspaces ?? (_workspaces = new Stack<TWorkspace>()); }
        }

        public TWorkspace CurrentWorkspace
        {
            get
            {
                if (Workspaces.Count == 0)
                {
                    var workspace = CreateWorkspace();

                    return workspace;
                }
                else
                {
                    var workspace = Workspaces.Peek();
                    return workspace;
                }
            }
        }

        public TWorkspace CreateWorkspace()
        {
            var workspace = CreateWorkspaceCore();
            SetWorkspace(workspace);
            return workspace;
        }

        public void SetWorkspace(TWorkspace workspace)
        {
            workspace.ReleaseCallback = () => ReleaseWorkspace(workspace);
            Workspaces.Push(workspace);
        }

        protected void ReleaseWorkspace(TWorkspace workspace)
        {
            var poppedWorkspace = Workspaces.Pop();

            if (!poppedWorkspace.Equals(workspace))
            {
                throw new InvalidOperationException();
            }
        }

        protected abstract TWorkspace CreateWorkspaceCore();
    }
}