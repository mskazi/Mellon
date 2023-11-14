using MediatR;
using Mellon.Services.Infrastracture.Repositotiries;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Mellon.Services.Application.Contact
{

    public class GetContactCommandHandler : IRequestHandler<GetContactCommand, ContactResource>
    {
        private readonly ILogger logger;
        private readonly IContactsRepository repository;
        private readonly string erpUrl;

        public GetContactCommandHandler(IConfiguration configuration, IContactsRepository repository, ILogger<GetContactCommandHandler> logger)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(IMembersRepository));
            this.logger = logger;
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));
            erpUrl = configuration["Endpoints:ERP"] ?? throw new Exception("ERP Service endpoint not found in configuration.");
        }
        public async Task<ContactResource> Handle(GetContactCommand request, CancellationToken cancellationToken)
        {
            var member = await repository.GetContact(request.Id, cancellationToken);
            return new ContactResource(member);
        }
    }

}
