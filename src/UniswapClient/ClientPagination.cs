using GraphQL;
using GraphQL.Client.Http;
using System.Collections.Generic;
using System.Dynamic;
using System.Threading.Tasks;

namespace UniswapClient
{
    public class ClientPagination
    {
        private readonly GraphQLHttpClient _client;
        public ClientPagination(GraphQLHttpClient client)
        {
            _client = client;
        }
      
        public async Task<GraphQLResponse<TData>> GetItems<TData, TItem>(string query, string operation, IDictionary<string, object> variables) where TData : IUniswapData<TItem>, new()
        {
            var count = 100;
            int skip = 0;
            var errors = new List<GraphQLError>();
            var data = new TData();

            while (true)
            {
                variables["first"] = count;
                variables["skip"] = skip;

                var request = new GraphQLRequest(query: query, operationName: operation, variables: variables);

                var items = await _client.SendQueryAsync<TData>(request);

                if (items.Errors != null)
                {
                    errors.AddRange(items.Errors);
                    break;
                }

                data.Items.AddRange(items.Data.Items);

                skip += count;

                if (items.Data.Items.Count < count)
                    break;
            }

            return new GraphQLResponse<TData>()
            {
                Data = data,
                Errors = errors.Count > 0 ? errors.ToArray() : null
            };
        }

    }
}
