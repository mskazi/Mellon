using MediatR;
using Mellon.Common.Services;
using Mellon.Services.Common.resources;

namespace Mellon.Services.Application.Vouchers.Commands
{
    public class GetVoucherOfficeCommand : IRequest<PaginatedListResult<VoucherOfficeItem>>
    {
        public GetVoucherOfficeCommand(string term, ListPaging paging, ListOrder ordering)
        {
            Term = term;
            Paging = paging;
            Ordering = ordering;
        }
        public string? Term { get; set; }
        public ListPaging Paging { get; }
        public ListOrder Ordering { get; }
    }

    public class CreateVoucherOfficeCommand : CreateVoucherRequestData, IRequest<VoucherDetails>
    {
        public CreateVoucherOfficeCommand(int contactId, CreateVoucherRequestData data)
        {
            ContactId = contactId;
            VoucherAction = data.VoucherAction;
            VoucherCarrier = data.VoucherCarrier;
            VoucherCompany = data.VoucherCompany;
            VoucherDepartment = data.VoucherDepartment;
            VoucherMember = data.VoucherMember;
            VoucherType = data.VoucherType;
            VoucherDeliverTo = data.VoucherDeliverTo;
            VoucherWeight = data.VoucherAction;
            VoucherQuantities = data.VoucherQuantities;
            Comments = data.Comments;
            VoucherReference = data.VoucherReference;
            VoucherCondition = data.VoucherCondition;
            VoucherCodAmount = data.VoucherCodAmount;
            VoucherSaturdayDelivery = data.VoucherSaturdayDelivery;
            VoucherDeliveryTime = data.VoucherDeliveryTime;
            Validate();
        }
        public int ContactId { get; set; }

        protected override void Validate()
        {
            base.Validate();
            Guards.ValidIdentifier(ContactId);
        }
    }


}
