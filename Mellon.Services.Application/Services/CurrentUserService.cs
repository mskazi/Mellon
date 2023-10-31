using Mellon.Common.Services;
using Mellon.Services.Common.interfaces;
using Mellon.Services.Infrastracture.Repositotiries;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace Mellon.Services.Application.Services
{

    public class ElectraUser : IElectraUser
    {
        private readonly IConfiguration configuration;
        private readonly Mellon.Services.Infrastracture.Models.Member user;
        private readonly string _email;
        private readonly RoleTypeEnum _role;

        public ElectraUser(Mellon.Services.Infrastracture.Models.Member user, string _email, IConfiguration configuration)
        {
            this.user = user ?? throw new ArgumentNullException(nameof(user));
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));

            switch (user.Department)
            {
                case "office":
                    this._role = RoleTypeEnum.Office;
                    break;
                default:
                    this._role = RoleTypeEnum.None;
                    break;
            }
        }
        public int Id => user.Id;

        public string Member => user.MemberName;

        public string Department => user.Department;

        public string Company => user.Company;

        public string Email => _email;

        public bool IsActive => user.IsActive;

        public string Country => user.SysCountry;

        public RoleTypeEnum Role => _role;
    }

    public class CurrentUserService : ICurrentUserService
    {
        private IElectraUser _currentUser;
        private readonly IMembersRepository membersRepository;
        private readonly ILogger<CurrentUserService> logger;
        private readonly IConfiguration configuration;
        private ClaimsPrincipal principal;
        public CurrentUserService(
          ClaimsPrincipal principal,
          IMembersRepository membersRepository,
          ILogger<CurrentUserService> logger,
          IConfiguration configuration)
        {
            this.membersRepository = membersRepository ?? throw new ArgumentNullException(nameof(membersRepository));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this.principal = principal;
        }

        public IElectraUser CurrentUser
        {
            get
            {
                if (_currentUser is null)
                {
                    //what happens if we dont have claims principal?
                    var preferredUsername = principal?.Claims?.FirstOrDefault(s => s.Type == "name")?.Value;

                    if (string.IsNullOrWhiteSpace(preferredUsername))
                        throw new ArgumentNullException(nameof(principal));

                    var currentUser = this.membersRepository.GetMember(preferredUsername);

                    if (currentUser == null)
                        throw new UnauthorizedException($"User '{preferredUsername}' does not exist.", nameof(Member));

                    if (currentUser.IsActive == false)
                        throw new UnauthorizedException($"User '{currentUser.MemberName}' ({currentUser.Id}) is not active.", nameof(Member));



                    _currentUser = new ElectraUser(currentUser, "addEmail", configuration);
                }
                return _currentUser;
            }
        }
    }
}
