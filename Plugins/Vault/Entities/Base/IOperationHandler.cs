using System.Collections.Generic;
using powerGateServer.SDK;

namespace VaultServices.Entities.Base
{
    public interface IOperationHandler<T>
    {
        IQueryOperation<T> QueryOperations { get; }
        ICreateOperation<T> CreateOperations { get; }
        IDeleteOperation<T> DeleteOperations { get; }
        IUpdateOperation<T> UpdateOperations { get; }
    }

    public interface IQueryOperation<T>
    {
        bool CanExecute(IExpression<T> expression);
        IEnumerable<T> Execute();
    }

    public interface ICreateOperation<T> : IOperationExecutor<T>
    {
    }

    public interface IDeleteOperation<T> : IOperationExecutor<T>
    {
    }

    public interface IUpdateOperation<T> : IOperationExecutor<T>
    {
    }

    public interface IOperationExecutor<T>
    {
        bool CanExecute(T entity);
        void Execute();
    }
}