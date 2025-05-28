Console.WriteLine("Starting Holtz.ChunkDownloader...");
Console.WriteLine();
Console.WriteLine("Here is a example for base URL: https://example.com/chunks/my-id-video-segment-{0}.ts");

string baseUrl = "";
Console.Write("Enter the base URL for the chunk files: ");
baseUrl = Console.ReadLine()?.Trim() ?? "";

while (string.IsNullOrEmpty(baseUrl))
{
    Console.Write("Base URL cannot be empty. Please enter a valid base URL: ");
    baseUrl = Console.ReadLine()?.Trim() ?? "";
}

Console.WriteLine();

string contentType = "";
Console.Write("Enter the Content-Type for the chunk files: ");
contentType = Console.ReadLine()?.Trim() ?? "";

while (string.IsNullOrEmpty(contentType))
{
    Console.Write("Content-Type cannot be empty. Please enter a valid Content-Type: ");
    contentType = Console.ReadLine()?.Trim() ?? "";
}

string downloadName = "";
Console.Write("Enter the Download Name (the chunks will be downloaded into this folder): ");
downloadName = Console.ReadLine()?.Trim() ?? Guid.NewGuid().ToString();

// TODO: Ask for the next fields
int startIndex = 1;
string fileExtension = ".ts";

Console.WriteLine("Url: " + string.Format(baseUrl, startIndex));
Console.WriteLine("Content-Type: " + contentType);


using (var client = new HttpClient())
{
    try
    {
        // TODO: Foreach loop to download multiple chunks
        client.DefaultRequestHeaders.Add("Accept", contentType);
        string url = string.Format(baseUrl, startIndex);
        Console.WriteLine("Downloading from URL: " + url);
        var response = await client.GetAsync(url);
        response.EnsureSuccessStatusCode();

        string baseDirectory = Path.Combine(".downloads", downloadName);
        // string fileName = Path.Combine(Directory.GetCurrentDirectory(), downloadName, startIndex + fileExtension);
        if (!Directory.Exists(baseDirectory))
            Directory.CreateDirectory(baseDirectory);

        string filePath = Path.Combine(baseDirectory, startIndex + fileExtension);
        Console.WriteLine("Saving to file: " + filePath);
        using (var stream = response.Content.ReadAsStream())
        using (var file = new FileStream(filePath, FileMode.Create, FileAccess.Write))
        {
            stream.CopyTo(file);
        }
        Console.WriteLine("Download completed successfully.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"An error occurred during download: {ex.Message}");
        Console.ReadLine();
    }
}

Console.WriteLine("Finishing Holtz.ChunkDownloader...");
Console.ReadLine();
