using Mellon.Services.Infrastracture.ModelExtensions;
using System.ComponentModel.DataAnnotations;

namespace Mellon.Services.Infrastracture.Models;

public partial class Approval
{
    public int Id { get; set; }
    public string ERPCountry { get; set; }
    public string ERPCompany { get; set; }
    public string DocumentType { get; set; }
    public string DocumentNo { get; set; }
    public int ApprovalSequence { get; set; }
    public string DocumentOwner { get; set; }
    public int RequestNo { get; set; }
    public string ApprovalResponsible { get; set; }
    public string email { get; set; }
    public string DocumentOwnerEmail { get; set; }
    public ApprovalStatusEnum Status { get; set; }
    public string DocumentToken { get; set; }
    public string ERPTimeStamp { get; set; }
    public string ApprovalProcessComment { get; set; }
    public string ApprovalRequestComment { get; set; }
    [Timestamp]
    public byte[] RowVersion { get; set; } = null!;

    public int ApprovalOrderId { get; set; }



}
