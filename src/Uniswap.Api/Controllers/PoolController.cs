using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using UniswapClient;

namespace Uniswap.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AssetsController : ControllerBase
    {
        private readonly Client _client;
        public AssetsController()
        {
            _client = new Client(new Uri("https://api.thegraph.com/subgraphs/name/ianlapham/uniswap-v3-alt"));
        }

        [HttpGet("{id}/pools")]
        public async Task<object> Pools(string id)
        {
            var poolResponse = await _client.GetPoolsByToken(id);

            if (poolResponse.Errors != null)
                return BadRequest(poolResponse.Errors); //TODO: A generic error to prevent data leak

            var pools = poolResponse.Data.Items.Select(x =>
                        new Pool()
                        {
                            Id = x.Id,
                            Asset0Id = x.Token0.Id,
                            Asset0Name = x.Token1.Name,
                            Asset1Id = x.Token1.Id,
                            Asset1Name = x.Token1.Name
                        });

            return Ok(pools);
        }

        [HttpGet("{id}/swaps/totals")]
        public async Task<object> SwapTotals(string id, [FromQuery] string from, [FromQuery] string to)
        {
            var swapsResponse = await _client.GetSwapsByRangeAndToken(from, to, id);

            if (swapsResponse.Errors != null)
                return BadRequest(swapsResponse.Errors); //TODO: A generic error to prevent data leak

            // i'm assuming amount0 / amount1 equats to the total volume for the corresponding token0 / token1
            var totalVolume = swapsResponse.Data.TotalVolume(id);

            return Ok(new
            {
                TotalVolume = 0
            });
        }
    }
}
