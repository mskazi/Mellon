using Mellon.Common.Services;
using Mellon.Services.Common.resources;
using Microsoft.EntityFrameworkCore;

namespace Mellon.Services.Infrastracture.Repositotiries
{


    public partial class VouchersRepository : IVouchersRepository
    {
        public async Task<PaginatedListResult<VoucherSearchItem>> Search(string term, ListPaging paging, ListOrder order, CancellationToken cancellationToken)
        {
            context.Database.SetCommandTimeout(180);
            paging ??= new ListPaging();
            int? total = null;

            var ids = new List<int>() { 20, 21, 22, 9920, 9921, 9922 };
            var query = (from data in context.Data
                         join setup in context.ElectraProjectSetups on data.CarrierId equals setup.Id into joinSetup
                         from setupsDefaultIfEmpty in joinSetup.DefaultIfEmpty()

                         join carriers in context.Carriers on data.CarrierId equals carriers.Id into joinCarriers
                         from carriersDefaultIfEmpty in joinCarriers.DefaultIfEmpty()

                         join members in context.Members on data.OrderedBy equals members.Id into joinMembers
                         from membersDefaultIfEmpty in joinMembers.DefaultIfEmpty()

                         join dataLines in context.DataLines on data.Id equals dataLines.DataId into joinDataLines
                         from dataLinesDefaultIfEmpty in joinDataLines.DefaultIfEmpty()

                         join dims1 in context.Dims on new
                         {
                             Id = data.SysStatus,
                             Val = "sys_status"
                         } equals new
                         {
                             Id = dims1.ValueInt,
                             Val = dims1.Name
                         } into jointDims
                         from dimsDefaultIfEmpty in jointDims.DefaultIfEmpty()
                         where (
                             !ids.Contains(data.SysStatus)
                         )
                         select new { data, setupsDefaultIfEmpty, carriersDefaultIfEmpty, dimsDefaultIfEmpty, dataLinesDefaultIfEmpty.Value, membersDefaultIfEmpty }
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
                   s.Value.Contains(term)
                   || s.data.VoucherName.Contains(term)
                   || s.data.VoucherContact.Contains(term)
                   || s.data.CarrierVoucherNo.Contains(term)
                   || s.data.NavisionServiceOrderNo.Contains(term)
                   || s.data.NavisionSalesOrderNo.Contains(term)
                   || s.data.NavisionSellToCustomerNo.Contains(term)
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
                        case "carrierName": prop.Name = "carriersDefaultIfEmpty.DescrShort"; break;
                        case "carrierDeliveryStatus": prop.Name = "dimsDefaultIfEmpty.Description"; break;
                        case "orderedBy": prop.Name = "membersDefaultIfEmpty.MemberName"; break;
                    }
                }
                query = query.OrderBy(order?.Properties);
            }
            var qureyResults = query.Skip(paging.Start)
                .Take(paging.Length).AsQueryable();
            var results = await qureyResults.AsNoTracking().ToListAsync(cancellationToken);
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
                     CarrierName = s.carriersDefaultIfEmpty?.DescrShort,
                     CarrierDeliveryStatus = s.dimsDefaultIfEmpty?.Description,
                     OrderedBy = s.membersDefaultIfEmpty?.MemberName,
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
