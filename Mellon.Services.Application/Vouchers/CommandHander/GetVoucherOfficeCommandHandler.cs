using MediatR;
using Mellon.Common.Services;
using Mellon.Services.Application.Vouchers.Commands;
using Mellon.Services.Common.interfaces;
using Mellon.Services.Common.resources;
using Mellon.Services.External.CourierProviders;
using Mellon.Services.Infrastracture.Repositotiries;
using Microsoft.Extensions.Logging;

namespace Mellon.Services.Application.Vouchers.CommandHander
{
    public class GetVoucherOfficeCommandHandler : IRequestHandler<GetVoucherOfficeCommand, PaginatedListResult<VoucherOfficeItem>>,
                                                IRequestHandler<VouchersOfficePrint, Stream>,
                                                IRequestHandler<GetVouchersOfficeByCompany, ListResult<VoucherOfficePrintResource>>
    {
        private readonly ILogger logger;
        private readonly ICurrentUserService currentUserService;
        private readonly IVouchersRepository repository;
        private readonly CourierServiceFactory courierServiceFactory;
        private readonly ILookupRepository lookupRepository;

        public GetVoucherOfficeCommandHandler(ICurrentUserService currentUserService, ILogger<GetVoucherServiceCommandHandler> logger, IVouchersRepository repository, ILookupRepository lookupRepository, CourierServiceFactory courierServiceFactory)
        {
            this.currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
            this.logger = logger;
            this.repository = repository;
            this.courierServiceFactory = courierServiceFactory;
            this.lookupRepository = lookupRepository;

        }

        public async Task<PaginatedListResult<VoucherOfficeItem>> Handle(GetVoucherOfficeCommand request, CancellationToken cancellationToken)
        {
            return await repository.OfficeVouchers(request.Term, request.Paging, request.Ordering, cancellationToken);
        }

        public async Task<Stream> Handle(VouchersOfficePrint request, CancellationToken cancellationToken)
        {
            if (request.Vouchers == null)
            {
                throw new BadRequestException("You must select at least one entry. Action Cancelled.");
            }
            if (request.Vouchers.Count() == 0)
            {
                throw new BadRequestException("You must select at least one entry. Action Cancelled.");
            }
            var voucherDetails = await this.repository.VoucherDetailsVoucherNo(request.Vouchers.First(), cancellationToken);
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

            return await courierService.Print(request.Vouchers, cancellationToken);
        }

        public async Task<ListResult<VoucherOfficePrintResource>> Handle(GetVouchersOfficeByCompany request, CancellationToken cancellationToken)
        {


            var entities = await repository.OfficeVouchersForPrinting(request.Company, cancellationToken);
            var items = entities.Select(entity => new VoucherOfficePrintResource(entity));
            return new ListResult<VoucherOfficePrintResource>(items);
        }
    }



}
