using MediatR;
using Mellon.Services.Application.Vouchers.Commands;
using Mellon.Services.Common.interfaces;
using Mellon.Services.External.CourierProviders;
using Mellon.Services.Infrastracture.Repositotiries;
using Microsoft.Extensions.Logging;

namespace Mellon.Services.Application.Vouchers.CommandHander
{
    public class GetVoucherPrintHandler : IRequestHandler<GetVoucherPrintCommand, Stream>
    {
        private readonly ILogger logger;
        private readonly ICurrentUserService currentUserService;
        private readonly IVouchersRepository repository;
        private readonly CourierServiceFactory courierServiceFactory;
        private readonly ILookupRepository lookupRepository;


        public GetVoucherPrintHandler(ICurrentUserService currentUserService, ILogger<VoucherDetailCommandHandler> logger, CourierServiceFactory courierServiceFactory, ILookupRepository lookupRepository, IVouchersRepository repository)
        {
            this.currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
            this.logger = logger;
            this.repository = repository;
            this.courierServiceFactory = courierServiceFactory;
            this.lookupRepository = lookupRepository;

        }

        public async Task<Stream> Handle(GetVoucherPrintCommand request, CancellationToken cancellationToken)
        {

            var voucherDetails = await this.repository.VoucherDetails(request.Id, cancellationToken);
            if (voucherDetails == null)
            {
                throw new ArgumentNullException(nameof(voucherDetails));
            }
            var project = await this.lookupRepository.GetElectraProjectSetup(voucherDetails.CarrierId, voucherDetails.ElectraProjectId, cancellationToken);
            if (project == null)
            {
                throw new ArgumentNullException(nameof(project));
            }

            ICourierService courierService = this.courierServiceFactory.GetCourierService((CourierMode)voucherDetails.CarrierId, project);
            if (courierService == null)
            {
                throw new ArgumentNullException(nameof(courierService));
            }
            var vouchers = new List<string>();
            vouchers.Add(voucherDetails.CarrierVoucherNo);
            return await courierService.Print(vouchers.AsEnumerable(), cancellationToken);
        }


    }
}