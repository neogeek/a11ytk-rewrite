using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;

namespace A11YTK
{

    public static class CustomExtensions
    {

        public static string WrapText(this TextMeshProUGUI textMesh, string text, int maxCharLength = 20)
        {
            var lines = new List<string> { "" };

            var wrapWidth = textMesh.GetPreferredValues(new string('W', maxCharLength));

            var words = Regex.Split(text, @"(?:\s+)");

            foreach (var word in words)
            {
                var combinedWords = $"{lines[^1]} {word}".Trim();

                var valueSizeDelta = textMesh.GetPreferredValues(combinedWords);

                if (valueSizeDelta.x > wrapWidth.x)
                {
                    lines.Add(word);
                }
                else
                {
                    lines[^1] = combinedWords;
                }
            }

            return string.Join("\n", lines).Trim();
        }

    }

}
