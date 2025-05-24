using Market.Domain.Common.Interfaces;

namespace Market.Domain.Common;

public abstract class BaseEntity : IEntity
{
    public int Id { get; set; }
}