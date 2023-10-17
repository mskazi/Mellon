using Mellon.Services.Infrastracture.Base;
using Mellon.Services.Infrastracture.Context;
using Mellon.Services.Infrastracture.Models;


namespace Mellon.Services.Infrastracture.Repositotiries
{
    public interface IMembersRepository : IRepository
    {
        Member GetMember(string name);
    }

    public class MembersRepository : IMembersRepository
    {
        private readonly MellonContext context;
        public MembersRepository(MellonContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public IUnitOfWork UnitOfWork => context;

        public Member GetMember(string name)
        {
            return context.Members.FirstOrDefault(x => x.MemberName == name);
        }
    }
}
