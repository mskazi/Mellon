using MediatR;
using Mellon.Services.Application.Services;

namespace Mellon.Services.Application.Members
{
    public class GetCurrentMemberCommand : IRequest<ElectraUser>
    {
    }
}
