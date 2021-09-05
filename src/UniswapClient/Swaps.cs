using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace UniswapClient
{
    public class Swaps : IUniswapData<Swap>
    {
        public Swaps()
        {
            Items = new List<Swap>();
        }

        [JsonProperty("swaps")]
        public List<Swap> Items { get; set; }        

        public decimal TotalVolume(string tokenId)
        {
            var t1 = Items.Where(x => x.Token0.Id == tokenId).Sum(x => x.Amount0);
            var t2 = Items.Where(x => x.Token1.Id == tokenId).Sum(x => x.Amount1);

            return t1 + t2;
        }
    }
}
