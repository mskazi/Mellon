using System;
using System.Collections.Generic;

namespace Mellon.Services.Infrastracture.Models;

public partial class DataInability
{
    public int Id { get; set; }

    public int? DataId { get; set; }

    public DateTime? TrnDate { get; set; }

    public string? Reason { get; set; }
}
