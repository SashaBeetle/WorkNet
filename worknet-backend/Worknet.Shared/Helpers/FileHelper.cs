namespace Worknet.Shared.Helpers;
public static class FileHelper
{
    public static string GetPublicImageUrl(string fileId) => $"https://drive.google.com/thumbnail?id={fileId}";
}