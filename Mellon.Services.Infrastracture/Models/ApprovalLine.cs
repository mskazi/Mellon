using System.ComponentModel.DataAnnotations;

namespace Mellon.Services.Infrastracture.Models
{
    public class ApprovalLine
    {
        public int Id { get; set; }
        public string? ERPCompany { get; set; }
        public string DocumentType { get; set; }
        public string DocumentNo { get; set; }
        public string? LineNo { get; set; }
        public string? Description { get; set; }
        public double? LineAmount { get; set; }
        public string? BU { get; set; }
        public string? BUName { get; set; }
        public string? BL { get; set; }
        public string? BLName { get; set; }
        public string? PLLines { get; set; }
        public string? PLLinesName { get; set; }
        public string? Ergo { get; set; }
        public string? ErgoName { get; set; }
        public double? UnitPrice { get; set; }
        public double? Quantity { get; set; }
        public string? ERPTimeStamp { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; } = null!;

    }
}
