using System.Security.Cryptography;
using System.Text;
using Recchx.Users.Application.Services;

namespace Recchx.Users.Infrastructure.Services;

public class DeviceFingerprintService : IDeviceFingerprintService
{
    public string GenerateFingerprint(string ipAddress, string userAgent)
    {
        var input = $"{ipAddress}:{userAgent}";
        using var sha256 = SHA256.Create();
        var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
        return Convert.ToBase64String(hashBytes);
    }

    public string GetDeviceInfo(string userAgent)
    {
        if (string.IsNullOrWhiteSpace(userAgent))
            return "Unknown Device";

        userAgent = userAgent.ToLower();
        
        // Simple device detection
        if (userAgent.Contains("mobile") || userAgent.Contains("android") || userAgent.Contains("iphone"))
            return "Mobile Device";
        if (userAgent.Contains("tablet") || userAgent.Contains("ipad"))
            return "Tablet";
        if (userAgent.Contains("windows") || userAgent.Contains("mac") || userAgent.Contains("linux"))
            return "Desktop";
        
        return "Unknown Device";
    }
}
