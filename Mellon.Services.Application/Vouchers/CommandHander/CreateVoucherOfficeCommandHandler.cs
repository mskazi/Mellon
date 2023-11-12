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
    public class CreateVoucherOfficeCommandHandler : IRequestHandler<CreateVoucherOfficeCommand, VoucherDetails>
    {
        private readonly ILogger logger;
        private readonly ICurrentUserService currentUserService;
        private readonly IVouchersRepository repository;
        private readonly IContactsRepository contactRepository;
        private readonly ILookupRepository lookupRepository;
        private readonly CourierServiceFactory courierServiceFactory;



        public CreateVoucherOfficeCommandHandler(ICurrentUserService currentUserService, ILogger<GetVoucherServiceCommandHandler> logger, CourierServiceFactory courierServiceFactory, IVouchersRepository repository, ILookupRepository lookupRepository, IContactsRepository contactRepository)
        {
            this.currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
            this.logger = logger;
            this.repository = repository;
            this.contactRepository = contactRepository;
            this.lookupRepository = lookupRepository;
            this.courierServiceFactory = courierServiceFactory;
        }


        public async Task<VoucherDetails> Handle(CreateVoucherOfficeCommand request, CancellationToken cancellationToken)
        {
            var contact = await this.contactRepository.GetContact(request.ContactId, cancellationToken);
            if (contact == null)
            {
                throw new BadRequestException(String.Format("Cannot seem able to find a valid contact ID: {0}.Please try with another one ", request.ContactId));
            }

            var project = await this.lookupRepository.GetActiveElectraProjectSetup(request.VoucherCarrier, request.VoucherCompany, request.VoucherDepartment, cancellationToken);
            if (project == null)
            {
                throw new BadRequestException(String.Format("The Carrier({0}) Company({1}) Department({2}) provided does not correspond to a valid Courier Setup. ", request.VoucherCarrier, request.VoucherCompany, request.VoucherDepartment));

            }
            try
            {
                using (var dbContextTransaction = repository.UnitOfWork.BeginTransaction())
                {
                    var entity = new Mellon.Services.Infrastracture.Models.Datum()
                    {
                        VoucherName = contact.VoucherName,
                        VoucherContact = contact.VoucherContact,
                        VoucherAddress = contact.VoucherAddress,
                        VoucherCity = contact.VoucherCity,
                        VoucherCountry = contact.VoucherCountry,
                        VoucherPostCode = contact.VoucherPostCode,
                        VoucherPhoneNo = contact.VoucherPhoneNo,
                        VoucherMobileNo = contact.VoucherMobileNo,
                        VoucherDescription = request.Comments,
                        SysType = request.VoucherType,
                        SysDepartment = request.VoucherDepartment,
                        OrderedBy = request.VoucherMember,
                        SysCompany = request.VoucherCompany,
                        CarrierId = request.VoucherCarrier,
                        CarrierActionType = request.VoucherAction ?? 0,
                        CarrierPackageItems = request.VoucherQuantities,
                        CarrierPackageWeight = request.VoucherWeight ?? 0,
                        ConditionCode = request.VoucherCondition,
                        DeliverSaturday = request.VoucherSaturdayDelivery == 1,
                        VoucherScheduledDelivery = request.VoucherSaturdayDelivery == 0 ? 0 : request.VoucherDeliveryTime,
                        CodAmount = request.VoucherCondition == "COD" ? request.VoucherCodAmount : new decimal(0.00),
                        ElectraProjectId = project.Id,
                        SysSource = "OpenBI",
                        SysCheck = false,
                        SysStatus = 0,
                        SysCountry = this.currentUserService.CurrentUser.Country,
                        CreatedBy = this.currentUserService.CurrentUser.Email,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                        UpdatedBy = this.currentUserService.CurrentUser.Email,
                    };
                    repository.AddVoucher(entity);
                    await repository.UnitOfWork.SaveChangesAsync();

                    ICourierService courierService = this.courierServiceFactory.GetCourierService((CourierMode)project.CarrierId, project);
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
    }



}
