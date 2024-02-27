using Catalogify.Data.Entities;
using Catalogify.Data.Transfer;

namespace Catalogify.Services.Interface;

public interface IAuthenticationService
{
    public Task<User?> LoginAsync(LoginUser loginUser);

    public Task<TransferUser> RegisterAsync(CreateUser createUser);
    
    public Task<User?> GetUserAsync(Guid id);
    
    public Task SaveUserAsync(User user);
}