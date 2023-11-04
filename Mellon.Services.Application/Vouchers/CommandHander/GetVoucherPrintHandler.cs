using MediatR;
using Mellon.Services.Application.Vouchers.Commands;
using Mellon.Services.Common.interfaces;
using Mellon.Services.Infrastracture.Repositotiries;
using Microsoft.Extensions.Logging;

namespace Mellon.Services.Application.Vouchers.CommandHander
{
    public class GetVoucherPrintHandler : IRequestHandler<GetVoucherPrintCommand, VoucherTrack>


    {
        private readonly ILogger logger;
        private readonly ICurrentUserService currentUserService;
        private readonly IVouchersRepository repository;

        public GetVoucherPrintHandler(ICurrentUserService currentUserService, ILogger<VoucherDetailCommandHandler> logger, IVouchersRepository repository)
        {
            this.currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
            this.logger = logger;
            this.repository = repository;
        }

        public async Task<VoucherTrack> Handle(GetVoucherPrintCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }


    }
}