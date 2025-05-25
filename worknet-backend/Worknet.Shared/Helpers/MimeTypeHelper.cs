using Worknet.Core.Enums;

namespace Worknet.Shared.Helpers;
public static class MimeTypeHelper
{
    private static readonly Dictionary<string, MimeType> ExtensionToMimeEnum = new()
    {
        [".png"] = MimeType.ImagePng,
        [".jpg"] = MimeType.ImageJpeg,
        [".jpeg"] = MimeType.ImageJpeg,
        [".pdf"] = MimeType.ApplicationPdf,
        [".txt"] = MimeType.TextPlain,
        [".gif"] = MimeType.ImageGif,
        [".json"] = MimeType.ApplicationJson,
        [".zip"] = MimeType.ApplicationZip,
        [".doc"] = MimeType.ApplicationMsWord,
        [".docx"] = MimeType.ApplicationMsWord,
        [".xls"] = MimeType.ApplicationVndMsExcel,
        [".xlsx"] = MimeType.ApplicationVndMsExcel,
        [".mp4"] = MimeType.VideoMp4,
    };

    private static readonly Dictionary<MimeType, string> EnumToMimeString = new()
    {
        [MimeType.ImagePng] = "image/png",
        [MimeType.ImageJpeg] = "image/jpeg",
        [MimeType.ApplicationPdf] = "application/pdf",
        [MimeType.TextPlain] = "text/plain",
        [MimeType.ImageGif] = "image/gif",
        [MimeType.ApplicationJson] = "application/json",
        [MimeType.ApplicationZip] = "application/zip",
        [MimeType.ApplicationMsWord] = "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
        [MimeType.ApplicationVndMsExcel] = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
        [MimeType.VideoMp4] = "video/mp4"
    };

    private static readonly Dictionary<string, string> ExtensionToString= new()
    {
        [".png"] = "image/png",
        [".jpg"] = "image/jpeg",
        [".jpeg"] = "image/jpeg",
        [".pdf"] = "application/pdf",
        [".txt"] = "text/plain",
        [".gif"] = "image/gif",
        [".json"] = "application/json",
        [".zip"] = "application/zip",
        [".doc"] = "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
        [".docx"] = "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
        [".xls"] = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
        [".xlsx"] = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
        [".mp4"] = "video/mp4",
    };

    public static MimeType? GetMimeEnumFromExtension(string extension)
    {
        extension = extension.ToLower();
        return ExtensionToMimeEnum.TryGetValue(extension, out var mime) ? mime : null;
    }

    public static string? GetMimeString(MimeType mimeType)
    {
        return EnumToMimeString.TryGetValue(mimeType, out var mime) ? mime : null;
    }
    public static string? GetStringFromMimeString(string extension)
    {
        return ExtensionToString.TryGetValue(extension, out var mime) ? mime : null;
    }
}