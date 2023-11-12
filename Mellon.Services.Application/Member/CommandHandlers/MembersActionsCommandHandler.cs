using MediatR;
using Mellon.Common.Services;
using Mellon.Services.Common.interfaces;
using Mellon.Services.Infrastracture.Repositotiries;
using Microsoft.Extensions.Logging;


namespace Mellon.Services.Application.Member
{
    public class MembersActionsCommandHandler : IRequestHandler<MemberCreateCommand, MemberResource>, IRequestHandler<MemberUpdateCommand, MemberResource>
    {
        private readonly IMembersRepository repository;
        private readonly ILogger<MembersActionsCommandHandler> logger;
        private readonly ICurrentUserService currentUserService;

        public MembersActionsCommandHandler(IMembersRepository repository, ILogger<MembersActionsCommandHandler> logger, ICurrentUserService currentUserService)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
        }

        public async Task<MemberResource> Handle(MemberCreateCommand request, CancellationToken cancellationToken)
        {
            var entity = new Mellon.Services.Infrastracture.Models.Member()
            {
                Company = request.Company,
                IsActive = request.IsActive,
                Department = request.Department,
                MemberName = request.MemberName,
                SysCountry = "GR",
                CreatedBy = this.currentUserService.CurrentUser.Email,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                UpdatedBy = this.currentUserService.CurrentUser.Email,
            };
            repository.AddMember(entity);
            await repository.UnitOfWork.SaveChangesAsync(cancellationToken);

            logger.LogInformation("Created new member with id {id} and company {company}",
                entity.Id,
                entity.Company);

            return new MemberResource(entity);
        }

        public async Task<MemberResource> Handle(MemberUpdateCommand request, CancellationToken cancellationToken)
        {
            Mellon.Services.Infrastracture.Models.Member existingEntity = await repository.GetMemberById(request.Id, cancellationToken);

            if (existingEntity is null)
                throw new NotFoundException(nameof(MemberResource), request.Id);

            existingEntity.Company = request.Company;
            existingEntity.IsActive = request.IsActive;
            existingEntity.Department = request.Department;
            existingEntity.Department = request.Department;
            existingEntity.MemberName = request.MemberName;
            existingEntity.SysCountry = this.currentUserService.CurrentUser.Country;
            existingEntity.UpdatedAt = DateTime.Now;
            existingEntity.UpdatedBy = this.currentUserService.CurrentUser.Email;

            await repository.UnitOfWork.SaveChangesAsync(cancellationToken);

            logger.LogInformation("Updated  member with id {id} and company {company}",
              request.Id,
              request.Company);

            return new MemberResource(existingEntity);
        }

    }
}