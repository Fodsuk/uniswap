using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;
using UniswapClient;

namespace Uniswap.IntegrationTests
{
    public class ClientTests
    {
        private Client _client;

        [SetUp]
        public void Setup()
        {
            _client = new Client(new Uri("https://api.thegraph.com/subgraphs/name/ianlapham/uniswap-v3-alt"));
        }

        [Test]
        public async Task Pools_Returned_With_Valid_TokenId()
        {
            var response = await _client.GetPoolsByToken("0xa0b86991c6218b36c1d19d4a2e9eb0ce3606eb48");
            Assert.Greater(response.Data.Items.Count, 1);
            Assert.IsNull(response.Errors);

            foreach(var poolId in response.Data.Items)
            {
                Assert.IsNotNull(poolId.Id);
                Assert.Contains("0xa0b86991c6218b36c1d19d4a2e9eb0ce3606eb48", new[] { poolId.Token0.Id, poolId.Token1.Id });
            }
        }

        [Test]
        public async Task Pools_Empty_With_Invalid_Token()
        {
            var pools = await _client.GetPoolsByToken("no such token");
            Assert.AreEqual(pools.Data.Items.Count, 0);
        }

        [Test]
        public async Task Swaps_Filter_On_Date_Range()
        {
            var from = new DateTime(2021, 08, 18, 12, 1, 0);
            var to = new DateTime(2021, 08, 18, 12, 3, 0);

            var swaps = await _client.GetSwapsByRangeAndToken(DateTimeToUnixTimeStamp(from), DateTimeToUnixTimeStamp(to), "0xa0b86991c6218b36c1d19d4a2e9eb0ce3606eb48");

            Assert.Greater(swaps.Data.Items.Count, 0);

            foreach (var swap in swaps.Data.Items)
            {
                DateTime dt = UnixTimeStampToDateTime(swap.Timestamp);
                Assert.That(dt > from && dt < to);
            }

        }

        public DateTime UnixTimeStampToDateTime(string unixTimeStamp)
        {
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            dtDateTime = dtDateTime.AddSeconds(double.Parse(unixTimeStamp));
            return dtDateTime;
        }

        public string DateTimeToUnixTimeStamp(DateTime datetime)
        {
            return ((Int32)(datetime.Subtract(new DateTime(1970, 1, 1, 0, 0, 0, 0))).TotalSeconds).ToString();
        }
    }
}