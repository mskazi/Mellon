using MediatR;
using Mellon.Services.Infrastracture.Repositotiries;
using Microsoft.Extensions.Logging;

namespace Mellon.Services.Application.Contact
{

    public class GetContactCommandHandler : IRequestHandler<GetContactCommand, ContactResource>
    {
        private readonly ILogger logger;
        private readonly IContactsRepository repository;
        public GetContactCommandHandler(IContactsRepository repository, ILogger<GetContactCommandHandler> logger)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(IMembersRepository));
            this.logger = logger;
        }
        public async Task<ContactResource> Handle(GetContactCommand request, CancellationToken cancellationToken)
        {
            var member = await repository.GetContact(request.Id, cancellationToken);
            return new ContactResource(member);
        }
    }

}
