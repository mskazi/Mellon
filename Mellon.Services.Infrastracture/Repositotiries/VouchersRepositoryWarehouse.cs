﻿using Mellon.Common.Services;
using Mellon.Services.Common.resources;
using Microsoft.EntityFrameworkCore;

namespace Mellon.Services.Infrastracture.Repositotiries
{

    public partial class VouchersRepository : IVouchersRepository
    {
        public async Task<PaginatedListResult<VoucherWarehouseItem>> WarehouseVouchers(string term, ListPaging paging, ListOrder order, CancellationToken cancellationToken)
        {
            paging ??= new ListPaging();
            int? total = null;

            var query = ((from data in context.Data
                          join setup in context.ElectraProjectSetups on data.ElectraProjectId equals setup.Id
                          join carriers in context.Carriers on data.CarrierId equals carriers.Id
                          join members in context.Members on data.OrderedBy equals members.Id
                          join dimsStatus in context.Dims on data.SysStatus equals dimsStatus.ValueInt
                          join dimsActionType in context.Dims on data.CarrierActionType equals dimsActionType.ValueInt
                          join dimsDeliveryTime in context.Dims on data.VoucherScheduledDelivery equals dimsDeliveryTime.ValueInt
                          where (
                          dimsActionType.Name == "carrier_action_type" &&
                          dimsDeliveryTime.Name == "sys_time_delivery" &&
                          (data.ElectraProjectId == 72 || data.ElectraProjectId == 118) &&
                          dimsStatus.Name == "sys_status" &&
                          data.SysStatus < 9000
                          )
                          select new { data, setup, carriers, members, dimsStatus, dimsActionType, dimsDeliveryTime })
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
                      (s.dimsActionType.Description ?? "").Contains(term) ||
                      (s.data.CarrierVoucherNo ?? "").Contains(term) ||
                      (s.dimsStatus.Description ?? "").Contains(term) ||
                      (s.members.MemberName ?? "").Contains(term) ||
                      (s.setup.MellonProject ?? "").Contains(term) ||
                      (s.carriers.DescrShort ?? "").Contains(term)
                    );

            }
            query = query.Distinct();

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

                        case "createdAt": prop.Name = "data.CreatedAt"; break;

                    }
                }

                query = query.OrderBy(order?.Properties);
            }

            var results = await query.Skip(paging.Start)
                .Take(paging.Length)
                .ToListAsync(cancellationToken);

            var dataResults = results.Select(s =>
                 new VoucherWarehouseItem()
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
            return new PaginatedListResult<VoucherWarehouseItem>(
               paging.Start,
               paging.Length,
               total,
               dataResults);
        }
    }
}
