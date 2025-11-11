using Markdig;
using NoteApp.Models; // We need our Note model

namespace NoteApp;

// This attribute lets us receive the "id" from the navigation
[QueryProperty(nameof(NoteId), "id")]
public partial class EditorPage : ContentPage
{
    private static readonly MarkdownPipeline _pipeline = new MarkdownPipelineBuilder()
                                                            .UseAdvancedExtensions()
                                                            .Build();

    // This will hold the note we are editing
    private Note _currentNote = null!;

    // This property will be set by the navigation
    public int NoteId { get; set; }

	public EditorPage()
	{
		InitializeComponent();
	}

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadNoteAsync();
    }

    private async Task LoadNoteAsync()
    {
        if (NoteId == 0)
        {
            // This is a new note
            _currentNote = new Note();
        }
        else
        {
            // This is an existing note
            _currentNote = await DatabaseService.GetNote(NoteId);
        }

        // Fill the UI with the note's data
        TitleEntry.Text = _currentNote.Title;
        MarkdownEditor.Text = _currentNote.MarkdownText;
    }

    private void OnMarkdownEditorTextChanged(object sender, TextChangedEventArgs e)
    {
        string markdownText = e.NewTextValue ?? string.Empty;
        string htmlText = Markdown.ToHtml(markdownText, _pipeline);

        // (Your HTML preview code is unchanged)
        string fullHtml = $@"
            <html>
            <head>
                <meta name='viewport' content='width=device-width, initial-scale-1.0'>
                <style>
                    body {{ font-family: -apple-system, sans-serif; padding: 10px; }}
                    h1 {{ color: #007AFF; }}
                    code {{ background-color: #f0f0f0; padding: 2px; border-radius: 4px; }}
                </style>
            </head>
            <body>
                {htmlText}
            </body>
            </html>";

        HtmlPreview.Source = new HtmlWebViewSource { Html = fullHtml };
    }

    private async void OnSaveButtonClicked(object sender, EventArgs e)
    {
        // Update the note object with the UI data
        _currentNote.Title = TitleEntry.Text;
        _currentNote.MarkdownText = MarkdownEditor.Text;
        _currentNote.Date = DateTime.Now;

        // Save to the database
        await DatabaseService.SaveNote(_currentNote);

        // Go back to the note list
        await Shell.Current.GoToAsync("..");
    }

    private async void OnDeleteButtonClicked(object sender, EventArgs e)
    {
        if (_currentNote.Id == 0)
        {
            // Note hasn't been saved yet, just go back
            await Shell.Current.GoToAsync("..");
            return;
        }

        // Ask for confirmation
        bool confirm = await DisplayAlert("Delete Note", $"Are you sure you want to delete '{_currentNote.Title}'?", "Yes", "No");
        if (confirm)
        {
            await DatabaseService.DeleteNote(_currentNote);
            await Shell.Current.GoToAsync("..");
        }
    }
}