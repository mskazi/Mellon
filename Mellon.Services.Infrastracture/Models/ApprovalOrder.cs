using System.ComponentModel.DataAnnotations;

namespace Mellon.Services.Infrastracture.Models;

public partial class ApprovalOrder
{
    public ApprovalOrder()
    {
        Approvals = new HashSet<Approval>();
        ApprovalLines = new HashSet<ApprovalLine>();
    }
    public int Id { get; set; }

    public string? ERPCountry { get; set; }

    public string? ERPCompany { get; set; }

    public string? DocumentNo { get; set; }

    public string? DocumentType { get; set; }

    public string? SourceName { get; set; }

    public DateTime? OrderDate { get; set; }

    public string? Bu { get; set; }

    public string? BUName { get; set; }

    public string? BL { get; set; }

    public string? BLName { get; set; }

    public string? PLLines { get; set; }

    public string? PLLinesName { get; set; }

    public string? Ergo { get; set; }

    public string? ErgoName { get; set; }

    public string? ERPTimeStamp { get; set; }

    public string? NotificationMail { get; set; }

    public string? Currency { get; set; }

    [Timestamp]
    public byte[] RowVersion { get; set; } = null!;

    public virtual ICollection<Approval> Approvals { get; set; }

    public virtual ICollection<ApprovalLine> ApprovalLines { get; set; }

}
