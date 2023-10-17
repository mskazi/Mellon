namespace Mellon.Services.Common.interfaces
{
    public interface IElectraUser
    {
        int Id { get; }
        string Member { get; }
        string Department { get; }
        string Company { get; }
        string Email { get; }
        bool IsActive { get; }
        string Country { get; }

    }
    public interface ICurrentUserService
    {
        IElectraUser CurrentUser { get; }
    }

    public enum RoleTypeEnum
    {
        None,
        Office
    }


}
