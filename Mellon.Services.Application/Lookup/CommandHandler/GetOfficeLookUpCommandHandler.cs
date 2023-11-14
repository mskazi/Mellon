using MediatR;
using Mellon.Common.Services;
using Mellon.Services.Application.Lookup.Commands;
using Mellon.Services.Infrastracture.Repositotiries;

namespace Mellon.Services.Application.Lookup.CommandHandler
{
    public class GetOfficeLookUpCommandHandler : IRequestHandler<GetElectraProjectOfficesLookupCommand, ListResult<string>>
    {
        private readonly ILookupRepository repository;

        public GetOfficeLookUpCommandHandler(ILookupRepository repository)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<ListResult<string>> Handle(GetElectraProjectOfficesLookupCommand request, CancellationToken cancellationToken)
        {
            var entities = await repository.GetElectraProjectOffices(cancellationToken);
            return new ListResult<string>(entities);
        }
    }
}
