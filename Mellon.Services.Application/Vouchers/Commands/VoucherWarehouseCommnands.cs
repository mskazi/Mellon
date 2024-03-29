﻿using MediatR;
using Mellon.Common.Services;
using Mellon.Services.Common.resources;

namespace Mellon.Services.Application.Vouchers.Commands
{
    public class GetVoucherWarehouseCommand : IRequest<PaginatedListResult<VoucherWarehouseItem>>
    {
        public GetVoucherWarehouseCommand(string term, ListPaging paging, ListOrder ordering)
        {
            Term = term;
            Paging = paging;
            Ordering = ordering;
        }
        public string? Term { get; set; }
        public ListPaging Paging { get; }
        public ListOrder Ordering { get; }
    }
}
