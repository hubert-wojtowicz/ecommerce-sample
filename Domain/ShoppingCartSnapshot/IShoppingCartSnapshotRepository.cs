using Common.Domain;
using Domain.ShoppingCartSnapshot.ValueObjects;

namespace Domain.ShoppingCartSnapshot
{
    public interface IShoppingCartSnapshotRepository : IRepository<ShoppingCartSnapshot, ShoppingCartSnapshotId>
    {
    }
}
