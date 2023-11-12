using System;
using System.Collections.Generic;

namespace Mellon.Services.Api.Models;

public partial class CarriersProjectRestriction
{
    public int Id { get; set; }

    public string Project { get; set; } = null!;

    public string? Action { get; set; }

    public int CarrierId { get; set; }

    public virtual Carrier Carrier { get; set; } = null!;
}
