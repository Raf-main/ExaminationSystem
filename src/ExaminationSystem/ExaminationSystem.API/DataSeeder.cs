using ExaminationSystem.DAL.Entities;
using Microsoft.AspNetCore.Identity;

// let it be here
namespace ExaminationSystem.API;

public class DataSeeder
{
    private readonly UserManager<ApplicationUser> _userManager;

    public DataSeeder(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public void SeedUsers()
    {
        var users = new List<ApplicationUser>
        {
            new()
            {
                Id = Guid.NewGuid().ToString(),
                Email = "rafikrrrafik@gmail.com",
                EmailConfirmed = true,
                UserName = "Rafik",
                NormalizedUserName = "RAFIKRRRAFIK@GMAIL.COM"
            },
            new()
            {
                Id = Guid.NewGuid().ToString(),
                Email = "rafikrrrafik3@gmail.com",
                EmailConfirmed = true,
                UserName = "Rafik3",
                NormalizedUserName = "RAFIKRRRAFIK3@GMAIL.COM"
            }
        };

        if (users.Select(user => _userManager.CreateAsync(user, "Raf1234!"))
            .Any(identityResult => !identityResult.Result.Succeeded))
        {
            throw new ApplicationException();
        }
    }
}