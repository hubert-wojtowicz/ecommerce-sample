using Domain.ShoppingCart;
using Domain.ShoppingCartSnapshot;
using Domain.ShoppingCartSnapshot.ValueObjects;

namespace Infrastructure.Repositories
{
    public class ShoppingCartSnapshotRepository : Repository<ShoppingCartSnapshot, ShoppingCartSnapshotId>, IShoppingCartSnapshotRepository
    {
    }
}
