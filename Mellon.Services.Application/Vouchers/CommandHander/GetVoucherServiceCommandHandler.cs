using MediatR;
using Mellon.Common.Services;
using Mellon.Services.Application.Vouchers.Commands;
using Mellon.Services.Common.interfaces;
using Mellon.Services.Common.resources;
using Mellon.Services.Infrastracture.Repositotiries;
using Microsoft.Extensions.Logging;

namespace Mellon.Services.Application.Vouchers.CommandHander
{
    public class GetVoucherServiceCommandHandler : IRequestHandler<GetVoucherServiceCommand, PaginatedListResult<VoucherServiceItem>>
    {
        private readonly ILogger logger;
        private readonly ICurrentUserService currentUserService;
        private readonly IVouchersRepository repository;

        public GetVoucherServiceCommandHandler(ICurrentUserService currentUserService, ILogger<GetVoucherServiceCommandHandler> logger, IVouchersRepository repository)
        {
            this.currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
            this.logger = logger;
            this.repository = repository;
        }

        public async Task<PaginatedListResult<VoucherServiceItem>> Handle(GetVoucherServiceCommand request, CancellationToken cancellationToken)
        {
            return await repository.ServiceVouchers(request.Term, request.Paging, request.Ordering, cancellationToken);
        }
    }



}
