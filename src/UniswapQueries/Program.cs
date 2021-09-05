using GraphQL;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using System;
using System.Collections.Generic;

namespace UniswapQueries
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var personAndFilmsRequest = new GraphQLRequest
            {
                Query = @"
                query PoolByTokenId($id: ID)  {
                    pools(where:{ token1: $id } ) {
                    id
                    }
                }",
                OperationName = "PoolByTokenId",
                Variables = new
                {
                    id = "0xa0b86991c6218b36c1d19d4a2e9eb0ce3606eb48"
                }
            };

            var graphQLClient = new GraphQLHttpClient("https://api.thegraph.com/subgraphs/name/ianlapham/uniswap-v3-alt", new NewtonsoftJsonSerializer());

            var outcome = graphQLClient.SendQueryAsync<Result>(personAndFilmsRequest).Result;

        }
    }

    public class Result
    {
        public List<Pool> pools { get; set; }
    }

    public class Pool {
        public string Id { get; set; }

    }
}
