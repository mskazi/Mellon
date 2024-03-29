﻿namespace Mellon.Services.Common.resources.Couriers
{
    public class CourierTrackResource
    {
        public IEnumerable<CourierTrackDetailsResource> Details { get; set; }
        public string Status { get; set; }
        public string DeliveredAt { get; set; }
        public DateTime DeliveryDate { get; set; }
    }

    public class CourierTrackDetailsResource
    {
        public DateTime StatusDate { get; set; }
        public string Position { get; set; }
        public string Status { get; set; }
    }

    public class CourierCreateResource
    {
        public string VoucherNo { get; set; }
        public long JobID { get; set; }

        public IEnumerable<CourierCreateSubVoucherResource> SubVouchers { get; set; }

    }

    public class CourierCreateSubVoucherResource
    {
        public string VoucherNo { get; set; }
        public string BelongsTo { get; set; }

    }
}
