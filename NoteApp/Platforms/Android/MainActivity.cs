using Android.App;
using Android.Content.PM;
using Android.OS;
using Plugin.Fingerprint; // This line is still correct

namespace NoteApp;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
    // THIS IS THE NEW, CORRECT OnCreate METHOD
    protected override void OnCreate(Bundle savedInstanceState)
    {
        base.OnCreate(savedInstanceState);

        // This is the correct way to initialize the plugin for MAUI
        CrossFingerprint.SetCurrentActivityResolver(() => this);
    }
}