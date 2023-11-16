using MediatR;
using Mellon.Common.Services;
using Mellon.Services.Common.resources;

namespace Mellon.Services.Application.Vouchers.Commands
{

    public class GetVoucherDetailsCommand : IRequest<VoucherDetails>
    {
        public GetVoucherDetailsCommand(int id)
        {
            Id = id;
        }
        public int Id { get; set; }
    }

    public class GetVoucherSearchCommand : IRequest<PaginatedListResult<VoucherSearchItem>>
    {
        public GetVoucherSearchCommand(string term, ListPaging paging, ListOrder ordering)
        {
            Term = term;
            Paging = paging;
            Ordering = ordering;
        }
        public string? Term { get; set; }
        public ListPaging Paging { get; }
        public ListOrder Ordering { get; }
    }

    public class GetSummaryCommand : IRequest<VoucherSummary>
    {

    }

    public class GetVoucherTrackCommand : IRequest<VoucherTrack>
    {
        public GetVoucherTrackCommand(int id)
        {
            Id = id;
        }
        public int Id { get; set; }
    }

    public class GetVoucherPrintCommand : IRequest<Stream>
    {
        public GetVoucherPrintCommand(int id)
        {
            Id = id;
        }
        public int Id { get; set; }
    }

    public class GetVoucherCancelCommand : IRequest<string>
    {
        public GetVoucherCancelCommand(int id)
        {
            Id = id;
        }
        public int Id { get; set; }
    }


}
