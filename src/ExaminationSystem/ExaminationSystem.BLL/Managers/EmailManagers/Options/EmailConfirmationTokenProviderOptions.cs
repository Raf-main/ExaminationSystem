using Microsoft.AspNetCore.Identity;

namespace ExaminationSystem.BLL.Managers.EmailManagers.Options;

public class EmailConfirmationTokenProviderOptions : DataProtectionTokenProviderOptions
{
    public EmailConfirmationTokenProviderOptions()
    {
        Name = "EmailDataProtectorTokenProvider";
    }
}