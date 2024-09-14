
using System.ComponentModel.DataAnnotations;

namespace OnlineStoreAPI
{
    /// <summary>
    ///  This's a Product DTO class used in purpose of adding product and not to set optional property( use not need to know all about product) 
    /// </summary>
    public class ProductCreateDto
    {
        public string Name { get; set; }

        /// <summary>
        /// Business Says that the price must be more than 0.01 and there's no maximum value ( limit ) 
        /// </summary>
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0.")]
        public decimal Price { get; set; }
        /// <summary>
        /// Business Says That the product UnitInStock must be between 0 and 10,000 
        /// </summary>
        [Range(0, 10000, ErrorMessage = "Stock must be between 0 and 10,000.")]
        public int QuantityInStock { get; set; }
    }
    /// <summary>
    ///  This's a Product DTO class used in purpose of Updating product and not to set optional property( use not need to know all about product) 
    /// </summary>
    public class ProductUpdateDto
    {
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0.")]

        public decimal Price { get; set; }
        [Range(0, 10000, ErrorMessage = "Stock must be between 0 and 10,000.")]

        public int QuantityInStock { get; set; }
    }

}
