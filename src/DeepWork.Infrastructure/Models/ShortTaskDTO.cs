﻿using DeepWork.Domain.Common;
using SQLite;

namespace DeepWork.Infrastructure.Models;

[Table("short-tasks")]
public class ShortTaskDTO : HasDomainEventBase
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    [MaxLength(255)]
    public string Name { get; set; }

    public string? Description { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    [Indexed]
    public int LongTaskId { get; set; }
}