using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.Extensions.DependencyInjection;

namespace ExaminationSystem.BLL.Helpers;

public class UrlHelper
{
    public UrlHelper(IServiceProvider serviceProvider)
    {
        ServerUrls = GetApplicationUrls(serviceProvider);
    }

    public ICollection<string> ServerUrls { get; }

    private static ICollection<string> GetApplicationUrls(IServiceProvider serviceProvider)
    {
        var server = serviceProvider.GetRequiredService<IServer>();
        var addressesFeatures = server.Features.Get<IServerAddressesFeature>();

        return addressesFeatures?.Addresses ?? Array.Empty<string>();
    }
}