using Microsoft.EntityFrameworkCore;

namespace Advantage.API.Models
{
    public class ApiContext: DbContext
    {
        public ApiContext(DbContextOptions<ApiContext> options) : base(options) { }

        public Dbset<Customer> Customers { get; set }
        public Dbset<Order> COrders { get; set }
        public Dbset<Server> Servers { get; set }
    }
}