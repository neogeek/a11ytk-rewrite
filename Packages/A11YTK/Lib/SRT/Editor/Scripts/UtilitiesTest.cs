using System;
using System.Linq;
using NUnit.Framework;

namespace A11YTK.SRT.Tests
{

    public class UtilitiesTest
    {

        [Test]
        public void ParseTimeFromTimestampTest()
        {
            Assert.That(Utilities.ParseTimeFromTimestamp(Mocks.TIMESTAMP), Is.EqualTo(TimeSpan.FromSeconds(4.2)));
        }

        [Test]
        public void ParseMillisecondsFromTimestampTest()
        {
            Assert.That(Utilities.ParseMillisecondsFromTimestamp(Mocks.TIMESTAMP), Is.EqualTo(4200d));
        }

        [Test]
        public void ParseTimeFromContentTest()
        {
            Utilities.ParseTimeFromContent(Mocks.TIMESTAMP_RANGE, out var startTime, out var endTime);

            Assert.That(startTime, Is.EqualTo(4.200d));
            Assert.That(endTime, Is.EqualTo(8.200d));
        }

        [Test]
        public void ParseSubtitlesFromStringTest()
        {
            var subtitles =
                Utilities.ParseSubtitlesFromString(Mocks.SUBTITLE_CONTENTS);

            Assert.That(subtitles[0].id, Is.EqualTo(1));
            Assert.That(subtitles[0].startTime, Is.EqualTo(4.200d));
            Assert.That(subtitles[0].endTime, Is.EqualTo(8.200d));
            Assert.That(subtitles[0].text, Is.EqualTo("Hello, world."));

            Assert.That(subtitles[3].id, Is.EqualTo(4));
            Assert.That(subtitles[3].startTime, Is.EqualTo(32.200d));
            Assert.That(subtitles[3].endTime, Is.EqualTo(36.200d));
            Assert.That(subtitles[3].text, Is.EqualTo($"If you can see me,{Environment.NewLine}can you wave?"));

            Assert.That(subtitles[4].id, Is.EqualTo(5));
            Assert.That(subtitles[4].startTime, Is.EqualTo(40.200d));
            Assert.That(subtitles[4].endTime, Is.EqualTo(44.200d));
            Assert.That(subtitles[4].text, Is.EqualTo("Fine. If you can't wave, just yell out."));
        }

        [Test]
        public void GetActiveSubtitleTest()
        {
            var subtitles =
                Utilities.ParseSubtitlesFromString(Mocks.SUBTITLE_CONTENTS);

            Assert.That(Utilities.GetActiveSubtitle(subtitles, 0), Is.EqualTo(null));
            Assert.That(Utilities.GetActiveSubtitle(subtitles, 5), Is.EqualTo(subtitles.First()));
        }

        [Test]
        public void ParseTimeFromContentWithExceptionTest()
        {
            Assert.Throws<InvalidOperationException>(
                () =>
                    Utilities.ParseTimeFromContent(Mocks.TIMESTAMP, out var _, out var _)
            );
        }

    }

}
