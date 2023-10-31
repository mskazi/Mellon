using MediatR;
using Mellon.Common.Services;
using Mellon.Services.Application.Vouchers;
using Mellon.Services.Common.interfaces;
using Mellon.Services.Common.resources;
using Mellon.Services.Infrastracture.Repositotiries;
using Microsoft.Extensions.Logging;

namespace Mellon.Services.Application.Members.ApprovalHandlers
{
    public class VoucherServiceCommandHandler : IRequestHandler<GetVoucherServiceCommand, PaginatedListResult<VoucherServiceItem>>
    {
        private readonly ILogger logger;
        private readonly ICurrentUserService currentUserService;
        private readonly IVouchersRepository repository;

        public VoucherServiceCommandHandler(ICurrentUserService currentUserService, ILogger<VoucherServiceCommandHandler> logger, IVouchersRepository repository)
        {
            this.currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
            this.logger = logger;
            this.repository = repository;
        }

        public async Task<PaginatedListResult<VoucherServiceItem>> Handle(GetVoucherServiceCommand request, CancellationToken cancellationToken)
        {
            return await this.repository.ServiceVouchers(request.Term, request.Paging, request.Ordering, cancellationToken);
        }
    }


    public class VoucherDetailCommandHandler : IRequestHandler<GetVoucherDetailsCommand, VoucherDetails>
    {
        private readonly ILogger logger;
        private readonly ICurrentUserService currentUserService;
        private readonly IVouchersRepository repository;

        public VoucherDetailCommandHandler(ICurrentUserService currentUserService, ILogger<VoucherServiceCommandHandler> logger, IVouchersRepository repository)
        {
            this.currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
            this.logger = logger;
            this.repository = repository;
        }

        public async Task<VoucherDetails> Handle(GetVoucherDetailsCommand request, CancellationToken cancellationToken)
        {

            return await this.repository.VoucherDetails(request.Id, cancellationToken);
        }
    }

}
