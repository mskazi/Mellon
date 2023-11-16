using Mellon.Common.Services;
using Mellon.Services.Common.resources;
using Microsoft.EntityFrameworkCore;

namespace Mellon.Services.Infrastracture.Repositotiries
{


    public partial class VouchersRepository : IVouchersRepository
    {
        public async Task<PaginatedListResult<VoucherServiceItem>> ServiceVouchers(string term, ListPaging paging, ListOrder order, CancellationToken cancellationToken)
        {
            paging ??= new ListPaging();
            int? total = null;

            var query = ((from data in context.Data
                          join setup in context.ElectraProjectSetups on data.ElectraProjectId equals setup.Id into joinSetup
                          from setupsDefaultIfEmpty in joinSetup.DefaultIfEmpty()

                          join carriers in context.Carriers on data.CarrierId equals carriers.Id into joinCarriers
                          from carriersDefaultIfEmpty in joinCarriers.DefaultIfEmpty()

                          join members in context.Members on data.OrderedBy equals members.Id into joinMembers
                          from membersDefaultIfEmpty in joinMembers.DefaultIfEmpty()

                          join dataLines in context.DataLines on data.Id equals dataLines.DataId into joinDataLines
                          from dataLinesDefaultIfEmpty in joinDataLines.DefaultIfEmpty()

                          join dimsStatus in context.Dims on data.SysStatus equals dimsStatus.ValueInt into jointDimStatus
                          from jointDimStatusDefaultIfEmpty in jointDimStatus.DefaultIfEmpty()

                          join dimsActionType in context.Dims on data.CarrierActionType equals dimsActionType.ValueInt into jointDimsActionType
                          from jointDimsActionTypeDefaultIfEmpty in jointDimsActionType.DefaultIfEmpty()

                          join dimsDeliveryTime in context.Dims on data.VoucherScheduledDelivery equals dimsDeliveryTime.ValueInt into jointDimsDeliveryTime
                          from jointDimsDeliveryTimeDefaultIfEmpty in jointDimsDeliveryTime.DefaultIfEmpty()

                          where (
                          data.SysDepartment == "service" &&
                          data.SysStatus < 9000 &&
                          jointDimsActionTypeDefaultIfEmpty.Name == "carrier_action_type" &&
                          jointDimsDeliveryTimeDefaultIfEmpty.Name == "sys_time_delivery" &&
                          dataLinesDefaultIfEmpty.Name == "serial_no"
                          )
                          select new { data, setupsDefaultIfEmpty, carriersDefaultIfEmpty, membersDefaultIfEmpty, dataLinesDefaultIfEmpty, jointDimStatusDefaultIfEmpty, jointDimsActionTypeDefaultIfEmpty, jointDimsDeliveryTimeDefaultIfEmpty }).Distinct()
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
                      (s.data.SysCompany ?? "").Contains(term) ||
                      (s.data.NavisionServiceOrderNo ?? "").Contains(term) ||
                      (s.data.NavisionSalesOrderNo ?? "").Contains(term) ||
                      (s.jointDimsActionTypeDefaultIfEmpty.Description ?? "").Contains(term) ||
                      (s.data.CarrierVoucherNo ?? "").Contains(term) ||
                      (s.jointDimStatusDefaultIfEmpty.Description ?? "").Contains(term) ||
                      (s.membersDefaultIfEmpty.MemberName ?? "").Contains(term) ||
                      (s.setupsDefaultIfEmpty.MellonProject ?? "").Contains(term) ||
                      (s.carriersDefaultIfEmpty.DescrShort ?? "").Contains(term) ||
                      (s.dataLinesDefaultIfEmpty.Value ?? "").Contains(term)

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
                        case "mellonProject": prop.Name = "setupsDefaultIfEmpty.MellonProject"; break;
                        case "actionTypeDescription": prop.Name = "jointDimsActionTypeDefaultIfEmpty.Description"; break;
                        case "voucherName": prop.Name = "data.VoucherName"; break;
                        case "voucherAddress": prop.Name = "data.VoucherAddress"; break;
                        case "voucherPhoneNo": prop.Name = "data.VoucherPhoneNo"; break;
                        case "voucherMobileNo": prop.Name = "data.VoucherMobileNo"; break;
                        case "voucherDescription": prop.Name = "data.VoucherDescription"; break;
                        case "serialNo": prop.Name = "dataLinesDefaultIfEmpty.Value"; break;
                        case "carrierName": prop.Name = "carriersDefaultIfEmpty.DescrShort"; break;
                        case "statusDescription": prop.Name = "jointDimStatusDefaultIfEmpty.Description"; break;
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
                     SerialNo = s.dataLinesDefaultIfEmpty.Value,
                     SystemCompany = s.data.SysCompany,
                     NavisionServiceOrderNo = s.data.NavisionServiceOrderNo,
                     NavisionSalesOrderNo = s.data.NavisionSalesOrderNo,
                     ActionTypeDescription = s.jointDimsActionTypeDefaultIfEmpty.Description,
                     CarrierVoucherNo = s.data.CarrierVoucherNo,
                     StatusDescription = s.jointDimStatusDefaultIfEmpty.Description,
                     OrderedBy = s.membersDefaultIfEmpty.MemberName,
                     MellonProject = s.setupsDefaultIfEmpty.MellonProject,
                     CarrierName = s.carriersDefaultIfEmpty.DescrShort,
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
    }
}
