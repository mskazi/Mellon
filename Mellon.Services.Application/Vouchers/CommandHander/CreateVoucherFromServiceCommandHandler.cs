using External.ERP.Service;
using MediatR;
using Mellon.Services.Application.Vouchers.Commands;
using Mellon.Services.Common.interfaces;
using Mellon.Services.Common.resources;
using Mellon.Services.External.CourierProviders;
using Mellon.Services.Infrastracture.Repositotiries;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Mellon.Services.Application.Vouchers.CommandHander
{
    public class CreateVoucherFromServiceCommandHandler : IRequestHandler<CreateVoucherFromServiceScanCommand, VoucherDetails>,
                                                            IRequestHandler<CreateVoucherServiceOrderCommand, VoucherDetails>
    {
        private readonly ILogger logger;
        private readonly ICurrentUserService currentUserService;
        private readonly IVouchersRepository repository;
        private readonly IContactsRepository contactRepository;
        private readonly ILookupRepository lookupRepository;
        private readonly CourierServiceFactory courierServiceFactory;
        private readonly string erpUrl;

        public CreateVoucherFromServiceCommandHandler(IConfiguration configuration, ICurrentUserService currentUserService, ILogger<GetVoucherServiceCommandHandler> logger, CourierServiceFactory courierServiceFactory, IVouchersRepository repository, ILookupRepository lookupRepository, IContactsRepository contactRepository)
        {
            this.currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
            this.logger = logger;
            this.repository = repository;
            this.contactRepository = contactRepository;
            this.lookupRepository = lookupRepository;
            this.courierServiceFactory = courierServiceFactory;
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));
            erpUrl = configuration["Endpoints:ERP"] ?? throw new Exception("ERP Service endpoint not found in configuration.");
        }


        public async Task<VoucherDetails> Handle(CreateVoucherFromServiceScanCommand request, CancellationToken cancellationToken)
        {
            var endpoint = DataAccessSoapClient.EndpointConfiguration.DataAccessSoap;
            using var client = new DataAccessSoapClient(endpoint, this.erpUrl);
            var responseApproverOrder = await client.Electra_Get_ServiceOrderAsync(new Electra_Get_ServiceOrderRequest()
            {
                ServiceHeaderNo = request.ScanSerial
            });


            try
            {
                using (var dbContextTransaction = repository.UnitOfWork.BeginTransaction())
                {
                    var entity = new Mellon.Services.Infrastracture.Models.Datum()
                    {
                        //VoucherName = contact.VoucherName,
                        //VoucherContact = contact.VoucherContact,
                        //VoucherAddress = contact.VoucherAddress,
                        //VoucherCity = contact.VoucherCity,
                        //VoucherCountry = contact.VoucherCountry,
                        //VoucherPostCode = contact.VoucherPostCode,
                        //VoucherPhoneNo = contact.VoucherPhoneNo,
                        //VoucherMobileNo = contact.VoucherMobileNo,
                        //VoucherDescription = request.Comments,
                        //SysType = request.VoucherType,
                        //SysDepartment = request.VoucherDepartment,
                        //OrderedBy = request.VoucherMember,
                        //SysCompany = request.VoucherCompany,
                        //CarrierId = request.VoucherCarrier,
                        //CarrierActionType = request.VoucherAction ?? 0,
                        //CarrierPackageItems = request.VoucherQuantities,
                        //CarrierPackageWeight = request.VoucherWeight ?? 0,
                        //ConditionCode = request.VoucherCondition,
                        //DeliverSaturday = request.VoucherSaturdayDelivery == 1,
                        //VoucherScheduledDelivery = request.VoucherSaturdayDelivery == 0 ? 0 : request.VoucherDeliveryTime,
                        //CodAmount = request.VoucherCondition == "COD" ? request.VoucherCodAmount : new decimal(0.00),
                        //ElectraProjectId = project.Id,
                        //SysSource = "OpenBI",
                        //SysCheck = false,
                        //SysStatus = 0,
                        //SysCountry = this.currentUserService.CurrentUser.Country,
                        //CreatedBy = this.currentUserService.CurrentUser.Email,
                        //CreatedAt = DateTime.Now,
                        //UpdatedAt = DateTime.Now,
                        //UpdatedBy = this.currentUserService.CurrentUser.Email,
                    };
                    repository.AddVoucher(entity);
                    await repository.UnitOfWork.SaveChangesAsync();

                    ICourierService courierService = null;// this.courierServiceFactory.GetCourierService((CourierMode)project.CarrierId, project);
                    if (courierService == null)
                    {
                        throw new ArgumentNullException(nameof(courierService));
                    }
                    var courierCreateResource = await courierService.Create(entity, cancellationToken);

                    entity.CarrierJobid = courierCreateResource.JobID.ToString();
                    entity.CarrierVoucherNo = courierCreateResource.VoucherNo;
                    entity.UpdatedAt = DateTime.Now;
                    entity.UpdatedBy = this.currentUserService.CurrentUser.Email;
                    await repository.UnitOfWork.SaveChangesAsync();
                    repository.UnitOfWork.Commit();
                    return await this.repository.VoucherDetails(entity.Id, cancellationToken);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Task<VoucherDetails> Handle(CreateVoucherServiceOrderCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }



}
