﻿using MediatR;
using Mellon.Common.Services;
using Mellon.Services.Common.interfaces;
using Mellon.Services.Infrastracture.Repositotiries;
using Microsoft.Extensions.Logging;

namespace Mellon.Services.Application.Member
{
    public class GetMembersCommandHandler :
        IRequestHandler<GetMembersServiceCommand, PaginatedListResult<MemberResource>>,
        IRequestHandler<GetAllActiveMembersByDepartmentCommand, ListResult<MemberResource>>,
        IRequestHandler<GetAllActiveMembersCommand, ListResult<MemberResource>>
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

        public async Task<ListResult<MemberResource>> Handle(GetAllActiveMembersByDepartmentCommand request, CancellationToken cancellationToken)
        {

            var entities = await repository.GetAllActiveMembersByDepartment(request.Company, request.Department, cancellationToken);
            var items = entities.Select(entity => new MemberResource(entity));
            return new ListResult<MemberResource>(items);
        }

        public async Task<ListResult<MemberResource>> Handle(GetAllActiveMembersCommand request, CancellationToken cancellationToken)
        {
            var entities = await repository.GetAllMembers(cancellationToken);
            var items = entities.Select(entity => new MemberResource(entity));
            return new ListResult<MemberResource>(items);
        }
    }
}
