using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace TABApps.TestTask
{
    public abstract class Popup : MonoBehaviour
    {
        public event Action OnCloseRequested;

        [SerializeField] private Button _closeButton;
        [SerializeField] private TextMeshProUGUI _title;

        public virtual void Setup(string titleText)
        {
            _title.text = titleText;
            _closeButton.onClick.AddListener(OnCloseButtonClick);
        }

        private void OnCloseButtonClick()
        {
            OnCloseRequested?.Invoke();
        }

        public virtual void Dispose()
        {
            _closeButton.onClick.RemoveListener(OnCloseButtonClick);
        }
    }
}