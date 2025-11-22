namespace Recchx.Users.Application.Services;

public interface IDeviceFingerprintService
{
    string GenerateFingerprint(string ipAddress, string userAgent);
    string GetDeviceInfo(string userAgent);
}
