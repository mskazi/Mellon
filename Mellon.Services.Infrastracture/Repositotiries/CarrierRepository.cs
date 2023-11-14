using Mellon.Services.Common.resources;
using Mellon.Services.Infrastracture.Base;
using Mellon.Services.Infrastracture.Models;
using Microsoft.EntityFrameworkCore;

namespace Mellon.Services.Infrastracture.Repositotiries
{
    public interface ICarriersRepository : IRepository
    {
        Task<Carrier> GetCarrierByProject(int electraProjectId, CancellationToken cancellationToken);

        Task<IEnumerable<Carrier>> GetVoucherCarriers(string postalCode, VoucherCreateRoleType VoucherCreateRoleType, CancellationToken cancellationToken);

        Task<Carrier> GetVoucherCarrierByPostalCode(string postalCode, CancellationToken cancellationToken);


        Task<IEnumerable<Carrier>> GetVoucherServiceOrderCarriers(string postalCode, string project, string orderType, CancellationToken cancellationToken);

    }

    public class CarriersRepository : ICarriersRepository
    {
        private readonly MellonContext context;
        public CarriersRepository(MellonContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public IUnitOfWork UnitOfWork => context;


        public async Task<Carrier> GetCarrierByProject(int electraProjectId, CancellationToken cancellationToken)
        {
            return await context.Carriers.Where(p => p.Id == electraProjectId).FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<IEnumerable<Carrier>> GetVoucherCarriers(string postalCode, VoucherCreateRoleType voucherCreateRoleType, CancellationToken cancellationToken)
        {
            if (voucherCreateRoleType == VoucherCreateRoleType.WAREHOUSE)
            {
                var queryWarehouse = ((from carriers in this.context.Carriers
                                       where (
                            carriers.Active == true && carriers.Id == 1)
                                       select new { carriers })).AsQueryable();
                queryWarehouse = queryWarehouse.OrderBy(x => x.carriers.OrFlag);

                return await queryWarehouse.Select(x => x.carriers).ToListAsync(cancellationToken);
            }

            var query = ((from carriers in this.context.Carriers
                          join carriersPostcodeRestrictions in context.CarriersPostcodeRestrictions on carriers.Id equals carriersPostcodeRestrictions.CarrierId
                          where (
               carriers.Active == true &&
               (carriersPostcodeRestrictions.PostCode == "%" || postalCode.Contains(carriersPostcodeRestrictions.PostCode)))
                          select new { carriers })).AsQueryable();
            query = query.OrderBy(x => x.carriers.OrFlag);
            return await query.Select(x => x.carriers).ToListAsync(cancellationToken);
        }

        public async Task<Carrier> GetVoucherCarrierByPostalCode(string postalCode, CancellationToken cancellationToken)
        {
            var query = ((from carriers in this.context.Carriers
                          join carriersPostcodeRestrictions in context.CarriersPostcodeRestrictions on carriers.Id equals carriersPostcodeRestrictions.CarrierId into joinCarriersPostcodeRestrictions
                          from joinCarriersPostCodeRestrictionsDefaultIfEmpty in joinCarriersPostcodeRestrictions.DefaultIfEmpty()

                          join carriersProjectRestrictions in context.CarriersProjectRestrictions on carriers.Id equals carriersProjectRestrictions.CarrierId into joinCarriersProjectRestrictions
                          from joinCarriersProjectRestrictionsDefaultIfEmpty in joinCarriersProjectRestrictions.DefaultIfEmpty()

                          where (
               carriers.Active == true &&
               (joinCarriersPostCodeRestrictionsDefaultIfEmpty.PostCode == "%" || joinCarriersPostCodeRestrictionsDefaultIfEmpty.PostCode.Contains(postalCode)))
                          select carriers)).AsQueryable();


            return await query.FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<IEnumerable<Carrier>> GetVoucherServiceOrderCarriers(string postalCode, string project, string orderType, CancellationToken cancellationToken)
        {
            if (project == null)
            {
                throw new ArgumentNullException(nameof(project));
            }
            var projectDesctription = project == "Other" ? "NBGDir" : project;
            var query = ((from carriers in this.context.Carriers
                          join carriersPostcodeRestrictions in context.CarriersPostcodeRestrictions on carriers.Id equals carriersPostcodeRestrictions.CarrierId
                          join carriersProjectRestrictions in context.CarriersProjectRestrictions on carriers.Id equals carriersProjectRestrictions.CarrierId

                          where (
               carriers.Active == true &&
               carriersProjectRestrictions.Project == projectDesctription &&
               carriersProjectRestrictions.Action == orderType &&
               (carriersPostcodeRestrictions.PostCode == "%" || carriersPostcodeRestrictions.PostCode.Contains(postalCode)))
                          select carriers)).AsQueryable();

            return await query.ToListAsync(cancellationToken);
        }
    }
}
