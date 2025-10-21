Console.WriteLine("Starting Holtz.ChunkDownloader...");
Console.WriteLine();
Console.WriteLine("Here is a example for base URL: https://example.com/chunks/my-id-video-segment-{0}.ts");
Console.WriteLine();

string baseUrl = "";
Console.Write("Enter the base URL for the chunk files: ");
baseUrl = Console.ReadLine()?.Trim() ?? "";

while (string.IsNullOrEmpty(baseUrl))
{
    Console.WriteLine();
    Console.Write("Base URL cannot be empty. Please enter a valid base URL: ");
    baseUrl = Console.ReadLine()?.Trim() ?? "";
}

Console.WriteLine();
Console.Write("Enter the Content-Type for the chunk files: ");
string contentType = Console.ReadLine()?.Trim() ?? "";

Console.WriteLine();
Console.Write("Enter the Download Name (the chunks will be downloaded into this folder): ");
string downloadName = Console.ReadLine()?.Trim() ?? Guid.NewGuid().ToString();

// TODO: Ask for the next fields
int startIndex = 1;
int maxLoops = 100 * 1000;
int delayInMilliseconds = 2000;
string fileExtension = ".ts";

Console.WriteLine("Url: " + string.Format(baseUrl, startIndex));
Console.WriteLine("Content-Type: " + contentType);
Console.WriteLine("Starting chunk downloads...");
Console.WriteLine();

using (var client = new HttpClient())
{
    if (!string.IsNullOrWhiteSpace(contentType))
        client.DefaultRequestHeaders.Add("Accept", contentType);
    try
    {
        for (int i = startIndex; i < maxLoops; i++)
        {
            Console.WriteLine($"Downloading chunk {i} of {maxLoops}...");
            string url = string.Format(baseUrl, i);

            Console.WriteLine(" => Downloading from: " + url);
            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();

            string baseDirectory = Path.Combine(".downloads", downloadName);
            if (!Directory.Exists(baseDirectory))
                Directory.CreateDirectory(baseDirectory);

            string filePath = Path.Combine(baseDirectory, i + fileExtension);
            Console.WriteLine(" => Saving to file: " + filePath);
            using (var stream = response.Content.ReadAsStream())
            using (var file = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                stream.CopyTo(file);
            }
            Console.WriteLine(" => Download completed successfully.");
            Console.WriteLine();
            Thread.Sleep(delayInMilliseconds);
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"An error occurred during download: {ex.Message}");
        Console.ReadLine();
    }
}

Console.WriteLine("Finishing Holtz.ChunkDownloader...");
Console.ReadLine();
