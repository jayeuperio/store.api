using Store.Api.Domain.Enums;

namespace Store.Api.Domain.Data.Entities
{
    public class Items
    {
        public int ItemId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public QuantityType QuantityType { get; set; }
        public string Remarks { get; set; }
    }
}
