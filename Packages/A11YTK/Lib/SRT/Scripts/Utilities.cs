using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace A11YTK.SRT
{

    public static class Utilities
    {

        public const int MILLISECOND = 1000;

        public static readonly string[] TIMESTAMP_DELIMITER = { "-->" };

        public static TimeSpan ParseTimeFromTimestamp(string timestamp)
        {
            TimeSpan.TryParse(timestamp.Trim().Replace(',', '.'), CultureInfo.CurrentCulture, out var results);

            return results;
        }

        public static double ParseMillisecondsFromTimestamp(string timestamp)
        {
            return ParseTimeFromTimestamp(timestamp).TotalMilliseconds;
        }

        public static void ParseTimeFromContent(string content, out double startTime, out double endTime)
        {
            var times = content.Split(TIMESTAMP_DELIMITER, StringSplitOptions.None);

            try
            {
                startTime = ParseMillisecondsFromTimestamp(times[0]) / MILLISECOND;
                endTime = ParseMillisecondsFromTimestamp(times[1]) / MILLISECOND;
            }
            catch (Exception error)
            {
                startTime = -1;
                endTime = -1;

                throw new InvalidOperationException($"Non-valid timestamp range. {error.Message}");
            }
        }

        public static List<Subtitle> ParseSubtitlesFromString(string content)
        {
            var subtitles = new List<Subtitle>();

            var sanitizedContent = Regex.Replace(content.Trim(), @"[\n]{3,}", "\n\n");

            var matches = sanitizedContent.Split('\n').ToList().ChunkListWithPatternDelimiter(@"^\s*$");

            foreach (var match in matches)
            {
                var subtitle = new Subtitle();

                int.TryParse(match[0], out subtitle.id);

                ParseTimeFromContent(match[1], out subtitle.startTime, out subtitle.endTime);

                var text = new StringBuilder();

                for (var i = 2; i < match.Count; i += 1)
                {
                    text.Append(match[i]);
                    text.Append(Environment.NewLine);
                }

                subtitle.text = text.ToString().Trim();

                subtitles.Add(subtitle);
            }

            return subtitles;
        }

        public static Subtitle? GetActiveSubtitle(List<Subtitle> subtitles, double time)
        {
            foreach (var subtitle in subtitles)
            {
                if (time > subtitle.startTime && time < subtitle.endTime) return subtitle;
            }

            return null;
        }

    }

}
