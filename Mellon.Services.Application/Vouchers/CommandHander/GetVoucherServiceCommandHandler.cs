using External.ERP.Service;
using MediatR;
using Mellon.Common.Services;
using Mellon.Services.Application.Vouchers.Commands;
using Mellon.Services.Common.interfaces;
using Mellon.Services.Common.resources;
using Mellon.Services.External.CourierProviders;
using Mellon.Services.Infrastracture.Repositotiries;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Mellon.Services.Application.Vouchers.CommandHander
{
    public class GetVoucherServiceCommandHandler : IRequestHandler<GetVoucherServiceCommand, PaginatedListResult<VoucherServiceItem>>,
                                                   IRequestHandler<GetVoucherServiceOrderInfoCommand, VoucherServiceOrderResource>
    {
        private readonly ILogger logger;
        private readonly ICurrentUserService currentUserService;
        private readonly IVouchersRepository repository;
        private readonly ILookupRepository lookupRepository;
        private readonly CourierServiceFactory courierServiceFactory;
        private readonly string erpUrl;

        public GetVoucherServiceCommandHandler(ICurrentUserService currentUserService, ILogger<GetVoucherServiceCommandHandler> logger, IVouchersRepository repository,
            IConfiguration configuration, CourierServiceFactory courierServiceFactory, ILookupRepository lookupRepository)
        {
            this.currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
            this.logger = logger;
            this.repository = repository;
            this.lookupRepository = lookupRepository;
            this.courierServiceFactory = courierServiceFactory;

            if (configuration == null) throw new ArgumentNullException(nameof(configuration));
            erpUrl = configuration["Endpoints:ERP"] ?? throw new Exception("ERP Service endpoint not found in configuration.");
        }

        public async Task<PaginatedListResult<VoucherServiceItem>> Handle(GetVoucherServiceCommand request, CancellationToken cancellationToken)
        {
            return await repository.ServiceVouchers(request.Term, request.Paging, request.Ordering, cancellationToken);
        }

        public async Task<VoucherServiceOrderResource> Handle(GetVoucherServiceOrderInfoCommand request, CancellationToken cancellationToken)
        {
            var endpoint = DataAccessSoapClient.EndpointConfiguration.DataAccessSoap;
            using var client = new DataAccessSoapClient(endpoint, this.erpUrl);
            var responseApproverOrder = await client.Electra_Get_SalesOrderAsync(new Electra_Get_SalesOrderRequest()
            {
                SerialNo = request.Order
            });

            if (responseApproverOrder.Electra_Get_SalesOrderResult == null)
            {
                throw new BadRequestException("No ERP SRVOrder found. Either there does not exist a SRVOrder, OR, the SRVOrder found is failing validations. Check ERP for more details.");
            }




            throw new NotImplementedException();
        }
    }



}
