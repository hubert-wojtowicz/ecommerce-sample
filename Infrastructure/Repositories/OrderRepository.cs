using Domain.Order;
using Domain.Order.ValueObjects;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class OrderRepository : Repository<Order, OrderId>, IOrderRepository
    {
        public async Task<int> GetNextOrderNumber()
        {
            if (!Collection.Any()) return 1;
            return Collection.Max(x => x.OrderNumber) + 1;
        }
    }
}
