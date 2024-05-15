using DeepWork.Domain.Common;
using SQLite;

namespace DeepWork.Domain.Entities;

[Table("long-Tasks")]
public class LongTask : HasDomainEventBase
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    [MaxLength(255)]
    public string Name { get; set; }

    public string? Description { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }
}
