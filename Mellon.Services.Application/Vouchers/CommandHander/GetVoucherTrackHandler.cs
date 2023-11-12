using MediatR;
using Mellon.Services.Application.Vouchers.Commands;
using Mellon.Services.Common.interfaces;
using Mellon.Services.External.CourierProviders;
using Mellon.Services.Infrastracture.Repositotiries;
using Microsoft.Extensions.Logging;

namespace Mellon.Services.Application.Vouchers.CommandHander
{
    public class GetVoucherTrackHandler : IRequestHandler<GetVoucherTrackCommand, VoucherTrack>

    {
        private readonly ILogger logger;
        private readonly ICurrentUserService currentUserService;
        private readonly IVouchersRepository repository;
        private readonly ILookupRepository lookupRepository;
        private readonly CourierServiceFactory courierServiceFactory;


        public GetVoucherTrackHandler(ICurrentUserService currentUserService, ILogger<VoucherDetailCommandHandler> logger, IVouchersRepository repository, ILookupRepository lookupRepository, CourierServiceFactory courierServiceFactory)
        {
            this.currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
            this.logger = logger;
            this.repository = repository;
            this.lookupRepository = lookupRepository;
            this.courierServiceFactory = courierServiceFactory;

        }


        public async Task<VoucherTrack> Handle(GetVoucherTrackCommand request, CancellationToken cancellationToken)
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
            var courierTrackResource = await courierService.Track(voucherDetails, cancellationToken);
            return new VoucherTrack(voucherDetails, courierTrackResource);
        }

    }
}