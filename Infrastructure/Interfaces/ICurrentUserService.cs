namespace Infrastructure.Interfaces;

public interface ICurrentUserService
{
    string Email { get; }
    string DisplayName { get; }
    string EmployeeCode { get; }
}

