using System;
using System.Collections.Generic;

namespace Mellon.Services.Infrastracture.Models;

public partial class Dim
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? ValueChar { get; set; }

    public int ValueInt { get; set; }

    public string Description { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string? SysCountry { get; set; }

    public string? Description2 { get; set; }
}
