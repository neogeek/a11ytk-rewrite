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
        private TextMeshProUGUI _subtitleTextComp;

        [SerializeField]
        private RectTransform _subtitleBackground;

        [SerializeField]
        private TextAsset _subtitlesTextAsset;

        private bool _showSubtitles = true;

        private bool _audioClipAvailable => _audioSource && _audioSource.clip;

        private bool _videoClipAvailable => _videoPlayer && _videoPlayer.clip;

        private readonly List<Subtitle> _subtitles = new();

        public ReadOnlyCollection<Subtitle> subtitles => _subtitles.AsReadOnly();

        private void Start()
        {
            if (_subtitlesTextAsset)
            {
                SetSubtitle(_subtitlesTextAsset);
            }

            UpdateSubtitle(string.Empty);
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

            UpdateSubtitle(string.Empty);
        }

        private double GetElapsedTime()
        {
            if (_audioClipAvailable)
            {
                return _audioSource.time;
            }

            if (_videoClipAvailable)
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
            if (!_subtitleTextComp || (!_audioClipAvailable && !_videoClipAvailable))
            {
                return;
            }

            var activeSubtitle = Utilities.GetActiveSubtitle(_subtitles, GetElapsedTime());

            UpdateSubtitle(activeSubtitle.HasValue && _showSubtitles
                ? activeSubtitle.Value.text
                : "");
        }

        public void UpdateSubtitle(string subtitleText)
        {
            if (_subtitleTextComp.text == subtitleText)
            {
                return;
            }

            _subtitleTextComp.text = subtitleText;

            if (!_subtitleBackground)
            {
                return;
            }

            _subtitleTextComp.ForceMeshUpdate();

            _subtitleTextComp.verticalAlignment = VerticalAlignmentOptions.Geometry;
            _subtitleTextComp.enableWordWrapping = true;
            _subtitleTextComp.overflowMode = TextOverflowModes.Overflow;

            _subtitleBackground.sizeDelta = _subtitleTextComp.textBounds.size;
            _subtitleBackground.pivot = Vector2.one;
            _subtitleBackground.anchoredPosition = _subtitleTextComp.textBounds.extents;
        }

    }

}
