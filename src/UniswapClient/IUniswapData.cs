using System.Collections.Generic;

namespace UniswapClient
{
    public interface IUniswapData<TItem>
    {
        List<TItem> Items { get; }
    }
}
