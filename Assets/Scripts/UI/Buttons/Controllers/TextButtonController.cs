using UnityEngine;
using UI.Settings;
using UI.Views;

namespace UI.Controllers
{
    [RequireComponent(typeof(TextButtonView))]
    public sealed class TextButtonController : ButtonController
    {
        [Header("Controller")]
        [SerializeField] private TextButtonView _view;

        public void Setup(TextButtonSettings settings)
        {
            base.Setup(settings);

            _view.SetText(settings.Title);
            _view.SetButtonColor(settings.Color);
        }
    }
}