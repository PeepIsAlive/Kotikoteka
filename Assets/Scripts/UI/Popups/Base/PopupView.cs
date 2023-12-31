using Application = DWS.Application;
using System.Collections.Generic;
using UnityEngine.UI;
using UI.Controllers;
using UnityEngine;
using DG.Tweening;
using UI.Settings;
using System.Linq;
using Settings;
using TMPro;

namespace UI
{
    public class PopupView<T> : PopupViewBase where T : Popup
    {
        [Header("Base")]
        [SerializeField] private RectTransform _rootRect;
        [SerializeField] private Button _overlayButton;
        [SerializeField] private TMP_Text _titleLabel;
        [SerializeField] private Image _headerImage;

        [Header("Blocks")]
        [SerializeField] private RectTransform _topParent;
        [SerializeField] private RectTransform _middleParent;
        [SerializeField] private RectTransform _buttonParent;

        private readonly float _durationTween = 0.3f;
        private bool _ignoreOverlayButtonAction;
        private Vector3 _direction;

        public virtual void Setup(T settings)
        {
            _ignoreOverlayButtonAction = settings.IgnoreOverlayButtonAction;
            _direction = settings.Direction;

            InitializeButtons(settings.ButtonSettings);
            SetTitleText(settings.Title, settings.Color);
        }

        public override void Show()
        {
            base.Show();

            AddListeners();
            DoShow();
        }

        public override void Hide()
        {
            base.Hide();

            RemoveListeners();
            DoHide();
        }

        private void SetTitleText(string text, Color? color = null)
        {
            if (_titleLabel == null || _headerImage == null)
                return;

            _titleLabel.text = text;
            _headerImage.color = (Color)color;
        }

        private void InitializeButtons<B>(List<B> buttonSettings) where B : ButtonSettings
        {
            if (buttonSettings == null)
                return;

            var prefabsSet = SettingsProvider.Get<PrefabsSet>();

            foreach (var setting in buttonSettings)
            {
                if (setting is TextButtonSettings textButtonSettings)
                {
                    var prefab = prefabsSet.Buttons.First(x => x.GetComponent<TextButtonController>() != null)
                        .GetComponent<TextButtonController>();

                    Instantiate(prefab, _buttonParent, false)
                        .Setup(textButtonSettings);
                }
            }
        }

        private void DoShow()
        {
            _rootRect ??= gameObject.GetComponent<RectTransform>();

            var startOffset = GetDirection(_direction);
            var targetPosition = _rootRect.localPosition;

            if (Mathf.Abs(startOffset.sqrMagnitude) - Mathf.Abs(Vector2.zero.sqrMagnitude) > Mathf.Epsilon)
            {
                _rootRect.localPosition += startOffset;
                _rootRect.DOAnchorPos(targetPosition, _durationTween)
                    .SetEase(Ease.OutBack)
                    .OnComplete(() => _overlayButton.gameObject.SetActive(true));
            }
        }

        private void DoHide()
        {
            var targetPosition = GetDirection(_direction);

            if (Mathf.Abs(targetPosition.sqrMagnitude) - Mathf.Abs(Vector2.zero.sqrMagnitude) > Mathf.Epsilon)
            {
                _overlayButton.gameObject.SetActive(false);
                _rootRect.DOAnchorPos(targetPosition, _durationTween)
                    .SetEase(Ease.InBack)
                    .OnComplete(() => Destroy(gameObject));
            }
        }

        private Vector3 GetDirection(Vector3 direction)
        {
            var result = Vector3.down.normalized * Application.MainCanvasRect.sizeDelta.y;

            if (direction == Vector3.left || direction == Vector3.right)
                result = direction.normalized * Application.MainCanvasRect.sizeDelta.x;

            return result;
        }

        private void AddListeners()
        {
            if (_ignoreOverlayButtonAction)
                return;

            _overlayButton?.onClick.AddListener(Hide);
        }

        private void RemoveListeners()
        {
            _overlayButton?.onClick?.RemoveListener(Hide);
        }
    }
}
