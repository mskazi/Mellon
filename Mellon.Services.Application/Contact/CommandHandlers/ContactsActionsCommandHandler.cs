using MediatR;
using Mellon.Common.Services;
using Mellon.Services.Application.Contact;
using Mellon.Services.Common.interfaces;
using Mellon.Services.Infrastracture.Repositotiries;
using Microsoft.Extensions.Logging;


namespace Mellon.Services.Application.Contacts
{
    public class ContactsActionsCommandHandler : IRequestHandler<ContactCreateCommand, ContactResource>, IRequestHandler<ContactUpdateCommand, ContactResource>
    {
        private readonly IContactsRepository repository;
        private readonly ILogger<ContactsActionsCommandHandler> logger;
        private readonly ICurrentUserService currentUserService;

        public ContactsActionsCommandHandler(IContactsRepository repository, ILogger<ContactsActionsCommandHandler> logger, ICurrentUserService currentUserService)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
        }

        public async Task<ContactResource> Handle(ContactCreateCommand request, CancellationToken cancellationToken)
        {
            var entity = new Mellon.Services.Infrastracture.Models.OfficeContact()
            {
                VoucherFrom = request?.VoucherFrom,
                VoucherName = request?.VoucherName,
                VoucherContact = request?.VoucherContact,
                VoucherAddress = request?.VoucherAddress,
                VoucherCity = request?.VoucherCity,
                VoucherPostCode = request?.VoucherPostCode,
                VoucherCountry = request?.VoucherCountry,
                VoucherPhoneNo = request?.VoucherPhoneNo,
                VoucherMobileNo = request?.VoucherMobileNo,
                ContactNotes = request?.ContactNotes,
                Flag0 = (int)request.Mode,
                CreatedBy = this.currentUserService.CurrentUser.Email,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                UpdatedBy = this.currentUserService.CurrentUser.Email,
            };
            repository.AddContact(entity);
            await repository.UnitOfWork.SaveChangesAsync(cancellationToken);

            return new ContactResource(entity);
        }

        public async Task<ContactResource> Handle(ContactUpdateCommand request, CancellationToken cancellationToken)
        {
            Mellon.Services.Infrastracture.Models.OfficeContact existingEntity = await repository.GetContact(request.Id, cancellationToken);

            if (existingEntity is null)
                throw new NotFoundException(nameof(ContactResource), request.Id);

            existingEntity.VoucherFrom = request?.VoucherFrom;
            existingEntity.VoucherName = request?.VoucherName;
            existingEntity.VoucherContact = request?.VoucherContact;
            existingEntity.VoucherAddress = request?.VoucherAddress;
            existingEntity.VoucherCity = request?.VoucherCity;
            existingEntity.VoucherPostCode = request?.VoucherPostCode;
            existingEntity.VoucherCountry = request?.VoucherCountry;
            existingEntity.VoucherPhoneNo = request?.VoucherPhoneNo;
            existingEntity.VoucherMobileNo = request?.VoucherMobileNo;
            existingEntity.ContactNotes = request?.ContactNotes;
            existingEntity.Flag0 = (int)request.Mode;
            existingEntity.UpdatedAt = DateTime.Now;
            existingEntity.UpdatedBy = this.currentUserService.CurrentUser.Email;
            await repository.UnitOfWork.SaveChangesAsync(cancellationToken);
            return new ContactResource(existingEntity);
        }

    }
}