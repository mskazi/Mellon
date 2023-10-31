using Mellon.Common.Services;
using Mellon.Services.Common.interfaces;
using Mellon.Services.Common.resources;
using Mellon.Services.Infrastracture.Base;
using Mellon.Services.Infrastracture.Models;
using Microsoft.EntityFrameworkCore;

namespace Mellon.Services.Infrastracture.Repositotiries
{
    public interface IVouchersRepository : IRepository
    {
        Task<PaginatedListResult<VoucherServiceItem>> ServiceVouchers(string term, ListPaging paging, ListOrder order, CancellationToken cancellationToken);
        Task<VoucherDetails> VoucherDetails(int id, CancellationToken cancellationToken);
    }

    public class VouchersRepository : IVouchersRepository
    {
        private readonly MellonContext context;
        private readonly ICurrentUserService currentUserService;

        public VouchersRepository(MellonContext context, ICurrentUserService currentUserService)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.currentUserService = currentUserService;
        }
        public IUnitOfWork UnitOfWork => context;

        public async Task<PaginatedListResult<VoucherServiceItem>> ServiceVouchers(string term, ListPaging paging, ListOrder order, CancellationToken cancellationToken)
        {
            paging ??= new ListPaging();
            int? total = null;

            var query = (from data in context.Data
                         join setup in context.ElectraProjectSetups on data.ElectraProjectId equals setup.Id
                         join carriers in context.Carriers on data.CarrierId equals carriers.Id
                         join members in context.Members on data.OrderedBy equals members.Id
                         join dataLines in context.DataLines on data.Id equals dataLines.DataId
                         join dimsStatus in context.Dims on data.SysStatus equals dimsStatus.ValueInt
                         join dimsActionType in context.Dims on data.CarrierActionType equals dimsActionType.ValueInt
                         join dimsDeliveryTime in context.Dims on data.VoucherScheduledDelivery equals dimsDeliveryTime.ValueInt
                         where (
                         dimsActionType.Name == "carrier_action_type" &&
                         dimsDeliveryTime.Name == "sys_time_delivery" &&
                         data.SysDepartment == "service" &&
                         dataLines.Name == "serial_no" &&
                         data.SysCountry == this.currentUserService.CurrentUser.Country &&
                         data.SysStatus < 9000
                         )
                         select new { data, setup, carriers, members, dataLines, dimsStatus, dimsActionType, dimsDeliveryTime }
            ).AsQueryable();

            if (!string.IsNullOrWhiteSpace(term))
            {
                query = query.Where(s =>
                     s.data.VoucherName.Contains(term) ||
                     (s.data.VoucherContact ?? "").Contains(term) ||
                     (s.data.VoucherAddress ?? "").Contains(term) ||
                      (s.data.VoucherCity ?? "").Contains(term) ||
                      (s.data.VoucherPostCode ?? "").Contains(term) ||
                      (s.data.VoucherMobileNo ?? "").Contains(term) ||
                      (s.data.VoucherDescription ?? "").Contains(term) ||
                      (s.dataLines.Value ?? "").Contains(term) ||
                      (s.data.SysCompany ?? "").Contains(term) ||
                      (s.data.NavisionServiceOrderNo ?? "").Contains(term) ||
                      (s.data.NavisionSalesOrderNo ?? "").Contains(term) ||
                      (s.dimsActionType.Description ?? "").Contains(term) ||
                      (s.data.CarrierVoucherNo ?? "").Contains(term) ||
                      (s.dimsStatus.Description ?? "").Contains(term) ||
                      (s.members.MemberName ?? "").Contains(term) ||
                      (s.setup.MellonProject ?? "").Contains(term) ||
                      (s.carriers.DescrShort ?? "").Contains(term)
                    );
            }


            //get total
            total = await query.CountAsync(cancellationToken);

            //apply ordering
            if (order?.Properties?.Any() ?? false)
            {
                var listOrder = new ListOrder();
                foreach (var prop in order.Properties)
                {
                    switch (prop.Name)
                    {
                        case "id": prop.Name = "data.Id"; break;
                        case "createdAt": prop.Name = "data.CreatedAt"; break;
                        case "mellonProject": prop.Name = "setup.MellonProject"; break;
                        case "actionTypeDescription": prop.Name = "dimsActionType.Description"; break;
                        case "voucherName": prop.Name = "data.VoucherName"; break;
                        case "voucherAddress": prop.Name = "data.VoucherAddress"; break;
                        case "voucherPhoneNo": prop.Name = "data.VoucherPhoneNo"; break;
                        case "voucherMobileNo": prop.Name = "data.VoucherMobileNo"; break;
                        case "voucherDescription": prop.Name = "data.VoucherDescription"; break;
                        case "serialNo": prop.Name = "dataLines.Value"; break;
                        case "carrierName": prop.Name = "carriers.DescrShort"; break;
                        case "statusDescription": prop.Name = "dimsStatus.Description"; break;
                        case "carrierVoucherNo": prop.Name = "data.CarrierVoucherNo"; break;
                    }
                }

                query = query.OrderBy(order?.Properties);
            }

            var results = await query.Skip(paging.Start)
                .Take(paging.Length)
                .ToListAsync(cancellationToken);

            var dataResults = results.Select(s =>
                 new VoucherServiceItem()
                 {
                     Id = s.data.Id,
                     SystemStatus = s.data.SysStatus,
                     VoucherName = s.data.VoucherName,
                     VoucherContact = s.data.VoucherContact,
                     VoucherAddress = s.data.VoucherAddress,
                     VoucherCity = s.data.VoucherCity,
                     VoucherPostCode = s.data.VoucherPostCode,
                     VoucherPhoneNo = s.data.VoucherPhoneNo,
                     VoucherMobileNo = s.data.VoucherMobileNo,
                     VoucherDescription = s.data.VoucherDescription,
                     SerialNo = s.dataLines == null ? null : s.dataLines.Value,
                     SystemCompany = s.data.SysCompany,
                     NavisionServiceOrderNo = s.data.NavisionServiceOrderNo,
                     NavisionSalesOrderNo = s.data.NavisionSalesOrderNo,
                     ActionTypeDescription = s.dimsActionType == null ? null : s.dimsActionType.Description,
                     CarrierVoucherNo = s.data.CarrierVoucherNo,
                     StatusDescription = s.dimsStatus.Description,
                     OrderedBy = s.members == null ? null : s.members.MemberName,
                     MellonProject = s.setup == null ? null : s.setup.MellonProject,
                     CarrierName = s.carriers == null ? null : s.carriers.DescrShort,
                     CreatedBy = s.data.CreatedBy,
                     CreatedAt = s.data.CreatedAt,
                 }
            ).ToList();


            //return result
            return new PaginatedListResult<VoucherServiceItem>(
               paging.Start,
               paging.Length,
               total,
               dataResults);
        }

