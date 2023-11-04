using MediatR;
using Mellon.Common.Services;
using Mellon.Services.Application.Vouchers.Commands;
using Mellon.Services.Common.interfaces;
using Mellon.Services.Common.resources;
using Mellon.Services.Infrastracture.Repositotiries;
using Microsoft.Extensions.Logging;

namespace Mellon.Services.Application.Vouchers.CommandHander
{
    public class VoucherDetailCommandHandler : IRequestHandler<GetVoucherDetailsCommand, VoucherDetails>,
        IRequestHandler<GetVoucherSearchCommand, PaginatedListResult<VoucherSearchItem>>,
        IRequestHandler<GetSummaryCommand, VoucherSummary>

    {
        private readonly ILogger logger;
        private readonly ICurrentUserService currentUserService;
        private readonly IVouchersRepository repository;

        public VoucherDetailCommandHandler(ICurrentUserService currentUserService, ILogger<VoucherDetailCommandHandler> logger, IVouchersRepository repository)
        {
            this.currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
            this.logger = logger;
            this.repository = repository;
        }

        public async Task<VoucherDetails> Handle(GetVoucherDetailsCommand request, CancellationToken cancellationToken)
        {

            return await this.repository.VoucherDetails(request.Id, cancellationToken);
        }

        public async Task<PaginatedListResult<VoucherSearchItem>> Handle(GetVoucherSearchCommand request, CancellationToken cancellationToken)
        {
            return await repository.Search(request.Term, request.Paging, request.Ordering, cancellationToken);
        }

        public async Task<VoucherSummary> Handle(GetSummaryCommand request, CancellationToken cancellationToken)
        {
            return await repository.Summary(cancellationToken);
        }
    }
}