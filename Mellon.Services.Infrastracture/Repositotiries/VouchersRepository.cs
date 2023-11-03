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

        Task<PaginatedListResult<VoucherOfficeItem>> OfficeVouchers(string term, ListPaging paging, ListOrder order, CancellationToken cancellationToken);
        Task<PaginatedListResult<VoucherWarehouseItem>> WarehouseVouchers(string term, ListPaging paging, ListOrder order, CancellationToken cancellationToken);

        Task<PaginatedListResult<VoucherSearchItem>> Search(string term, ListPaging paging, ListOrder order, CancellationToken cancellationToken);

        Task<VoucherDetails> VoucherDetails(int id, CancellationToken cancellationToken);

        Task<VoucherSummary> Summary(CancellationToken cancellationToken);
    }

    public partial class VouchersRepository : IVouchersRepository
    {
        private readonly MellonContext context;
        private readonly ICurrentUserService currentUserService;

        public VouchersRepository(MellonContext context, ICurrentUserService currentUserService)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.currentUserService = currentUserService;
        }
        public IUnitOfWork UnitOfWork => context;

        public async Task<VoucherSummary> Summary(CancellationToken cancellationToken)
        {
            var c_delivered = await context.Data.Where(p => (p.SysStatus == 90 || p.SysStatus == 9990) && p.CarrierActionType == 0).CountAsync(cancellationToken);
            var p_delivered = await context.Data.Where(p => (p.SysStatus == 90 || p.SysStatus == 9990) && p.CarrierActionType == 1).CountAsync(cancellationToken);
            var c_intransit = await context.Data.Where(p => p.SysStatus == 10 && p.CarrierActionType == 0).CountAsync(cancellationToken);
            var p_intransit = await context.Data.Where(p => p.SysStatus == 10 && p.CarrierActionType == 1).CountAsync(cancellationToken);
            var c_ForPickup = await context.Data.Where(p => p.SysStatus == 0 && p.CarrierActionType == 0).CountAsync(cancellationToken);
            var c_ForVoucher = await context.Data.Where(p => p.SysStatus == 1 && p.CarrierActionType == 0).CountAsync(cancellationToken);
            var p_ForVoucher = await context.Data.Where(p => p.SysStatus == 1 && p.CarrierActionType == 1).CountAsync(cancellationToken);
            var c_CancelledMellon = await context.Data.Where(p => p.SysStatus == 21 || p.SysStatus == 9921).CountAsync(cancellationToken);
            var c_CancelledCarrier = await context.Data.Where(p => p.SysStatus == 22 || p.SysStatus == 9922).CountAsync(cancellationToken);

            return new VoucherSummary()
            {
                CDelivered = c_delivered,
                PDelivered = p_delivered,
                CIntransit = c_intransit,
                PIntransit = p_intransit,
                CForPickup = c_ForPickup,
                CForVoucher = c_ForVoucher,
                PForVoucher = p_ForVoucher,
                CCancelledMellon = c_CancelledMellon,
                CCancelledCarrier = c_CancelledCarrier
            };
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
