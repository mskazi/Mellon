using Mellon.Common.Services;
using Mellon.Services.Common.resources;
using Microsoft.EntityFrameworkCore;

namespace Mellon.Services.Infrastracture.Repositotiries
{


    public partial class VouchersRepository : IVouchersRepository
    {
        public async Task<PaginatedListResult<VoucherOfficeItem>> OfficeVouchers(string term, ListPaging paging, ListOrder order, CancellationToken cancellationToken)
        {
            paging ??= new ListPaging();
            int? total = null;

            var query = ((from data in context.Data
                          join setup in context.ElectraProjectSetups on data.CarrierId equals setup.Id
                          join carriers in context.Carriers on data.CarrierId equals carriers.Id
                          join members in context.Members on data.OrderedBy equals members.Id
                          join dimsStatus in context.Dims on data.SysStatus equals dimsStatus.ValueInt
                          join dimsActionType in context.Dims on data.CarrierActionType equals dimsActionType.ValueInt
                          join dimsDeliveryTime in context.Dims on data.VoucherScheduledDelivery equals dimsDeliveryTime.ValueInt
                          where (
                          dimsActionType.Name == "carrier_action_type" &&
                          dimsDeliveryTime.Name == "sys_time_delivery" &&
                          data.SysDepartment != "service" &&
                          data.SysSource != "OpenBI_Acct_Upload" &&
                          data.ElectraProjectId != 72 &&
                          dimsStatus.Name == "sys_status" &&
                          data.SysStatus < 9000
                          )
                          select new { data, setup, carriers, members, dimsStatus, dimsActionType, dimsDeliveryTime }).Distinct()
            ).AsQueryable();

            if (!string.IsNullOrWhiteSpace(term))
            {
                query = query.Where(s =>
                     s.data.VoucherName.Contains(term) ||
                     (s.data.VoucherContact ?? "").Contains(term) ||
                     (s.data.VoucherAddress ?? "").Contains(term) ||
                      (s.data.VoucherCity ?? "").Contains(term) ||
                      (s.data.VoucherPostCode ?? "").Contains(term) ||
                      (s.data.VoucherPhoneNo ?? "").Contains(term) ||
                      (s.data.VoucherMobileNo ?? "").Contains(term) ||
                      (s.data.VoucherDescription ?? "").Contains(term) ||
                      (s.data.SysCompany ?? "").Contains(term) ||
                      (s.data.CarrierVoucherNo ?? "").Contains(term) ||
                      (s.members.MemberName ?? "").Contains(term) ||
                      (s.dimsActionType.Description ?? "").Contains(term) ||
                      (s.dimsStatus.Description ?? "").Contains(term) ||
                      (s.carriers.DescrShort ?? "").Contains(term) ||
                      (s.data.CreatedBy ?? "").Contains(term)
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
                        case "systemStatus": prop.Name = "data.SysStatus"; break;
                        case "voucherName": prop.Name = "data.VoucherName"; break;
                        case "voucherAddress": prop.Name = "data.VoucherAddress"; break;
                        case "voucherPhoneNo": prop.Name = "data.VoucherPhoneNo"; break;
                        case "voucherMobileNo": prop.Name = "data.VoucherMobileNo"; break;
                        case "voucherDescription": prop.Name = "data.VoucherDescription"; break;
                        case "systemCompany": prop.Name = "data.SysCompany"; break;
                        case "carrierVoucherNo": prop.Name = "data.CarrierVoucherNo"; break;
                        case "carrierActionType": prop.Name = "data.CarrierActionType"; break;
                        case "statusDescription": prop.Name = "dimsStatus.Description"; break;
                        case "orderedBy": prop.Name = "members.MemberName"; break;
                        case "actionTypeDescription": prop.Name = "dimsActionType.Description"; break;
                        case "DeliveryTimeDescription": prop.Name = "dimsDeliveryTime.Description"; break;
                        case "createdBy": prop.Name = "data.CreatedBy"; break;
                        case "carrierName": prop.Name = "carriers.DescrShort"; break;
                        case "createdAt": prop.Name = "data.CreatedAt"; break;
                    }
                }
                query = query.OrderBy(order?.Properties);
            }

            var results = await query.Skip(paging.Start)
                .Take(paging.Length)
                .ToListAsync(cancellationToken);

            var dataResults = results.Select(s =>
                 new VoucherOfficeItem()
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
                     SystemCompany = s.data.SysCompany,
                     CarrierVoucherNo = s.data.CarrierVoucherNo,
                     CarrierActionType = s.data.CarrierActionType,
                     StatusDescription = s.dimsStatus.Description,
                     OrderedBy = s.members == null ? null : s.members.MemberName,
                     ActionTypeDescription = s.dimsActionType == null ? null : s.dimsActionType.Description,
                     DeliveryTimeDescription = s.dimsDeliveryTime.Description,
                     CarrierName = s.carriers == null ? null : s.carriers.DescrShort,
                     CreatedBy = s.data.CreatedBy,
                     CreatedAt = s.data.CreatedAt,
                 }
            ).ToList();


            //return result
            return new PaginatedListResult<VoucherOfficeItem>(
               paging.Start,
               paging.Length,
               total,
               dataResults);
        }
    }
}
