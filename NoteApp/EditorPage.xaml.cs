using Markdig;
using NoteApp.Models;

namespace NoteApp;

[QueryProperty(nameof(NoteId), "id")]
public partial class EditorPage : ContentPage
{
    private static readonly MarkdownPipeline _pipeline = new MarkdownPipelineBuilder()
                                                            .UseAdvancedExtensions()
                                                            .Build();

    private Note _currentNote = null!;

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
            _currentNote = new Note();
        }
        else
        {
            _currentNote = await DatabaseService.GetNote(NoteId);
        }

        TitleEntry.Text = _currentNote.Title;
        MarkdownEditor.Text = _currentNote.MarkdownText;
    }

    private void OnMarkdownEditorTextChanged(object sender, TextChangedEventArgs e)
    {
        string markdownText = e.NewTextValue ?? string.Empty;
        string htmlText = Markdown.ToHtml(markdownText, _pipeline);

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
        _currentNote.Title = TitleEntry.Text;
        _currentNote.MarkdownText = MarkdownEditor.Text;
        _currentNote.Date = DateTime.Now;

        await DatabaseService.SaveNote(_currentNote);

        await Shell.Current.GoToAsync("..");
    }

    private async void OnDeleteButtonClicked(object sender, EventArgs e)
    {
        if (_currentNote.Id == 0)
        {
            await Shell.Current.GoToAsync("..");
            return;
        }

        bool confirm = await DisplayAlert("Delete Note", $"Are you sure you want to delete '{_currentNote.Title}'?", "Yes", "No");
        if (confirm)
        {
            await DatabaseService.DeleteNote(_currentNote);
            await Shell.Current.GoToAsync("..");
        }
    }

}
