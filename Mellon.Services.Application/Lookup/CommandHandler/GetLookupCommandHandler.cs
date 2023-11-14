using MediatR;
using Mellon.Common.Services;
using Mellon.Services.Common.interfaces;
using Mellon.Services.Infrastracture.Repositotiries;

namespace Mellon.Services.Application.Lookup
{
    public class GetDepartmentLookupCommandHandler : IRequestHandler<GetDepartmentookupCommand, ListResult<DepartmentLookupResourse>>
    {
        private readonly ILookupRepository repository;

        public GetDepartmentLookupCommandHandler(ILookupRepository repository)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<ListResult<DepartmentLookupResourse>> Handle(GetDepartmentookupCommand request, CancellationToken cancellationToken)
        {
            var entities = await repository.GetDepartments(cancellationToken);
            var items = entities.Select(entity => new DepartmentLookupResourse(entity));
            return new ListResult<DepartmentLookupResourse>(items);
        }
    }


    public class GetCompanyLookupCommandHandler : IRequestHandler<GeCompanyLookupCommand, ListResult<CompanyLookupResourse>>
    {
        private readonly ILookupRepository repository;
        private readonly ICurrentUserService currentUserService;

        public GetCompanyLookupCommandHandler(ILookupRepository repository, ICurrentUserService currentUserService)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
            this.currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
        }

        public async Task<ListResult<CompanyLookupResourse>> Handle(GeCompanyLookupCommand request, CancellationToken cancellationToken)
        {
            var entities = await repository.GetCompanies(request.VouucherCreateRoleType, cancellationToken);
            var items = entities.Select(entity => new CompanyLookupResourse(entity));
            return new ListResult<CompanyLookupResourse>(items);
        }
    }

    public class GetCountryLookupCommandHandler : IRequestHandler<GeCountryLookupCommand, ListResult<CountryLookupResourse>>
    {
        private readonly ILookupRepository repository;
        private readonly ICurrentUserService currentUserService;

        public GetCountryLookupCommandHandler(ILookupRepository repository, ICurrentUserService currentUserService)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
            this.currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
        }

        public async Task<ListResult<CountryLookupResourse>> Handle(GeCountryLookupCommand request, CancellationToken cancellationToken)
        {
            var entities = await repository.GetCountries(cancellationToken);
            var items = entities.Select(entity => new CountryLookupResourse(entity));
            return new ListResult<CountryLookupResourse>(items);
        }
    }

}
