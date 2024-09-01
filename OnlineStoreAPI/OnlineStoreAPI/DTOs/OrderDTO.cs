using DAL;

namespace OnlineStoreAPI
{
    public class OrderCreateDTO
    {
        public OrderStatus Status { get; set; }
        public int? CustomerId { get; set; } // Optional Foreign Key
        public List<OrderDetailDTO> OrderDetails { get; set; } // List of Order Details

    }

    public class OrderUpdateDTO
    {
        public OrderStatus Status { get; set; }
    }
}
