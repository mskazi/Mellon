using MediatR;
using Mellon.Common.Services;
using Mellon.Services.Common.interfaces;
using Mellon.Services.Infrastracture.Repositotiries;
using Microsoft.Extensions.Logging;

namespace Mellon.Services.Application.Member
{
    public class GetMembersCommandHandler : IRequestHandler<GetMembersServiceCommand, PaginatedListResult<MemberResource>>
    {
        private readonly ILogger logger;
        private readonly ICurrentUserService currentUserService;
        private readonly IMembersRepository repository;

        public GetMembersCommandHandler(ICurrentUserService currentUserService, ILogger<GetMembersCommandHandler> logger, IMembersRepository repository)
        {
            this.currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
            this.logger = logger;
            this.repository = repository;
        }

        public async Task<PaginatedListResult<MemberResource>> Handle(GetMembersServiceCommand request, CancellationToken cancellationToken)
        {
            PaginatedListResult<Mellon.Services.Infrastracture.Models.Member> result = await repository.GetMembers(request.Term, request.Paging, request.Ordering, cancellationToken);
            return new PaginatedListResult<MemberResource>(
               result.Start,
               result.Length,
               result.Total,
               result.Data.Select(entity => new MemberResource(entity))
            );
        }
    }
}
