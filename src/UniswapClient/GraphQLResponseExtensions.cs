using GraphQL;
using System.Collections.Generic;

namespace UniswapClient
{
    public static class GraphQLResponseExtensions
    {
        public static void MergeGraphQLResponses<TData, TItem>(this GraphQLResponse<TData> response, GraphQLResponse<TData> response1, GraphQLResponse<TData> response2) where TData : IUniswapData<TItem>, new()
        {
            response.Data.Items.AddRange(response1.Data.Items);
            response.Data.Items.AddRange(response2.Data.Items);

            var allErrors = new List<GraphQLError>();

            if (response1.Errors != null) { allErrors.AddRange(response1.Errors); }
            if (response2.Errors != null) { allErrors.AddRange(response2.Errors); }

            if (allErrors.Count > 0)
                response.Errors = allErrors.ToArray();

            // return response;
        }
    }
}
