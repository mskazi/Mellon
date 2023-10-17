using System.ComponentModel.DataAnnotations;

namespace Mellon.Services.Infrastracture.Models;

public partial class ApprovalNotification
{
    public ApprovalNotification()
    {

    }
    public int Id { get; set; }
    public string DocumentType { get; set; }
    public string DocumentNo { get; set; }
    public string DocumentToken { get; set; }
    public string email { get; set; }
    public string DocumentOwner { get; set; }

    public DateTime NotificationSend { get; set; }

    public DateTime? NotificationCreated { get; set; }

    [Timestamp]
    public byte[] RowVersion { get; set; } = null!;
}
