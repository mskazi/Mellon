using System;
using System.Collections.Generic;

namespace Mellon.Services.Infrastracture.Models;

public partial class CarriersPostcodeRestriction
{
    public int Id { get; set; }

    public int? CarrierId { get; set; }

    public string PostCode { get; set; } = null!;

    public virtual Carrier? Carrier { get; set; }
}
