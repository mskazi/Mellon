using Mellon.Common.Services;
using Mellon.Services.Common.resources;
using Microsoft.EntityFrameworkCore;

namespace Mellon.Services.Infrastracture.Repositotiries
{


    public partial class VouchersRepository : IVouchersRepository
    {
        public async Task<PaginatedListResult<VoucherSearchItem>> Search(string term, ListPaging paging, ListOrder order, CancellationToken cancellationToken)
        {
            paging ??= new ListPaging();
            int? total = null;

            //((from data in context.Data
            //  join setup in context.ElectraProjectSetups on data.CarrierId equals setup.Id
            //  join carriers in context.Carriers on data.CarrierId equals carriers.Id
            //  join members in context.Members on data.OrderedBy equals members.Id
            //  join dims1 in context.Dims on data.ConditionCode equals dims1.ValueChar
            //  join dims2 in context.Dims on data.SysDepartment equals dims2.ValueChar
            //  join dimsDeliveryTime in context.Dims on data.VoucherScheduledDelivery equals dimsDeliveryTime.ValueInt
            //  join dimsDS in context.Dims on data.SysStatus equals dimsDS.ValueInt
            //  where (
            //  dimsDeliveryTime.Name == "sys_time_delivery" &&
            //  data.SysDepartment != "service" &&
            //  data.SysSource != "OpenBI_Acct_Upload" &&
            //  data.ElectraProjectId != 72 &&
            //  data.SysStatus < 9000
            //  )
            // select new { data, setup, carriers, dims1, dims2, dimsDS, members, dimsDeliveryTime }).Distinct()
            var query = ((from data in context.Data
                          join setup in context.ElectraProjectSetups on data.CarrierId equals setup.Id
                          join carriers in context.Carriers on data.CarrierId equals carriers.Id
                          join members in context.Members on data.OrderedBy equals members.Id
                          join dataLines in context.DataLines on data.Id equals dataLines.DataId
                          join dims1 in context.Dims on data.SysStatus equals dims1.ValueInt
                          where (
                        dims1.Name == "sys_status" && (
                        data.SysStatus == 20 || data.SysStatus == 21 || data.SysStatus == 22 || data.SysStatus == 9920 || data.SysStatus == 9921 || data.SysStatus == 9922
                        )
                          )
                          select new { data, setup, carriers, dims1, dataLines, members }).Distinct()
            ).AsQueryable();
            DateTime date;
            if (DateTime.TryParse(term, out date))
            {
                query = query.Where(s =>
                    s.data.CreatedAt >= date && s.data.CreatedAt <= date.AddDays(1)
                   );
            }
            else if (!string.IsNullOrWhiteSpace(term))
            {
                query = query.Where(s =>
                    s.data.VoucherName.Contains(term) ||
                    (s.data.VoucherContact ?? "").Contains(term) ||
                    (s.data.CarrierVoucherNo ?? "").Contains(term) ||
                    (s.data.NavisionServiceOrderNo ?? "").Contains(term) ||
                    (s.data.NavisionSalesOrderNo ?? "").Contains(term) ||
                    (s.data.NavisionSellToCustomerNo ?? "").Contains(term) ||
                    (s.dataLines.Value ?? "").Contains(term)
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
                        case "voucherName": prop.Name = "data.VoucherName"; break;
                        case "voucherContact": prop.Name = "data.VoucherContact"; break;
                        case "voucherAddress": prop.Name = "data.VoucherAddress"; break;
                        case "voucherPhoneNo": prop.Name = "data.VoucherPhoneNo"; break;
                        case "voucherMobileNo": prop.Name = "data.VoucherMobileNo"; break;
                        case "voucherDescription": prop.Name = "data.VoucherDescription"; break;
                        case "createdAt": prop.Name = "data.CreatedAt"; break;
                        case "systemCompany": prop.Name = "data.SysCompany"; break;
                        case "carrierVoucherNo": prop.Name = "data.CarrierVoucherNo"; break;
                        case "carrierName": prop.Name = "carriers.DescrShort"; break;
                        case "carrierDeliveryStatus": prop.Name = "dims1.Description"; break;
                        case "orderedBy": prop.Name = "members.MemberName"; break;
                    }
                }
                query = query.OrderBy(order?.Properties);
            }

            var results = await query.Skip(paging.Start)
                .Take(paging.Length)
                .ToListAsync(cancellationToken);

            var dataResults = results.Select(s =>
                 new VoucherSearchItem()
                 {
                     Id = s.data.Id,
                     VoucherName = s.data.VoucherName,
                     VoucherContact = s.data.VoucherContact,
                     VoucherAddress = s.data.VoucherAddress,
                     VoucherCity = s.data.VoucherCity,
                     VoucherPostCode = s.data.VoucherPostCode,
                     VoucherPhoneNo = s.data.VoucherPhoneNo,
                     VoucherMobileNo = s.data.VoucherMobileNo,
                     VoucherDescription = s.data.VoucherDescription,
                     CreatedAt = s.data.CreatedAt,
                     SystemCompany = s.data.SysCompany,
                     CarrierVoucherNo = s.data.CarrierVoucherNo,
                     CarrierName = s.carriers == null ? null : s.carriers.DescrShort,
                     CarrierDeliveryStatus = s.dims1.Description,
                     OrderedBy = s.members == null ? null : s.members.MemberName,
                 }
            ).ToList();


            //return result
            return new PaginatedListResult<VoucherSearchItem>(
               paging.Start,
               paging.Length,
               total,
               dataResults);
        }
    }
}
