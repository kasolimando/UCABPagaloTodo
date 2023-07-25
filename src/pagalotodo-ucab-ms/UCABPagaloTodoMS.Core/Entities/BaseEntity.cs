using System.Diagnostics.CodeAnalysis;

namespace UCABPagaloTodoMS.Core.Entities;

public class BaseEntity
{
    [ExcludeFromCodeCoverage]
    public Guid Id { get; set; }

}