        public async Task<VoucherDetails> VoucherDetails(int id, CancellationToken cancellationToken)
        {

            var query = (from data in context.Data
                         join setup in context.ElectraProjectSetups on data.ElectraProjectId equals setup.Id
                         join carriers in context.Carriers on data.CarrierId equals carriers.Id
                         join members in context.Members on data.OrderedBy equals members.Id
                         join dimsConditios in context.Dims on data.ConditionCode equals dimsConditios.ValueChar
                         join dimsDepartment in context.Dims on data.SysDepartment equals dimsDepartment.ValueChar
                         join dimsDelivery in context.Dims on data.VoucherScheduledDelivery equals dimsDelivery.ValueInt
                         join dimsDs in context.Dims on data.SysStatus equals dimsDs.ValueInt
                         where (
                         //dimsConditios.Name == "sys_condition" &&
                         //data.SysDepartment == "sys_department" &&
                         //dimsDelivery.Name == "sys_time_delivery" &&
                         //dimsDs.Name == "sys_status" &&
                         data.Id == id
                         )
                         select new { data, setup, carriers, members, dimsConditios, dimsDepartment, dimsDelivery, dimsDs }
            ).AsQueryable();


            var result = query.Select(s =>
                 new VoucherDetails()
                 {
                     Id = s.data.Id,
                     SystemStatus = s.data.SysStatus,
                     VoucherName = s.data.VoucherName,
                     VoucherContact = s.data.VoucherContact,
                     VoucherAddress = s.data.VoucherAddress,
                     VoucherCity = s.data.VoucherCity,
                     VoucherPostCode = s.data.VoucherPostCode,
                     VoucherPhoneNo = s.data.VoucherPhoneNo,
                     VoucherMobileNo = s.data.VoucherMobileNo,
                     VoucherDescription = s.data.VoucherDescription,
                     NavisionServiceOrderNo = s.data.NavisionServiceOrderNo,
                     NavisionSalesOrderNo = s.data.NavisionSalesOrderNo,
                     CarrierDelivereyStatus = s.dimsDs == null ? null : s.dimsDs.Description,
                     CarrierVoucherNo = s.data.CarrierVoucherNo,
                     CarrierPickupDate = s.data.CarrierPickupDate,
                     CarrierDeliveryDate = s.data.CarrierDeliveryDate,
                     CarrierDeliveredTo = s.data.CarrierDeliveredTo,
                     ConditionCode = s.dimsConditios == null ? null : s.dimsConditios.Description,
                     DeliverSaturday = s.data.DeliverSaturday,
                     DeliveryDescription = s.dimsDelivery == null ? null : s.dimsDelivery.Description,
                     CODAmount = s.data.CodAmount,
                     CreatedBy = s.data.CreatedBy,
                     CreatedAt = s.data.CreatedAt,
                     SystemCompany = s.data.SysCompany,
                     SystemDepertment = s.dimsDepartment == null ? null : s.dimsDepartment.Description,
                     OrderedBy = s.members == null ? null : s.members.MemberName,
                     CarrierCode = s.setup == null ? null : s.setup.CarrierCode,
                     MellonProject = s.setup == null ? null : s.setup.MellonProject,
                     CarrierName = s.carriers == null ? null : s.carriers.DescrShort,
                 }
            );
            var voucherDetails = await result.FirstOrDefaultAsync();
            if (voucherDetails != null)
            {
                voucherDetails.DataInabilities = new List<VoucherDataInability>();
                voucherDetails.DataInabilities = context.DataInabilities.Where(x => x.DataId == id).Select(x => new VoucherDataInability()
                {
                    Id = x.Id,
                    DataId = x.DataId,
                    Reason = x.Reason,
                    TrnDate = x.TrnDate
                }).ToList();
                voucherDetails.DataInabilities = new List<VoucherDataInability>();
                voucherDetails.DataLines = context.DataLines.Where(x => x.DataId == id).Select(x => new VoucherDataLine()
                {
                    Id = x.Id,
                    DataId = x.DataId,
                    CreatedAt = x.CreatedAt,
                    CreatedBy = x.CreatedBy,
                    Name = x.Name,
                    UpdatedAt = x.UpdatedAt,
                    UpdatedBy = x.UpdatedBy,
                    Value = x.Value
                }).ToList();
            }
            return voucherDetails;

        }
    }
}
