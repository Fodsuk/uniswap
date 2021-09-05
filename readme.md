# Introduction

## The API
A .net API that provides 2 endpoints:

### Get all pools for an assert
List all the pools that have an asset id referenced.

example query:
`/assets/0xa0b86991c6218b36c1d19d4a2e9eb0ce3606eb48/pools`

example response:
```json
[
    {
        "id": "0x059b50bc9ea067394dcaf5f3de5a7227a8ae8907",
        "asset0Id": "0xa0b86991c6218b36c1d19d4a2e9eb0ce3606eb48",
        "asset0Name": "xKAWA",
        "asset1Id": "0xf2454d3c376f4244c8229b3d8498cee95ef40160",
        "asset1Name": "xKAWA"
    },
    {
        "id": "0x05eafb5ea2286a65b42dfe9de54ca0fdae2511c7",
        "asset0Id": "0xa0b86991c6218b36c1d19d4a2e9eb0ce3606eb48",
        "asset0Name": "Furucombo",
        "asset1Id": "0xffffffff2ba8f66d4e51811c5190992176930278",
        "asset1Name": "Furucombo"
    }
]
```


### Get total swap volume for an assert
A aggregated volume total on for all asset swaps.

example query:
`/assets/0xa0b86991c6218b36c1d19d4a2e9eb0ce3606eb48/swaps/totals?from=1629288060&to=1629288180`

example response:
```json
{
    "totalVolume": 2219673.126421
}
```

## Code Projects

`Uniswap.Api` - 
.Net API project that exposes the APIs

`UniswapClient` - 
.Net DLL library that provides the logic for calling the uniswap GraphQL API

`Uniswap.IntegrationTests` - Set of integration tests for the UniswapClient library