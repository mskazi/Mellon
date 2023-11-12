using System;
using System.Collections.Generic;

namespace Mellon.Services.Infrastracture.Models;

public partial class Carrier
{
    public int Id { get; set; }

    public string DescrLong { get; set; } = null!;

    public string DescrShort { get; set; } = null!;

    public bool Active { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string? SysCountry { get; set; }

    public int? OrFlag { get; set; }

    public virtual ICollection<CarriersPostcodeRestriction> CarriersPostcodeRestrictions { get; set; } = new List<CarriersPostcodeRestriction>();

    public virtual ICollection<CarriersProjectRestriction> CarriersProjectRestrictions { get; set; } = new List<CarriersProjectRestriction>();

    public virtual ICollection<Datum> Data { get; set; } = new List<Datum>();

    public virtual ICollection<DataUpload> DataUploads { get; set; } = new List<DataUpload>();
}
