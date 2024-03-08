using System;
using UnityEngine;
using UnityEngine.UI;

namespace TABApps.TestTask
{
    public class DashboardView : MonoBehaviour
    {
        public event Action OnCreateButtonClick;
        public event Action OnDeleteButtonClick;
        public event Action OnUpdateButtonClick;
        public event Action OnRefreshButtonClick;

        [Header("Control buttons")]
        [SerializeField] private Button _createButton;
        [SerializeField] private Button _deleteButton;
        [SerializeField] private Button _updateButton;
        [SerializeField] private Button _refreshButton;

        [Space]
        [SerializeField] private CustomButtonsSetViewer _buttonsSetViewer;
        [SerializeField] private PopupManager _popupManager;
        [SerializeField] private OperationWaitingViewer _waitingViewer;

        public PopupManager PopupManager => _popupManager;
        public OperationWaitingViewer WaitingViewer => _waitingViewer;

        public void Setup()
        {
            _createButton.onClick.AddListener(CreateButtonClickListener);
            _deleteButton.onClick.AddListener(DeleteButtonClickListener);
            _updateButton.onClick.AddListener(UpdateButtonClickListener);
            _refreshButton.onClick.AddListener(RefreshButtonClickListener);

            _buttonsSetViewer.Setup();
            _popupManager.Setup();
        }

        private void CreateButtonClickListener()
        {
            OnCreateButtonClick?.Invoke();
        }

        private void DeleteButtonClickListener()
        {
            OnDeleteButtonClick?.Invoke();
        }

        private void UpdateButtonClickListener()
        {
            OnUpdateButtonClick?.Invoke();
        }

        private void RefreshButtonClickListener()
        {
            OnRefreshButtonClick?.Invoke();
        }

        public void Dispose()
        {
            _createButton.onClick.RemoveListener(CreateButtonClickListener);
            _deleteButton.onClick.RemoveListener(DeleteButtonClickListener);
            _updateButton.onClick.RemoveListener(UpdateButtonClickListener);
            _refreshButton.onClick.RemoveListener(RefreshButtonClickListener);

            _buttonsSetViewer.Dispose();
            _popupManager.Dispose();
        }
    }
}