using System;
using System.Collections.Generic;

namespace Mellon.Services.Infrastracture.Models;

public partial class ElectraProjectSetup
{
    public int Id { get; set; }

    public string SysCompany { get; set; } = null!;

    public string SysDepartment { get; set; } = null!;

    public int CarrierId { get; set; }

    public string MellonProject { get; set; } = null!;

    public string MellonProjectMaster { get; set; } = null!;

    public string? CarrierUsername { get; set; }

    public string? CarrierPassword { get; set; }

    public string? CarrierCode { get; set; }

    public string? CarrierKey { get; set; }

    public bool IsPallet { get; set; }

    public bool IsActive { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string? SysCountry { get; set; }
}
