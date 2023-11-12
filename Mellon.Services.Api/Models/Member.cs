using System;
using System.Collections.Generic;

namespace Mellon.Services.Api.Models;

public partial class Member
{
    public int Id { get; set; }

    public string Company { get; set; } = null!;

    public string Department { get; set; } = null!;

    public string MemberName { get; set; } = null!;

    public bool IsActive { get; set; }

    public string SysCountry { get; set; } = null!;

    public string CreatedBy { get; set; } = null!;

    public string UpdatedBy { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
