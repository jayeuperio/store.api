namespace Store.Api.Services.Query.Base
{
    public interface IQueryHandler
    {
    }

    public interface IQueryHandler<TResult>: IQueryHandler
    {
        TResult Execute();
    }

    public interface IQueryHandler<TResult, in TCriteria>: IQueryHandler
    {
        TResult Execute(TCriteria criteria);
    }

    public interface IQueryHandlerAsync<TResult> : IQueryHandler
    {
        Task<TResult> ExecuteAsync(CancellationToken cancellationToken = default(CancellationToken));
    }

    public interface IQueryHandlerAsync<TResult, in TCriteria>: IQueryHandler
    {
        Task<TResult> ExecuteAsync(TCriteria criteria, CancellationToken cancellationToken = default(CancellationToken));
    }
}
