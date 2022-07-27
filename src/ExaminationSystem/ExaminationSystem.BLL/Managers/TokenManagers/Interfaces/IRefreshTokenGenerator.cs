using ExaminationSystem.BLL.Models;
using ExaminationSystem.DAL.Entities;

namespace ExaminationSystem.BLL.Managers.TokenManagers.Interfaces;

public interface IRefreshTokenGenerator
{
    string GenerateRefreshToken();
    public UserRefreshToken GenerateUserRefreshToken(ApplicationUser user, DateTime? utcExpirationDate = null);
}