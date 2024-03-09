using System.Text.Json.Serialization;

namespace Months18.Models;

public  partial class DocumentModel :ObservableObject
{
    public string Title { get; set; } = string.Empty;

    [JsonIgnore]
    public string Uri { get; set; } = string.Empty;

    public string FilenameDocument { get; set; } = string.Empty;

    public string FilenameThumbnail { get; set; } = string.Empty;

    [ObservableProperty]
    [property: JsonIgnore]
    private byte[] _imageBytes = [];

    [ObservableProperty]
    [property: JsonIgnore]
    private bool _isSelected;

    public static async Task<DocumentModel> Create(DocumentModel d)
    {
        var doc = new DocumentModel() {  Title = d.Title};
        var imagePath = Path.Combine(Prefernces.DocumentDataPath, d.FilenameThumbnail); 
        doc.Uri =Path.Combine(Prefernces.DocumentDataPath, d.FilenameDocument);

        await doc.LoadImageBytesAsync(imagePath);

        return doc;
    }

    private async Task LoadImageBytesAsync(string imagePath)
    {
        try
        {
            ImageBytes = await Task.Run(() => File.ReadAllBytes(imagePath));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading image bytes: {ex.Message}");
        }
    }

}
