
namespace IntegrationTests.Helpers;

public class UriHelper
{

    public static string ExtractGameIdFromCreatedLocationUri(Uri uri)
    {
        string[] segments = uri.ToString().Trim('/').Split('/');
        string id = segments[^1];

        return id;
    }

}

