using MediatR;
using Mellon.Common.Services;
using Mellon.Services.Common.interfaces;
using Mellon.Services.Infrastracture.Models;
using Mellon.Services.Infrastracture.Repositotiries;
using Microsoft.Extensions.Logging;

namespace Mellon.Services.Application.Contact
{
    public class GetContactsCommandHandler : IRequestHandler<GetContactsCommand, PaginatedListResult<ContactResource>>
    {
        private readonly ILogger logger;
        private readonly ICurrentUserService currentUserService;
        private readonly IContactsRepository repository;

        public GetContactsCommandHandler(ICurrentUserService currentUserService, ILogger<GetContactsCommandHandler> logger, IContactsRepository repository)
        {
            this.currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
            this.logger = logger;
            this.repository = repository;
        }

        public async Task<PaginatedListResult<ContactResource>> Handle(GetContactsCommand request, CancellationToken cancellationToken)
        {
            PaginatedListResult<OfficeContact> result = await repository.GetContacts(request.Term, request.Paging, request.Ordering, cancellationToken);
            return new PaginatedListResult<ContactResource>(
               result.Start,
               result.Length,
               result.Total,
               result.Data.Select(entity => new ContactResource(entity))
            );
        }
    }
}
