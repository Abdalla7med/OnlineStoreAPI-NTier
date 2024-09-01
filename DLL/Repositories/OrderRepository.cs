using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class OrderRepository:Repository<Order>
    {

       public OrderRepository(OnlineStoreContext context) : base(context) { }
    }
}
