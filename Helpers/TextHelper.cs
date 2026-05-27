using System.Text.RegularExpressions;

namespace PlaywrightAssignment.Helpers;

public static class TextHelper
{
    public static int CountUniqueWords(string text) =>
        text.Split(new[] { ' ', '\n', '\r', '\t' }, StringSplitOptions.RemoveEmptyEntries)
            .Select(w => Regex.Replace(w, @"[^a-zA-Z0-9]", ""))
            .Where(w => w.Length > 0)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .Count();
}
