using Google.Apis.Auth.OAuth2;
using Google.Apis.Util.Store;

namespace KebabStoreGen2.Core.Utils;

public class GoogleDriveAuthUtil
{
    public static UserCredential GetGoogleCredential(string credentialsFilePath, string tokenFilePath, string[] scopes)
    {
        using var stream = new FileStream(credentialsFilePath, FileMode.Open, FileAccess.Read);
        return GoogleWebAuthorizationBroker.AuthorizeAsync(
            GoogleClientSecrets.Load(stream).Secrets,
            scopes,
            "user",
            CancellationToken.None,
            new FileDataStore(tokenFilePath, true)).Result;
    }
}