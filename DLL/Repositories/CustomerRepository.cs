using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DAL
{
    /// <summary>
    /// Customers Management:
    ///  Create Customer: The system should allow for the creation of customer profiles, including details like name, email, and phone number.
    /// Update Customer: The system should allow updating existing customer details.
    /// Retrieve Customers: The system should allow retrieving customer profiles, including filtering by name, email, and registration date.
    /// Delete Customer: The system should allow for the removal of customer profiles.
    /// </summary>
    public class CustomerRepository : Repository<Customer>
    {
        /// Create Customer Repo
        public CustomerRepository(OnlineStoreContext context) : base(context)
        {

        }
    }
}
