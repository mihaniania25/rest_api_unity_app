using System;
using UnityEngine;

namespace TABApps.TestTask
{
    public class PopupManager : MonoBehaviour
    {
        public event Action OnPopupClosed;
        public event Action<string> OnInputNumberEntered;

        [SerializeField] private InputNumberPopup _inputNumberPopup;
        [SerializeField] private GameObject _substrate;

        public void Setup()
        {
            _substrate.SetActive(false);
            _inputNumberPopup.gameObject.SetActive(false);

            _inputNumberPopup.OnInputNumberEntered += OnPopupNumberEntered;
            _inputNumberPopup.OnCloseRequested += OnPopupCloseRequested;
        }

        public void ShowInputNumberPopup(string title, bool emptyAccepted)
        {
            _inputNumberPopup.Setup(title);
            _inputNumberPopup.AcceptsEmptyField = emptyAccepted;

            _inputNumberPopup.gameObject.SetActive(true);
            _substrate.SetActive(true);
        }

        private void OnPopupNumberEntered(string numberString)
        {
            OnInputNumberEntered?.Invoke(numberString);
            ClosePopup(_inputNumberPopup);
        }

        private void OnPopupCloseRequested()
        {
            ClosePopup(_inputNumberPopup);
        }

        private void ClosePopup(Popup popup)
        {
            popup.Dispose();
            popup.gameObject.SetActive(false);
            _substrate.SetActive(false);

            OnPopupClosed?.Invoke();
        }

        public void Dispose()
        {
            _inputNumberPopup.OnInputNumberEntered -= OnPopupNumberEntered;
            _inputNumberPopup.OnCloseRequested -= OnPopupCloseRequested;
        }
    }
}