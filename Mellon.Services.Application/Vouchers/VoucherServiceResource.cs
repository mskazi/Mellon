using Mellon.Services.Application.Carrier;
using Mellon.Services.Application.Contact;
using Mellon.Services.Application.Lookup;

namespace Mellon.Services.Application.Vouchers
{
    public class VoucherServiceOrderResource
    {
        public VoucherServiceOrderResource(ContactResourceData contact, IEnumerable<CarrierLookupResourse> carries, IEnumerable<ProjectLookupResourse> project, string member)
        {
            this.Contact = contact;
            this.Carries = carries;
            this.Project = project;
            this.Member = member;

        }

        public ContactResourceData Contact { get; }

        public IEnumerable<CarrierLookupResourse> Carries { get; }

        public IEnumerable<ProjectLookupResourse> Project { get; }

        public string Member { get; }


    }
}
