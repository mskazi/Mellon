using System;
using System.Collections.Generic;

namespace Mellon.Services.Api.Models;

public partial class DataCancellation
{
    public int Id { get; set; }

    public int ElectraId { get; set; }

    public string? CancellationStatus { get; set; }

    public string? CancellationReason { get; set; }

    public DateTime? CreatedAt { get; set; }
}
