using System;
using System.Collections.Generic;

namespace Mellon.Services.Infrastracture.Models;

public partial class DimDate
{
    public DateTime? Date { get; set; }

    public bool? Isholiday { get; set; }

    public int Id { get; set; }
}
