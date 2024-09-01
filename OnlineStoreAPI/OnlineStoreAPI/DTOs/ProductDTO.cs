
namespace OnlineStoreAPI
{
    /// <summary>
    ///  This's a Product DTO class used in purpose of adding product and not to set optional property( use not need to know all about product) 
    /// </summary>
    public class ProductCreateDto
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int QuantityInStock { get; set; }
    }
    /// <summary>
    ///  This's a Product DTO class used in purpose of Updating product and not to set optional property( use not need to know all about product) 
    /// </summary>
    public class ProductUpdateDto
    {
        public decimal Price { get; set; }
        public int QuantityInStock { get; set; }
    }

}
