using System;
using System.Globalization;
using Microsoft.Maui.Controls;
using NoteApp.Models;

namespace NoteApp.Converters
{
    public class TitleOrTextConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is Note note)
            {
                if (!string.IsNullOrWhiteSpace(note.Title))
                    return note.Title;

                if (!string.IsNullOrWhiteSpace(note.MarkdownText))
                {
                    // Return first non-empty line of MarkdownText (trim and limit length)
                    var lines = note.MarkdownText.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var line in lines)
                    {
                        var trimmed = line.Trim();
                        if (!string.IsNullOrEmpty(trimmed))
                        {
                            // Truncate if too long
                            const int max = 60;
                            return trimmed.Length <= max ? trimmed : trimmed.Substring(0, max - 1) + "…";
                        }
                    }
                }

                return "Untitled Note";
            }

            // Fallback: if someone bound a string directly
            if (value is string s)
            {
                return string.IsNullOrWhiteSpace(s) ? "Untitled Note" : s;
            }

            return string.Empty;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
