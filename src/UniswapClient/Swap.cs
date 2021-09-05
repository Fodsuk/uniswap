namespace UniswapClient
{
    public class Swap
    {
        public string Id { get; set; } 
        public string Timestamp { get; set; } 
        public decimal AmountUSD { get; set; } 
        public decimal Amount0 { get; set; }
        public decimal Amount1 { get; set; }
        public Token Token0 { get; set; }
        public Token Token1 { get; set; }
    }
}
