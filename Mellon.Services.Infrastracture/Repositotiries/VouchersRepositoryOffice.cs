using Mellon.Common.Services;
using Mellon.Services.Common.resources;
using Mellon.Services.Infrastracture.Models;
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
                          join setup in context.ElectraProjectSetups on data.CarrierId equals setup.Id into joinSetup
                          from setupsDefaultIfEmpty in joinSetup.DefaultIfEmpty()

                          join carriers in context.Carriers on data.CarrierId equals carriers.Id into joinCarriers
                          from carriersDefaultIfEmpty in joinCarriers.DefaultIfEmpty()
                          join members in context.Members on data.OrderedBy equals members.Id into joinMembers
                          from membersDefaultIfEmpty in joinMembers.DefaultIfEmpty()

                          join dimsStatus in context.Dims on data.SysStatus equals dimsStatus.ValueInt into jointDimStatus
                          from jointDimStatusDefaultIfEmpty in jointDimStatus.DefaultIfEmpty()

                          join dimsActionType in context.Dims on data.CarrierActionType equals dimsActionType.ValueInt into jointDimsActionType
                          from jointDimsActionTypeDefaultIfEmpty in jointDimsActionType.DefaultIfEmpty()

                          join dimsDeliveryTime in context.Dims on data.VoucherScheduledDelivery equals dimsDeliveryTime.ValueInt into jointDimsDeliveryTime
                          from jointDimsDeliveryTimeDefaultIfEmpty in jointDimsDeliveryTime.DefaultIfEmpty()
                          where (
                          jointDimsActionTypeDefaultIfEmpty.Name == "carrier_action_type" &&
                          jointDimsDeliveryTimeDefaultIfEmpty.Name == "sys_time_delivery" &&
                          data.SysDepartment != "service" &&
                          data.SysSource != "OpenBI_Acct_Upload" &&
                          data.ElectraProjectId != 72 &&
                          jointDimStatusDefaultIfEmpty.Name == "sys_status" &&
                          data.SysStatus < 9000
                          )
                          select new { data, setupsDefaultIfEmpty, carriersDefaultIfEmpty, membersDefaultIfEmpty, jointDimStatusDefaultIfEmpty, jointDimsActionTypeDefaultIfEmpty, jointDimsDeliveryTimeDefaultIfEmpty })
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
                      (s.membersDefaultIfEmpty.MemberName ?? "").Contains(term) ||
                      (s.jointDimsActionTypeDefaultIfEmpty.Description ?? "").Contains(term) ||
                      (s.jointDimStatusDefaultIfEmpty.Description ?? "").Contains(term) ||
                      s.carriersDefaultIfEmpty.DescrShort.Contains(term) ||
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
                        case "voucherContact": prop.Name = "data.VoucherContact"; break;
                        case "voucherAddress": prop.Name = "data.VoucherAddress"; break;
                        case "voucherPhoneNo": prop.Name = "data.VoucherPhoneNo"; break;
                        case "voucherMobileNo": prop.Name = "data.VoucherMobileNo"; break;
                        case "voucherDescription": prop.Name = "data.VoucherDescription"; break;
                        case "systemCompany": prop.Name = "data.SysCompany"; break;
                        case "carrierVoucherNo": prop.Name = "data.CarrierVoucherNo"; break;
                        case "carrierActionType": prop.Name = "data.CarrierActionType"; break;
                        case "createdBy": prop.Name = "data.CreatedBy"; break;
                        case "createdAt": prop.Name = "data.CreatedAt"; break;
                        case "statusDescription": prop.Name = "jointDimStatusDefaultIfEmpty.Description"; break;
                        case "orderedBy": prop.Name = "membersDefaultIfEmpty.MemberName"; break;
                        case "actionTypeDescription": prop.Name = "jointDimsActionTypeDefaultIfEmpty.Description"; break;
                        case "DeliveryTimeDescription": prop.Name = "jointDimsDeliveryTimeDefaultIfEmpty.Description"; break;
                        case "carrierName": prop.Name = "carriersDefaultIfEmpty.DescrShort"; break;
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
                     StatusDescription = s.jointDimStatusDefaultIfEmpty.Description,
                     OrderedBy = s.membersDefaultIfEmpty == null ? null : s.membersDefaultIfEmpty.MemberName,
                     ActionTypeDescription = s.jointDimsActionTypeDefaultIfEmpty == null ? null : s.jointDimsActionTypeDefaultIfEmpty.Description,
                     DeliveryTimeDescription = s.jointDimsDeliveryTimeDefaultIfEmpty.Description,
                     CarrierName = s.carriersDefaultIfEmpty == null ? null : s.carriersDefaultIfEmpty.DescrShort,
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

        public void AddVoucher(Datum data)
        {
            this.context.Data.Add(data);
        }

        public void Delete(Datum data)
        {
            this.context.Data.Remove(data);
        }
        public async Task<IEnumerable<Datum>> OfficeVouchersForPrinting(string company, CancellationToken cancellationToken)
        {
            return await this.context.Data.Where(p => p.SysDepartment != "service" &&
                    p.SysSource != "OpenBI_Acct_Upload" &&
                    p.ElectraProjectId != 72 &&
                    p.SysCompany == company &&
                    p.SysStatus == 0 &&
                    p.CarrierActionType == 0
            ).OrderBy(p => p.VoucherName).OrderBy(p => p.VoucherContact).ToListAsync();

        }
    }


}
