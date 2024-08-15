using System.Collections.Generic;
using System.Collections.ObjectModel;
using A11YTK.SRT;
using TMPro;
using UnityEngine;
using UnityEngine.Video;

namespace A11YTK
{

    public class SubtitleRenderer : MonoBehaviour
    {

        [SerializeField]
        private AudioSource _audioSource;

        [SerializeField]
        private VideoPlayer _videoPlayer;

        [SerializeField]
        private int _maxCharLength = 20;

        [SerializeField]
        private TextMeshProUGUI _subtitleTextComp;

        [SerializeField]
        private RectTransform _subtitleBackground;

        [SerializeField]
        private TextAsset _subtitlesTextAsset;

        private bool _showSubtitles = true;

        private bool _audioSourceAvailable => _audioSource && _audioSource.clip;

        private bool _videoPlayerAvailable => _videoPlayer && _videoPlayer.clip;

        private readonly List<Subtitle> _subtitles = new();

        public ReadOnlyCollection<Subtitle> subtitles => _subtitles.AsReadOnly();

        private void Start()
        {
            if (_subtitlesTextAsset)
            {
                SetSubtitle(_subtitlesTextAsset);
            }
        }

        public void SetSubtitle(string text)
        {
            _subtitles.Clear();
            _subtitles.AddRange(Utilities.ParseSubtitlesFromString(text));
        }

        public void SetSubtitle(TextAsset textAsset)
        {
            _subtitles.Clear();
            _subtitles.AddRange(Utilities.ParseSubtitlesFromString(textAsset.text));
        }

        public void ClearSubtitles()
        {
            _subtitles.Clear();
        }

        private double GetElapsedTime()
        {
            if (_audioSourceAvailable)
            {
                return _audioSource.time;
            }

            if (_videoPlayerAvailable)
            {
                return _videoPlayer.time;
            }

            return 0;
        }

        public bool IsVisible()
        {
            return _showSubtitles;
        }

        public void ToggleVisibility()
        {
            _showSubtitles = !_showSubtitles;
        }

        private void Update()
        {
            if (!_subtitleTextComp || !_audioSourceAvailable && !_videoPlayerAvailable)
            {
                return;
            }

            var activeSubtitle = Utilities.GetActiveSubtitle(_subtitles, GetElapsedTime());

            _subtitleTextComp.text =
                activeSubtitle.HasValue && _showSubtitles
                    ? _subtitleTextComp.WrapText(activeSubtitle.Value.text, _maxCharLength)
                    : "";

            if (!_subtitleBackground)
            {
                return;
            }

            _subtitleTextComp.rectTransform.sizeDelta = _subtitleTextComp.GetPreferredValues(_subtitleTextComp.text);

            _subtitleBackground.sizeDelta = _subtitleTextComp.rectTransform.sizeDelta;
            _subtitleBackground.anchoredPosition = _subtitleTextComp.rectTransform.anchoredPosition;
        }

    }

}
