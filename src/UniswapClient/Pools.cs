using Newtonsoft.Json;
using System.Collections.Generic;

namespace UniswapClient
{
    public class Pools : IUniswapData<PoolId>
    {
        public Pools()
        {
            Items = new List<PoolId>();
        }

        [JsonProperty("pools")]
        public List <PoolId> Items { get; set; }
    }
}
