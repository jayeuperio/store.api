using Microsoft.AspNetCore.Mvc;
using Store.Api.Services.Command;
using Store.Api.Services.Query;

namespace Store.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private readonly IItemsQuery _itemsQuery;
        private readonly IItemsCommandHandler _itemsCommandHandler;

        public ItemsController(IItemsQuery itemsQuery,
            IItemsCommandHandler itemsCommandHandler)
        {
            _itemsQuery = itemsQuery;
            _itemsCommandHandler = itemsCommandHandler;

        }

        [HttpGet]
        public async Task<IActionResult> GetItems(CancellationToken cancellationToken = default)
        {
            var result = await _itemsQuery.ExecuteAsync(cancellationToken);

            return Ok(result);
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetItem(int id, CancellationToken cancellationToken = default)
        {
            var item = await _itemsQuery.ExecuteAsync(id, cancellationToken);

            return Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> SaveItem(ItemsSaveCommand command, CancellationToken cancellationToken = default)
        {
            var result = await _itemsCommandHandler.ExecuteAsync(command, cancellationToken);

            return Ok(result);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteItem(ItemsDeleteCommand command, CancellationToken cancellationToken = default)
        {
            var result = await _itemsCommandHandler.ExecuteAsync(command, cancellationToken);

            return Ok(result);
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchItems([FromQuery]ItemSearchByNameCriteria criteria, CancellationToken cancellationToken = default)
        {
            var result = await _itemsQuery.ExecuteAsync(criteria, cancellationToken);

            return Ok(result);
        }
    }
}
