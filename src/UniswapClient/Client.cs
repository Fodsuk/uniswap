using GraphQL;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UniswapClient
{

    public class Client
    {
        private readonly GraphQLHttpClient _client;
        private readonly ClientPagination _pagination;

        public Client(Uri uniswapGraphQLEndpoint)
        {
            var _uniswapGraphQLEndpoint = uniswapGraphQLEndpoint;

            _client = new GraphQLHttpClient($"{_uniswapGraphQLEndpoint.AbsoluteUri}", new NewtonsoftJsonSerializer());

            _pagination = new ClientPagination(_client);
        }

        public async Task<GraphQLResponse<Pools>> GetPoolsByToken(string tokenId)
        {
            // according to the documentation https://thegraph.com/docs/developer/graphql-api#filtering and testing
            // thegraph.com doesn't look to support where: { _or: [] } logic
            // as a temporary measure, i have queries both properties asynchronously

            var token0 = GetPoolsByTokenName(tokenId, "token0");
            var token1 = GetPoolsByTokenName(tokenId, "token1");

            var response = new GraphQLResponse<Pools>();
            response.Data = new Pools();

            await Task.WhenAll(token0, token1).ContinueWith(x =>
            {
                response.MergeGraphQLResponses<Pools, PoolId>(x.Result[0], x.Result[1]);
            });

            return await Task.FromResult(response);
        }

        public async Task<GraphQLResponse<Pools>> GetPoolsByTokenName(string assetId, string tokenName)
        {
            var query = @"
                    query PoolByTokenId($first: Int, $skip: Int, $id: ID)  {
                        pools(first: $first, skip: $skip, orderBy: id, where:{ " + tokenName + @": $id } ) {
                            id
                            token0 {
                            id
                            name
                            }
                            token1 {
                                id
                                name
                            }
                        }
                    }";
            var operation = "PoolByTokenId";
            var variables = new Dictionary<string, object>();
            variables["id"] = assetId;

            var response = await _pagination.GetItems<Pools, PoolId>(query, operation, variables);

            return response;
        }

        public async Task<GraphQLResponse<Swaps>> GetSwapsByRangeAndToken(string unixFrom, string unixTo, string assetId)
        {
            // according to the documentation https://thegraph.com/docs/developer/graphql-api#filtering and testing
            // thegraph.com doesn't look to support where: { _or: [] } logic
            // as a temporary measure, i have queries both token0 and token1 properties asynchronously

            var token0 = GetSwapsByRangeAndTokenName(unixFrom, unixTo, assetId, "token0");
            var token1 = GetSwapsByRangeAndTokenName(unixFrom, unixTo, assetId, "token1");

            var response = new GraphQLResponse<Swaps>();
            response.Data = new Swaps();

            await Task.WhenAll(token0, token1).ContinueWith(x =>
            {
                response.MergeGraphQLResponses<Swaps, Swap>(x.Result[0], x.Result[1]);
            });

            return await Task.FromResult(response);
        }

        public async Task<GraphQLResponse<Swaps>> GetSwapsByRangeAndTokenName(string unixFrom, string unixTo, string assetId, string tokenName)
        {
            var query = @"
                    query SwapsByRangeAndTokenId($from: BigInt, $to: BigInt, $id: ID)  {
                        swaps(first: $first, skip: $skip, orderBy: id, where:{ timestamp_gt: $from, timestamp_lt: $to, " + tokenName + @": $id } ) {
                            id
                            timestamp
                            amountUSD
                            amount0
                            amount1
                            token0 {
                            id
                            name
                            }
                            token1 {
                                id
                                name
                            }
                        }
                    }";
            var operation = "SwapsByRangeAndTokenId";
            var variables = new Dictionary<string, object>();
            variables["id"] = assetId;
            variables["from"] = unixFrom;
            variables["to"] = unixTo;

            var response = await _pagination.GetItems<Swaps, Swap>(query, operation, variables);

            return response;
        }
    }
}
