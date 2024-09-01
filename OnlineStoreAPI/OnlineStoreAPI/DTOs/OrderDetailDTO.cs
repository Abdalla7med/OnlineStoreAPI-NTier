namespace OnlineStoreAPI
{
    public class OrderDetailDTO
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        //public decimal PriceAtPurchase { get; set; } calculated according to Business logic 
    }
}
