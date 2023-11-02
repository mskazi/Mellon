
using Mellon.Common.Services;

namespace Mellon.Services.Application.Contact
{
    public class ContactResource : ContactResourceData
    {
        public ContactResource() { }

        public ContactResource(Mellon.Services.Infrastracture.Models.OfficeContact contact)
            : base(contact)
        {
            Id = contact.Id;
        }

        public int Id { get; set; }

        protected override void Validate()
        {
            base.Validate();
            Guards.ValidIdentifier(Id);
        }
    }

    public class ContactResourceData
    {
        public ContactResourceData() { }

        public ContactResourceData(Mellon.Services.Infrastracture.Models.OfficeContact contact)
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
            SysCompany = contact?.SysCompany;
            Active = contact?.Active ?? true;
        }


        public string? VoucherFrom { get; set; }

        public string? VoucherName { get; set; }

        public string? VoucherContact { get; set; }

        public string VoucherAddress { get; set; } = null!;

        public string VoucherCity { get; set; } = null!;

        public string VoucherPostCode { get; set; } = null!;

        public string VoucherCountry { get; set; } = null!;

        public string? VoucherPhoneNo { get; set; }

        public string? VoucherMobileNo { get; set; }

        public string? ContactNotes { get; set; }

        public string SysCompany { get; set; } = null!;

        public bool Active { get; set; }

        protected virtual void Validate()
        {
            Guards.StringNotNullOrEmpty(VoucherName, nameof(VoucherName));
            Guards.StringMaximumLength(VoucherName, nameof(VoucherName), 600);
            Guards.StringNotNullOrEmpty(VoucherPostCode, nameof(VoucherPostCode));
            Guards.StringMaximumLength(VoucherPostCode, nameof(VoucherPostCode), 6);
            Guards.StringNotNullOrEmpty(VoucherCity, nameof(VoucherCity));
            Guards.StringMaximumLength(VoucherCity, nameof(VoucherCity), 100);
            Guards.StringNotNullOrEmpty(VoucherCountry, nameof(VoucherCountry));
            Guards.StringMaximumLength(VoucherCountry, nameof(VoucherCountry), 100);


        }
    }

}
