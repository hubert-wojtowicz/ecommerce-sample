using Common.Domain.ValueObjects.Identifiers;
using System;

namespace Domain.ShoppingCartSnapshot.ValueObjects
{
    public class ShoppingCartSnapshotId : GuidIdValueObject
    {
        public ShoppingCartSnapshotId() : base()
        {
        }

        public ShoppingCartSnapshotId(Guid id) : base(id)
        {
        }

        public static ShoppingCartSnapshotId CreateOrNull(Guid? id) => id is null ? null : new ShoppingCartSnapshotId(id.Value);
    }
}
