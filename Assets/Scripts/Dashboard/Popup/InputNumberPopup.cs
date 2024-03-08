using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace TABApps.TestTask
{
    public class InputNumberPopup : Popup
    {
        public event Action<string> OnInputNumberEntered;

        [SerializeField] private TMP_InputField _inputField;
        [SerializeField] private Button _enterButton;

        public bool AcceptsEmptyField { get; set; }

        public override void Setup(string title)
        {
            base.Setup(title);

            _enterButton.onClick.AddListener(OnEnterButtonClick);
            _inputField.text = "";
        }

        private void OnEnterButtonClick()
        {
            if (string.IsNullOrEmpty(_inputField.text) && !AcceptsEmptyField)
                return;

            OnInputNumberEntered?.Invoke(_inputField.text);
        }

        public override void Dispose()
        {
            base.Dispose();

            _enterButton.onClick.RemoveListener(OnEnterButtonClick);
        }
    }
}