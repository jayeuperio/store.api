using Store.Api.Domain;
using Store.Api.Domain.Enums;
using Store.Api.Domain.Model;
using Store.Api.Services.Command.Base;

namespace Store.Api.Services.Command
{
    public class ItemsSaveCommand
    {
        public int ItemId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int Quantity { get; set; }
        public QuantityType Type { get; set; }
        public string Remarks { get; set; }
    }

    public class ItemsDeleteCommand
    {
        public int ItemId { get; set; }
    }

    public interface IItemsCommandHandler: ICommandHandlerAsync<ItemsSaveCommand>,
        ICommandHandlerAsync<ItemsDeleteCommand>
    { }
    public class ItemsCommandHandler : IItemsCommandHandler
    {
        private readonly UnitOfWork _unitOfWork;
        public ItemsCommandHandler(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> ExecuteAsync(ItemsSaveCommand command, CancellationToken cancellationToken = default)
        {
            var result = new Result();
            var item = new Domain.Data.Entities.Items();

            if (command.ItemId > 0)
            {
                item = _unitOfWork.ItemsRepository.GetByID(command.ItemId);

                if (item != null)
                {
                    item.Code = command.Code;
                    item.Remarks = command.Remarks;
                    item.Quantity = command.Quantity;
                    item.QuantityType = command.Type;
                    item.Name = command.Name;

                    _unitOfWork.ItemsRepository.Update(item);
                }
                else
                {
                    result.AddError($"Unable to find Item with ID = {command.ItemId}");
                }
            }
            else
            {
                item = new Domain.Data.Entities.Items
                {
                    Code = command.Code,
                    Name = command.Name,
                    Quantity = command.Quantity,
                    QuantityType = command.Type,
                    Remarks = command.Remarks
                };
                _unitOfWork.ItemsRepository.Insert(item);
            }

            if (result.IsSuccess)
            {
                await _unitOfWork.SaveAsync(cancellationToken);
            }

            return result;
        }

        public async Task<Result> ExecuteAsync(ItemsDeleteCommand command, CancellationToken cancellationToken = default)
        {
            var result = new Result();
            var item = _unitOfWork.ItemsRepository.GetByID(command.ItemId);

            if (item != null)
            {
                _unitOfWork.ItemsRepository.Update(item);
            }
            else
            {
                result.AddError($"Unable to find Item with ID = {command.ItemId}");
            }

            if (result.IsSuccess)
            {
                _unitOfWork.ItemsRepository.Delete(item);
                await _unitOfWork.SaveAsync(cancellationToken);
            }

            return result;
        }
    }
}
