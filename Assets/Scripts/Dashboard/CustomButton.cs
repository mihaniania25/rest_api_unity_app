using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TABApps.TestTask
{
    public class CustomButton : MonoBehaviour
    {
        private const string APPEAR_ANIMPARAM = "Appear";
        private const string DISAPPEAR_ANIMPARAM = "Disappear";

        public string ID => _buttonData.id;

        private ButtonData _buttonData;

        [SerializeField] private Button _button;
        [SerializeField] private TextMeshProUGUI _label;
        [SerializeField] private Image _bg;
        [SerializeField] private Animator _animator;

        public void UpdateData(ButtonData buttonData)
        {
            _buttonData = buttonData;

            UpdateTextAndColor();
        }

        private void UpdateTextAndColor()
        {
            _label.text = $"{ID}: {_buttonData.text}";

            Color newColor;

            if (ColorUtility.TryParseHtmlString(_buttonData.color, out newColor))
                _bg.color = newColor;
        }

        public void Appear()
        {
            if (_buttonData.appearAnimEnabled)
                _animator.SetTrigger(APPEAR_ANIMPARAM);
        }

        public void Disappear()
        {
            if (_buttonData.disappearAnimEnabled)
                _animator.SetTrigger(DISAPPEAR_ANIMPARAM);
            else
                Destroy(gameObject);
        }

        public void OnDisappearAnimCompleted()
        {
            Destroy(gameObject);
        }
    }
}