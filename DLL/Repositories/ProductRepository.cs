using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class ProductRepository: Repository<Product>
    {
        public ProductRepository(OnlineStoreContext context): base(context) { }

    }
}
