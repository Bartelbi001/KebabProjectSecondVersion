using CSharpFunctionalExtensions;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using KebabStoreGen2.Core.Abstractions;
using KebabStoreGen2.Core.Models;
using KebabStoreGen2.Core.Utils;
using Microsoft.AspNetCore.Http;

namespace KebabStoreGen2.Application.Services;

public class GoogleDriveService : IGoogleDriveService
{
    static string[] Scopes = { DriveService.Scope.DriveFile };
    static string ApplicationName = "KebabProjectGen2";
    private readonly DriveService _driveService;

    public GoogleDriveService()
    {
        var credentialsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Credentials", "credentials.json");
        var tokenPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "token.json");
        var credential = GoogleDriveAuthUtil.GetGoogleCredential(credentialsPath, tokenPath, Scopes);

        _driveService = new DriveService(new BaseClientService.Initializer()
        {
            HttpClientInitializer = credential,
            ApplicationName = ApplicationName,
        });
    }

    public async Task<Result<Image>> CreateImage(IFormFile file, string fileName)
    {
        if (file == null || file.Length == 0)
        {
            return Result.Failure<Image>("Invalid file");
        }

        var uniqueFileName = $"{Guid.NewGuid()}_{fileName}";
        var fileMetadata = new Google.Apis.Drive.v3.Data.File
        {
            Name = uniqueFileName,
            MimeType = file.ContentType,
        };

        using var stream = new MemoryStream();
        await file.CopyToAsync(stream);
        stream.Position = 0;

        var request = _driveService.Files.Create(fileMetadata, stream, file.ContentType);
        request.Fields = "id";
        var response = await request.UploadAsync();

        if (response.Status != Google.Apis.Upload.UploadStatus.Completed)
        {
            return Result.Failure<Image>("Error uploading file to Google Drive");
        }

        var fileId = request.ResponseBody.Id;
        var imageUrl = $"https://drive.google.com/uc?id={fileId}";

        var result = Image.Create(uniqueFileName, imageUrl);

        if (result.IsFailure)
        {
            return Result.Failure<Image>(result.Error);
        }

        return Result.Success(result.Value);
    }
}