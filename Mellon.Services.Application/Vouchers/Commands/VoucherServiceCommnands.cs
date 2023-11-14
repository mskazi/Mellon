using MediatR;
using Mellon.Common.Services;
using Mellon.Services.Common.resources;

namespace Mellon.Services.Application.Vouchers.Commands
{
    public class GetVoucherServiceCommand : IRequest<PaginatedListResult<VoucherServiceItem>>
    {
        public GetVoucherServiceCommand(string term, ListPaging paging, ListOrder ordering)
        {
            Term = term;
            Paging = paging;
            Ordering = ordering;
        }
        public string? Term { get; set; }
        public ListPaging Paging { get; }
        public ListOrder Ordering { get; }
    }

    public class CreateVoucherFromServiceScanCommand : CreateVoucherServiceScanData, IRequest<VoucherDetails>
    {
        public CreateVoucherFromServiceScanCommand(CreateVoucherServiceScanData data)
        {
            ScanSerial = data.ScanSerial;
            VoucherSaturdayDelivery = data.VoucherSaturdayDelivery;
            VoucherDeliveryTime = data.VoucherDeliveryTime;
            Validate();
        }
        protected override void Validate()
        {
            base.Validate();
        }
    }

    public class GetVoucherServiceOrderInfoCommand : IRequest<VoucherServiceOrderResource>
    {
        public GetVoucherServiceOrderInfoCommand(string order)
        {
            Order = order;
            Validate();
        }

        public string Order { get; set; }

        protected virtual void Validate()
        {
            Guards.StringNotNullOrEmpty(Order, nameof(Order));
        }
    }

    public class CreateVoucherServiceOrderCommand : IRequest<VoucherDetails>
    {
        public CreateVoucherServiceOrderCommand(string order)
        {
            Order = order;
            Validate();
        }

        public string Order { get; set; }

        protected virtual void Validate()
        {
            Guards.StringNotNullOrEmpty(Order, nameof(Order));
        }
    }

}
