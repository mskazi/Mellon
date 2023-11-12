using System;
using System.Collections.Generic;

namespace Mellon.Services.Api.Models;

public partial class Country
{
    public string Iso { get; set; } = null!;

    public string Country1 { get; set; } = null!;

    public int? Id { get; set; }
}
