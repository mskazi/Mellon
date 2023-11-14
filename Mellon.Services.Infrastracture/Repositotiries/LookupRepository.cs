using Mellon.Services.Common.resources;
using Mellon.Services.Infrastracture.Base;
using Mellon.Services.Infrastracture.Models;
using Microsoft.EntityFrameworkCore;

namespace Mellon.Services.Infrastracture.Repositotiries
{
    public interface ILookupRepository : IRepository
    {
        Task<IEnumerable<Dim>> GetDepartments(CancellationToken cancellationToken);

        Task<IEnumerable<Dim>> GetCompanies(VoucherCreateRoleType voucherCreateRoleType, CancellationToken cancellationToken);

        Task<IEnumerable<Country>> GetCountries(CancellationToken cancellationToken);

        Task<ElectraProjectSetup> GetElectraProjectSetup(int carriedId, int projectId, CancellationToken cancellationToken);

        Task<IEnumerable<Dim>> GetVoucherTypes(CancellationToken cancellationToken);
        Task<IEnumerable<Dim>> GetVoucherDepartments(VoucherCreateRoleType voucherCreateRoleType, CancellationToken cancellationToken);

        Task<IEnumerable<Dim>> GetVoucherConditions(CancellationToken cancellationToken);

        Task<IEnumerable<Dim>> GetVoucherDeliveryTimes(CancellationToken cancellationToken);

        Task<ElectraProjectSetup> GetActiveElectraProjectSetup(int carriedId, string company, string deparment, CancellationToken cancellationToken);

        Task<ElectraProjectSetup> GetActiveElectraProjectSetupServiceOrder(string project, CancellationToken cancellationToken);

        Task<IEnumerable<string>> GetElectraProjectOffices(CancellationToken cancellationToken);
    }

    public class LookupRepository : ILookupRepository
    {
        private readonly MellonContext context;
        public LookupRepository(MellonContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public IUnitOfWork UnitOfWork => context;

        public async Task<IEnumerable<Dim>> GetCompanies(VoucherCreateRoleType voucherCreateRoleType, CancellationToken cancellationToken)
        {
            if (voucherCreateRoleType == VoucherCreateRoleType.SERVICE)
            {
                return await this.context.Dims
                .Where(p => p.Name == "sys_company" && p.ValueChar == "MT")
                .OrderBy(o => o.ValueChar).Take(1).ToListAsync(cancellationToken);
            }


            if (voucherCreateRoleType == VoucherCreateRoleType.WAREHOUSE)
            {
                return await this.context.Dims
                 .Where(p => p.Name == "sys_company" && p.ValueChar == "MT")
                 .OrderBy(o => o.ValueChar).ToListAsync(cancellationToken);
            }
            return await this.context.Dims
                 .Where(p => p.Name == "sys_company")
                 .OrderBy(o => o.ValueChar).ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Country>> GetCountries(CancellationToken cancellationToken)
        {
            return await this.context.Countries
                  .OrderBy(o => o.Country1).ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Dim>> GetDepartments(CancellationToken cancellationToken)
        {
            return await this.context.Dims
                .Where(p => p.Name == "sys_department")
                .OrderBy(o => o.ValueChar).ToListAsync(cancellationToken);
        }

        public async Task<ElectraProjectSetup> GetElectraProjectSetup(int carriedId, int projectId, CancellationToken cancellationToken)
        {
            return await this.context.ElectraProjectSetups
                .Where(p => p.CarrierId == carriedId && p.Id == projectId).FirstOrDefaultAsync(cancellationToken);
        }
        public async Task<ElectraProjectSetup> GetActiveElectraProjectSetup(int carriedId, string companyId, string deparment, CancellationToken cancellationToken)
        {
            return await this.context.ElectraProjectSetups
                .Where(p => p.CarrierId == carriedId && p.SysCompany == companyId && p.SysDepartment == deparment && p.IsActive == true).FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<IEnumerable<string>> GetElectraProjectOffices(CancellationToken cancellationToken)
        {
            return await this.context.ElectraProjectSetups
                .Where(p => p.SysDepartment != "service").Select(p => p.SysCompany).Distinct().ToListAsync(cancellationToken);
        }


        public async Task<IEnumerable<Dim>> GetVoucherTypes(CancellationToken cancellationToken)
        {
            return await this.context.Dims
                .Where(p => p.Name == "sys_type")
                .OrderBy(o => o.Description).ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Dim>> GetVoucherConditions(CancellationToken cancellationToken)
        {
            return await this.context.Dims
                .Where(p => p.Name == "sys_condition" && p.ValueChar != "ZZZ")
                .OrderBy(o => o.Description).ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Dim>> GetVoucherDepartments(VoucherCreateRoleType voucherCreateRoleType, CancellationToken cancellationToken)
        {
            if (voucherCreateRoleType == VoucherCreateRoleType.SERVICE)
            {
                return await this.context.Dims
               .Where(p => p.Name == "sys_department" && p.ValueChar == "service")
               .OrderBy(o => o.Description).Take(1).ToListAsync(cancellationToken);
            }
            if (voucherCreateRoleType == VoucherCreateRoleType.WAREHOUSE)
            {
                return await this.context.Dims
               .Where(p => p.Name == "sys_department")
               .OrderBy(o => o.Description).ToListAsync(cancellationToken);
            }
            return await this.context.Dims
                .Where(p => p.Name == "sys_department" && p.ValueChar != "service")
                .OrderBy(o => o.Description).ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Dim>> GetVoucherDeliveryTimes(CancellationToken cancellationToken)
        {
            return await this.context.Dims
                .Where(p => p.Name == "sys_time_delivery" && p.ValueInt != 0)
                .OrderBy(o => o.Description).ToListAsync(cancellationToken);
        }

        public async Task<ElectraProjectSetup> GetActiveElectraProjectSetupServiceOrder(string project, CancellationToken cancellationToken)
        {
            var electraProject = await this.context.ElectraProjectSetups
               .Where(p => p.MellonProject == project && p.IsActive == true).FirstOrDefaultAsync(cancellationToken);

            if (electraProject != null)
            {
                return electraProject;
            }

            return await this.context.ElectraProjectSetups
               .Where(p => p.SysCompany == "MT" && p.MellonProject == "NbgOUT" && p.IsActive == true).FirstOrDefaultAsync(cancellationToken);

        }
    }
}
