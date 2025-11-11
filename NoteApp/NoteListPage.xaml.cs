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
            // Wait for the page to be fully visible, then try to auth
            await Task.Delay(100); // <-- THIS HELPS PREVENT A CRASH
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

        // --- NEW SAFETY CHECK ---
        var isAvailable = await CrossFingerprint.Current.IsAvailableAsync(true);
        if (!isAvailable)
        {
            // This means the phone has no scanner OR no fingerprints enrolled
            await DisplayAlert("Not Set Up", "Biometric authentication is not available or not set up on this device.", "OK");
            
            // We can just unlock the app here, or close it. 
            // For now, let's just close it.
            await CloseApp();
            return;
        }
        // --- END OF NEW CHECK ---

        var request = new AuthenticationRequestConfiguration("Unlock NoteApp", "Prove it's you to access your notes.");
        
        var result = await CrossFingerprint.Current.AuthenticateAsync(request);

        if (result.Authenticated)
        {
            // --- SUCCESS ---
            _isUnlocked = true;
            NotesCollectionView.IsVisible = true;
            AddNoteButton.IsVisible = true;
            await LoadNotes();
        }
        else
        {
            // --- FAILURE ---
            await DisplayAlert("Authentication Failed", "Could not verify your identity. The app will close.", "OK");
            await CloseApp();
        }
    }

    // --- NEW, SIMPLER WAY TO CLOSE THE APP ---
    private async Task CloseApp()
    {
        await Task.Delay(100); // Give the alert time to dismiss
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