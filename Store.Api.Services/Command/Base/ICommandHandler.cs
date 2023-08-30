using Store.Api.Domain.Model;

namespace Store.Api.Services.Command.Base
{
    public interface ICommandHandler
    {
    }

    public interface ICommandHandler<in TCommand> : ICommandHandler where TCommand : class
    {
        Result Execute(TCommand command);
    }

    public interface ICommandHandlerAsync<in TCommand>: ICommandHandler where TCommand : class
    {
        Task<Result> ExecuteAsync(TCommand command, CancellationToken cancellationToken = default);
    }
}
