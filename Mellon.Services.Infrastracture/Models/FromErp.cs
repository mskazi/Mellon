using System;
using System.Collections.Generic;

namespace Mellon.Services.Infrastracture.Models;

public partial class FromErp
{
    public int Id { get; set; }

    public string? No { get; set; }

    public string? CarrierFlag { get; set; }

    public string? ServiceOrderNo { get; set; }

    public string? SerialNo { get; set; }

    public string? ServiceOrderType { get; set; }

    public string? SellToCustomerNo { get; set; }

    public string? CustomerName { get; set; }

    public string? BillToCustomerNo { get; set; }

    public string? ShipToName { get; set; }

    public string? ShipToName2 { get; set; }

    public string? ShipToAddress { get; set; }

    public string? ShipToAddress2 { get; set; }

    public string? ShipToCity { get; set; }

    public string? ShipToContact { get; set; }

    public string? OrderDate { get; set; }

    public string? ShipToPostCode { get; set; }

    public string? ShipToCounty { get; set; }

    public string? ShipToCountryCode { get; set; }

    public string? ShipmentDate { get; set; }

    public string? PostingDescription { get; set; }

    public string? ShipmentMethodCode { get; set; }

    public string? DeliveryNoteNo { get; set; }

    public string? ShipToPhone { get; set; }

    public string? TerminalId { get; set; }

    public string? ShipToMobile { get; set; }

    public string? MellonCourierProject { get; set; }

    public string? CourierServiceType { get; set; }

    public string? LinkedDocNo { get; set; }

    public string? LinkedPostDate { get; set; }
}
