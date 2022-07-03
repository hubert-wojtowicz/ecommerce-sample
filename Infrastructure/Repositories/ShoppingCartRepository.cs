using System;
using System.Linq;
using System.Threading.Tasks;
using Common.Domain.User.ValueObjects;
using Domain.ShoppingCart;
using Domain.ShoppingCart.ValueObjects;

namespace Infrastructure.Repositories
{
    public class ShoppingCartRepository : Repository<ShoppingCart, ShoppingCartId>, IShoppingCartRepository
    {
        public ShoppingCartRepository()
        {
            Collection.Add(new ShoppingCart(UserId.CreateOrNull(Guid.Parse("f91a6abd-2f61-4549-95f0-0527c69189f4"))));
        }

        public async Task<ShoppingCart> GetByUserIdAsync(UserId userId)
        {
            var shoppingCart = Collection.SingleOrDefault(x => x.UserId.Equals(userId));
            if (shoppingCart is null)
            {
                throw new ApplicationException($"Cannot find shopping cart for user: {userId}");
            }

            return shoppingCart;
        }
    }
}
