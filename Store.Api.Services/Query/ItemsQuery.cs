using Microsoft.EntityFrameworkCore;
using Store.Api.Domain;
using Store.Api.Domain.Enums;
using Store.Api.Services.Query.Base;

namespace Store.Api.Services.Query
{
    public class ItemData
    {
        public int ItemId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public QuantityType QtyType { get; set; }
        public string Remarks { get; set; }
    }

    public class ItemSearchByNameCriteria
    {
        public string Name { get; set; }
    }

    public interface IItemsQuery: IQueryHandlerAsync<List<ItemData>>,
        IQueryHandlerAsync<List<ItemData>, ItemSearchByNameCriteria>,
        IQueryHandlerAsync<ItemData, int>
    { }

    public class ItemsQuery : IItemsQuery
    {
        private readonly UnitOfWork _unitOfWork;
        public ItemsQuery(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<List<ItemData>> ExecuteAsync(CancellationToken cancellationToken = default)
        {
            return await _unitOfWork.ItemsRepository.Get().Select(x => new ItemData
            {
                Code = x.Code,
                ItemId = x.ItemId,
                Name = x.Name,
                Quantity = x.Quantity,
                QtyType = x.QuantityType,
                Remarks = x.Remarks
            }).ToListAsync(cancellationToken);
        }

        public async Task<List<ItemData>> ExecuteAsync(ItemSearchByNameCriteria criteria, CancellationToken cancellationToken = default)
        {
            var result = new List<ItemData>();

            var query = await _unitOfWork.ItemsRepository.Get(x => x.Name.Contains(criteria.Name)).ToListAsync(cancellationToken);

            if (query != null && query.Any())
            {
                result = query.Select(x => new ItemData
                {
                    Code = x.Code,
                    ItemId = x.ItemId,
                    Name = x.Name,
                    Quantity = x.Quantity,
                    QtyType = x.QuantityType,
                    Remarks = x.Remarks
                }).ToList();
            }

            return result;
        }

        public async Task<ItemData> ExecuteAsync(int criteria, CancellationToken cancellationToken = default)
        {
            var item = await _unitOfWork.ItemsRepository.Get(x => x.ItemId == criteria).Select(x => new ItemData
            {
                Code = x.Code,
                ItemId = x.ItemId,
                Name = x.Name,
                Quantity = x.Quantity,
                QtyType = x.QuantityType,
                Remarks = x.Remarks
            }).FirstOrDefaultAsync(cancellationToken);

            return item;
        }
    }
}
