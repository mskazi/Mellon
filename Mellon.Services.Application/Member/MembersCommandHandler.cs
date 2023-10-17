using MediatR;
using Mellon.Services.Application.Services;
using Mellon.Services.Common.interfaces;
using Microsoft.Extensions.Logging;

namespace Mellon.Services.Application.Members.ApprovalHandlers
{
    public class MembersCommandHandler : IRequestHandler<GetCurrentMemberCommand, ElectraUser>
    {
        private readonly ILogger logger;
        private readonly ICurrentUserService currentUserService;
        public MembersCommandHandler(ICurrentUserService currentUserService, ILogger<MembersCommandHandler> logger)
        {
            this.currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
            this.logger = logger;
        }
        public async Task<ElectraUser> Handle(GetCurrentMemberCommand request, CancellationToken cancellationToken)
        {
            return (ElectraUser)this.currentUserService.CurrentUser;
        }
    }

}
