using MediatR;
using Mellon.Services.Application.Services;
using Mellon.Services.Common.interfaces;
using Mellon.Services.Infrastracture.Repositotiries;
using Microsoft.Extensions.Logging;

namespace Mellon.Services.Application.Member
{
    public class GetCurrentMemberCommandHandler : IRequestHandler<GetCurrentMemberCommand, ElectraUser>
    {
        private readonly ILogger logger;
        private readonly ICurrentUserService currentUserService;
        public GetCurrentMemberCommandHandler(ICurrentUserService currentUserService, ILogger<GetCurrentMemberCommandHandler> logger)
        {
            this.currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
            this.logger = logger;
        }
        public async Task<ElectraUser> Handle(GetCurrentMemberCommand request, CancellationToken cancellationToken)
        {
            return (ElectraUser)currentUserService.CurrentUser;
        }
    }

    public class GetMemberCommandHandler : IRequestHandler<GetMemberCommand, MemberResource>
    {
        private readonly ILogger logger;
        private readonly IMembersRepository repository;
        public GetMemberCommandHandler(IMembersRepository repository, ILogger<GetCurrentMemberCommandHandler> logger)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(IMembersRepository));
            this.logger = logger;
        }
        public async Task<MemberResource> Handle(GetMemberCommand request, CancellationToken cancellationToken)
        {
            var member = await repository.GetMemberById(request.Id, cancellationToken);
            return new MemberResource(member);
        }
    }

}
