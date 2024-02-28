using System.Security.Cryptography;
using Catalogify.Data;
using Catalogify.Data.Entities;
using Catalogify.Data.Transfer;
using Catalogify.Services.Interface;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;

namespace Catalogify.Services;

public class AuthenticationService(IDbContextFactory<ApplicationDbContext> _factory, IHttpClientFactory _httpClientFactory) : IAuthenticationService
{
    public async Task<User?> LoginAsync(LoginUser loginUser)
    {
        await using var db = await _factory.CreateDbContextAsync();

        var user = await db.Users.SingleOrDefaultAsync(e => e.Email == loginUser.Email);

        if (user is null) return null;
        
        var hashedPw = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: loginUser.Password, salt: Convert.FromBase64String(user.Salt),
            prf: KeyDerivationPrf.HMACSHA256, iterationCount: 100000,
            numBytesRequested: 256 / 8));
            
        return hashedPw == user.PasswordHash ? user : null;
    }

    public async Task<TransferUser> RegisterAsync(CreateUser createUser)
    {
        var salt = RandomNumberGenerator.GetBytes(128 / 8);

        var hashedPw = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: createUser.Password, salt: salt, prf: KeyDerivationPrf.HMACSHA256, iterationCount: 100000,
            numBytesRequested: 256 / 8));

        await using var db = await _factory.CreateDbContextAsync();

        await db.Users.AddAsync(new User
        {
            Email = createUser.Email,
            PasswordHash = hashedPw,
            Salt = Convert.ToBase64String(salt),
            Role = "User",
            CreatedAt = DateTime.UtcNow,
            ModifiedAt = DateTime.UtcNow
        });
        
        await db.SaveChangesAsync();

        return new TransferUser() { Email = createUser.Email, PasswordHash = hashedPw, Role = "User" };
    }

    public async Task<User?> GetUserAsync(Guid id)
    {
        await using var db = await _factory.CreateDbContextAsync();

        var user = await db.Users.FindAsync(id);

        return user;
    }
    
    public async Task<User?> GetUserByEmailAsync(string email)
    {
        await using var db = await _factory.CreateDbContextAsync();

        var user = await db.Users.SingleOrDefaultAsync(e => e.Email == email);

        return user;
    }
    

    public Task SaveUserAsync(User user)
    {
        throw new NotImplementedException();
    }
}