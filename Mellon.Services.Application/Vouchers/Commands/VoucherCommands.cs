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

    public class GetVoucherDetailsCommand : IRequest<VoucherDetails>
    {
        public GetVoucherDetailsCommand(int id)
        {
            Id = id;
        }
        public int Id { get; set; }
    }
}
