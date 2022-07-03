TODO: 
- solve inventory reservation problem with another service implementing Reservation Bussines Archetype - when user collect items to cart products may be no longer availiable
- change from throwing ApplicationException to returnign OperationResult<T> object in Domain and Application layer, to improve performance

Assumption
- Domain.DiscountVoucher.ValueObjects.ExpirationDate - assume that DateTime is allways in UTC otherwise there should be Time Zone Name Id
- We do not consider currency (all items in catalog in same currency)