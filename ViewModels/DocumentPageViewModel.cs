using System.Text.Json;
using System.Text.Json.Serialization;

namespace Months18.ViewModels;

public partial class DocumentPageViewModel : ObservableObject
{
    public DocumentPageViewModel()
    {
        _source = [];
        _ = LoadLocalDocumentsAsync();
    }

    private readonly List<DocumentModel> _source;
    private event EventHandler? SelectionChanged;

    [ObservableProperty]
    private DocumentModel? _selectedDocument;

    [ObservableProperty]
    private ObservableCollection<DocumentModel> _documents = [];


    [RelayCommand]
    private void DocumentTap(DocumentModel tappedItem)
    {
        if (SelectedDocument != null && SelectedDocument != tappedItem)
            SelectedDocument.IsSelected = false;

        if (tappedItem.IsSelected)
        {
            tappedItem.IsSelected = false;
            SelectedDocument = null;
        }
        else
        {
            SelectedDocument = tappedItem;
            tappedItem.IsSelected = true;
            SelectionChanged?.Invoke(this, new EventArgs());
        }
    }

    public void SubscribeToSelectionChanged(EventHandler handler) => SelectionChanged += handler;

    private async Task LoadLocalDocumentsAsync()
    {
        if (_source.Count == 0)
        {
            try
            {
                var jsonString = await File.ReadAllTextAsync(Prefernces.DocumentDataFilePath).ConfigureAwait(false);
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    Converters =
                        {
                            new JsonStringEnumConverter(JsonNamingPolicy.CamelCase),
                            new JsonStringEnumConverter(JsonNamingPolicy.SnakeCaseLower),
                        },
                };
                var docs = JsonSerializer.Deserialize<IEnumerable<DocumentModel>>(jsonString, options);

                foreach (var d in docs ?? [])
                {
                    try
                    {
                        _source.Add(await DocumentModel.Create(d).ConfigureAwait(false));
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error adding document: {d.Title}, error " + ex.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading from JSON: {ex.Message}");
            }
        }

        Documents = new ObservableCollection<DocumentModel>(_source);
    }
}