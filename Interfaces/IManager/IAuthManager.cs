
using Models.DTOs;

namespace Interfaces.IManager
{
    public interface IAuthManager
    {
        Task<Result> Register(RegisterDto register);
        Task<Result<AuthResponseDto>> LoginAsync(LoginDto login);
        Task<Result> AssignRole(string userId, string role);
        Task<Result<AuthResponseDto>> Refreshtoken(RefreshRequestDto request);
    }
}
