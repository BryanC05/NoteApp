# 🔒 NoteApp

A secure, multi-note Markdown editor built with .NET MAUI. This app allows you to create, edit, and manage multiple notes, all protected behind a biometric (fingerprint/face) app lock.

## ✨ Features

  * **Biometric App Lock:** Uses the phone's built-in fingerprint or face scanner to secure the app. (`Plugin.Fingerprint`)
  * **Multi-Note Database:** Store and manage an unlimited number of notes.
  * **Full CRUD:**
      * **Create:** Add new notes.
      * **Read:** View a list of all saved notes.
      * **Update:** Edit existing notes.
      * **Delete:** Remove notes with a confirmation prompt.
  * **Markdown Editor:** A split-screen editor where you can type in Markdown on the top and see a live HTML preview on the bottom.
  * **Live Preview:** Uses the `Markdig` library to instantly convert Markdown to HTML.
  * **Light/Dark Mode:** The UI automatically adapts to your phone's system theme, including the note list and text.

## 🛠️ Tech Stack

  * **.NET MAUI:** The core framework used for the cross-platform UI and logic (C\# & XAML).
  * **SQLite:** A local database (`sqlite-net-pcl`) is used to store all the notes.
  * **Plugin.Fingerprint:** The NuGet package used to handle biometric authentication.
  * **Markdig:** The NuGet package used for fast and powerful Markdown-to-HTML conversion.

## 🚀 How to Run

### Prerequisites

1.  **[.NET 8 SDK](https://dotnet.microsoft.com/download)** (or newer)
2.  **.NET MAUI Workload:** Install by running `dotnet workload install maui`.
3.  **Android Studio:** Required to install the Android SDK, emulators, and accept SDK licenses.

### Project Setup

1.  **Clone the Repository:**
    ```bash
    git clone [Your-GitHub-Repo-URL]
    cd NoteApp/NoteApp
    ```
2.  **Add Android Permissions:**
    Open `Platforms/Android/AndroidManifest.xml` and add this line inside the `<manifest>` tag:
    ```xml
    <uses-permission android:name="android.permission.USE_BIOMETRIC" />
    ```
3.  **Restore Dependencies:**
    Run `dotnet restore` to download all the required packages (SQLite, Markdig, Fingerprint, etc.).

### Running the App

1.  **Connect Your Phone:** Plug in your physical Android phone (with **USB Debugging** enabled).
2.  **Run the Build Command:**
    Use this command to build for your phone's `arm64` architecture:
    ```bash
    dotnet build -t:Run -f net8.0-android -r android-arm64
    ```

## 📂 Project Structure

```
NoteApp/
├── Models/
│   └── Note.cs           # The C# class for a Note object
├── Platforms/
│   └── Android/
│       ├── MainActivity.cs     # Initializes the fingerprint plugin
│       └── AndroidManifest.xml # Contains the biometric permission
├── Resources/
│   └── Styles/
│       ├── Colors.xaml   # Contains Light/Dark mode colors
│       └── Styles.xaml   # App-wide styles
├── DatabaseService.cs    # Static class to handle all SQLite actions
├── EditorPage.xaml       # The Markdown editor and preview screen
├── NoteListPage.xaml     # The main list of notes (with app lock logic)
├── AppShell.xaml         # App navigation
├── GlobalUsings.cs       # Manages global 'using' statements
└── MauiProgram.cs        # Registers the fingerprint service
```
