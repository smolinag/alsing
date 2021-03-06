namespace Alsing.Workspace
{
    using System;
    using System.Linq;

    public interface IWorkspace : IDisposable
    {
        event EventHandler Committed;

        event EventHandler Committing;

        IQueryable<T> MakeQuery<T>() where T : class;

        void Add<T>(T entity) where T : class;

        void Remove<T>(T entity) where T : class;

        void Commit();

        void AddCommitAction(CommitAction commitAction);
    }
}
