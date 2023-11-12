using System;
using System.Collections.Generic;

namespace Mellon.Services.Api.Models;

public partial class DataLine
{
    public int Id { get; set; }

    public int DataId { get; set; }

    public string Name { get; set; } = null!;

    public string Value { get; set; } = null!;

    public string CreatedBy { get; set; } = null!;

    public string UpdatedBy { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Datum Data { get; set; } = null!;
}
