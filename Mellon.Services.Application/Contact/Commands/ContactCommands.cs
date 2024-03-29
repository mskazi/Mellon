﻿using MediatR;
using Mellon.Common.Services;

namespace Mellon.Services.Application.Contact
{
    public class GetContactsCommand : IRequest<PaginatedListResult<ContactResource>>
    {
        public GetContactsCommand(int role, string term, ListPaging paging, ListOrder ordering)
        {
            Role = role;
            Term = term;
            Paging = paging;
            Ordering = ordering;
        }

        public int Role { get; set; }
        public string? Term { get; set; }
        public ListPaging Paging { get; }
        public ListOrder Ordering { get; }
    }

    public class GetContactCommand : IRequest<ContactResource>
    {
        public GetContactCommand(int id)
        {
            Id = id;
        }
        public int Id { get; set; }
    }

    public class ContactCreateCommand : ContactResourceData, IRequest<ContactResource>
    {
        public ContactCreateCommand(ContactResourceData contact)
        {
            VoucherFrom = contact?.VoucherFrom;
            VoucherName = contact?.VoucherName;
            VoucherContact = contact?.VoucherContact;
            VoucherAddress = contact?.VoucherAddress;
            VoucherCity = contact?.VoucherCity;
            VoucherPostCode = contact?.VoucherPostCode;
            VoucherCountry = contact?.VoucherCountry;
            VoucherPhoneNo = contact?.VoucherPhoneNo;
            VoucherMobileNo = contact?.VoucherMobileNo;
            ContactNotes = contact?.ContactNotes;
            Mode = contact?.Mode;
            Validate();
        }
    }

    public class ContactUpdateCommand : ContactResource, IRequest<ContactResource>
    {
        public ContactUpdateCommand(ContactResource contact)
        {
            Id = contact.Id;
            VoucherFrom = contact?.VoucherFrom;
            VoucherName = contact?.VoucherName;
            VoucherContact = contact?.VoucherContact;
            VoucherAddress = contact?.VoucherAddress;
            VoucherCity = contact?.VoucherCity;
            VoucherPostCode = contact?.VoucherPostCode;
            VoucherCountry = contact?.VoucherCountry;
            VoucherPhoneNo = contact?.VoucherPhoneNo;
            VoucherMobileNo = contact?.VoucherMobileNo;
            ContactNotes = contact?.ContactNotes;
            Mode = contact?.Mode;
            Validate();
        }
    }

}