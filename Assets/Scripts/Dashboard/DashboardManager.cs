using System;
using UnityEngine;

namespace TABApps.TestTask
{
    public class DashboardManager
    {
        private DashboardView _dashboardView;
        private PopupManager _popupManager;
        private DataInteractor _dataInteractor = new DataInteractor();
        private Action<string> _enteredIdHandler;

        private AppModel _model => AppContext.Model;

        public void Setup(DashboardView dashboardView)
        {
            _dashboardView = dashboardView;
            _popupManager = _dashboardView.PopupManager;

            SetupDataInteractor();
            SetupDashboardView();

            RequestButtonsData();
        }

        private void SetupDataInteractor()
        {
            _dataInteractor = new DataInteractor();
            _dataInteractor.Setup();

            _dataInteractor.IsBusy.Subscribe(OnDataInteractionActivityUpd);
        }

        private void OnDataInteractionActivityUpd(bool isDataInteractionActive)
        {
            _dashboardView.WaitingViewer.SetWaitingViewActive(isDataInteractionActive);
        }

        private void SetupDashboardView()
        {
            _dashboardView.Setup();

            _dashboardView.OnRefreshButtonClick += TryRefreshButtons;
            _dashboardView.OnCreateButtonClick += TryCreateButton;
            _dashboardView.OnDeleteButtonClick += TryDeleteButton;
            _dashboardView.OnUpdateButtonClick += TryUpdateButton;
        }

        private void TryRefreshButtons()
        {
            TryPerformIdDependendRequest(title: "Button refreshing", emptyAllowed: true, handler: RequestButtonsData);
        }

        private void RequestButtonsData(string id = null)
        {
            if (string.IsNullOrEmpty(id))
                _dataInteractor.RefreshAllButtons();
            else
                _dataInteractor.RefreshButton(id);
        }

        private void TryCreateButton()
        {
            _dataInteractor.TryCreateButton();
        }

        private void TryDeleteButton()
        {
            TryPerformIdDependendRequest(title: "Button deleting", emptyAllowed: false, handler: RequestDeleteButtonData);
        }

        private void RequestDeleteButtonData(string buttonID)
        {
            _dataInteractor.TryDeleteButton(buttonID);
        }

        private void TryUpdateButton()
        {
            TryPerformIdDependendRequest(title: "Button updating", emptyAllowed: false, handler: RequestUpdateButtonData);
        }

        private void RequestUpdateButtonData(string buttonID)
        {
            ButtonData buttonData = _model.ButtonsData.Find(bd => bd.id == buttonID);

            if (buttonData != null)
            {
                ButtonData newData = buttonData.Clone();
                newData.RandomizeColor();

                _dataInteractor.TryUpdateButton(buttonID, JsonUtility.ToJson(newData));
            }
        }

        private void TryPerformIdDependendRequest(string title, bool emptyAllowed, Action<string> handler)
        {
            _enteredIdHandler = handler;

            _popupManager.OnPopupClosed += UnsubscribeIdPopup;
            _popupManager.OnInputNumberEntered += OnIdentifierEntered;

            _popupManager.ShowInputNumberPopup(title, emptyAllowed);
        }

        private void OnIdentifierEntered(string id)
        {
            _enteredIdHandler?.Invoke(id);
            UnsubscribeIdPopup();
        }

        private void UnsubscribeIdPopup()
        {
            _popupManager.OnPopupClosed -= UnsubscribeIdPopup;
            _popupManager.OnInputNumberEntered -= OnIdentifierEntered;
        }

        public void Dispose()
        {
            _dataInteractor.IsBusy.Unsubscribe(OnDataInteractionActivityUpd);
            _dataInteractor = null;

            _popupManager.OnPopupClosed -= UnsubscribeIdPopup;
            _popupManager.OnInputNumberEntered -= OnIdentifierEntered;
            _popupManager = null;

            _dashboardView.OnRefreshButtonClick -= TryRefreshButtons;
            _dashboardView.OnCreateButtonClick -= TryCreateButton;
            _dashboardView.OnDeleteButtonClick -= TryDeleteButton;
            _dashboardView.OnUpdateButtonClick -= TryUpdateButton;

            _dashboardView.Dispose();
            _dashboardView = null;
        }
    }
}