using NoteApp.Models;
using Plugin.Fingerprint;
using Plugin.Fingerprint.Abstractions;

namespace NoteApp;

public partial class NoteListPage : ContentPage
{
    private bool _isUnlocked = false;

	public NoteListPage()
	{
		InitializeComponent();
	}

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (_isUnlocked)
        {
            await LoadNotes();
        }
        else
        {
            await Task.Delay(100);
            await AuthenticateApp();
        }
    }

    private async Task LoadNotes()
    {
        NotesCollectionView.ItemsSource = await DatabaseService.GetNotes();
    }

    private async Task AuthenticateApp()
    {
        NotesCollectionView.IsVisible = false;
        AddNoteButton.IsVisible = false;

        var isAvailable = await CrossFingerprint.Current.IsAvailableAsync(true);
        if (!isAvailable)
        {
            await DisplayAlert("Not Set Up", "Biometric authentication is not available or not set up on this device.", "OK");
            await CloseApp();
            return;
        }

        var request = new AuthenticationRequestConfiguration("Unlock NoteApp", "Prove it's you to access your notes.");
        
        var result = await CrossFingerprint.Current.AuthenticateAsync(request);

        if (result.Authenticated)
        {
            _isUnlocked = true;
            NotesCollectionView.IsVisible = true;
            AddNoteButton.IsVisible = true;
            await LoadNotes();
        }
        else
        {
            await DisplayAlert("Authentication Failed", "Could not verify your identity. The app will close.", "OK");
            await CloseApp();
        }
    }

    private async Task CloseApp()
    {
        await Task.Delay(100);
        Application.Current?.CloseWindow(GetParentWindow());
    }

    private async void OnAddNoteClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("EditorPage");
    }

    private async void OnNoteTapped(object sender, TappedEventArgs e)
    {
        if (sender is BindableObject bindable && bindable.BindingContext is Note selectedNote)
        {
            await Shell.Current.GoToAsync($"EditorPage?id={selectedNote.Id}");
        }
    }

}
