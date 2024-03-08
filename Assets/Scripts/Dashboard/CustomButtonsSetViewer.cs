using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace TABApps.TestTask
{
    public class CustomButtonsSetViewer : MonoBehaviour
    {
        [SerializeField] private GameObject _sample;
        [SerializeField] private Transform _container;
        [SerializeField] private ScrollRect _scrollRect;

        private Dictionary<string, CustomButton> _customButtons = new Dictionary<string, CustomButton>();
        private List<ButtonData> _lastHandledCustomButtons = new List<ButtonData>();

        private AppModel _model => AppContext.Model;

        public void Setup()
        {
            _model.OnDataUpdated += OnButtonsDataUpdated;
            _model.OnSingleDataUpdated += OnSingleDataUpdate;
        }

        private void OnButtonsDataUpdated()
        {
            float scrollValue = _scrollRect.verticalNormalizedPosition;

            RemoveOldButtons();
            AddNewButtons();
            UpdateRemainingButtons();

            _scrollRect.verticalNormalizedPosition = scrollValue;

            _lastHandledCustomButtons = new List<ButtonData>(_model.ButtonsData);
        }

        private void OnSingleDataUpdate(ButtonData buttonData)
        {
            bool needToRemove = _model.ButtonsData.Any(bd => bd.id == buttonData.id) == false;

            if (needToRemove)
                RemoveButton(buttonData);
            else
            {
                CustomButton customButton = _customButtons[buttonData.id];
                customButton.UpdateData(buttonData);
            }
        }

        private void RemoveOldButtons()
        {
            List<ButtonData> oldButtons = GetOldButtons();

            foreach (ButtonData oldButtonData in oldButtons)
                RemoveButton(oldButtonData);
        }

        private void RemoveButton(ButtonData oldButtonData)
        {
            CustomButton oldButton = _customButtons[oldButtonData.id];
            oldButton.Disappear();

            _customButtons.Remove(oldButtonData.id);
            _lastHandledCustomButtons.Remove(oldButtonData);
        }

        private List<ButtonData> GetOldButtons()
        {
            List<ButtonData> oldButtons = new List<ButtonData>();

            foreach (ButtonData viewedData in _lastHandledCustomButtons)
            {
                ButtonData existingButtonData = _model.ButtonsData.Find(bd => bd.id == viewedData.id);

                if (existingButtonData == null)
                    oldButtons.Add(viewedData);
            }

            return oldButtons;
        }

        private void AddNewButtons()
        {
            List<ButtonData> newButtons = GetNewButtons();

            foreach (ButtonData newButtonData in newButtons)
            {
                GameObject buttonGO = Instantiate(_sample, _container);
                buttonGO.SetActive(true);

                CustomButton buttonView = buttonGO.GetComponent<CustomButton>();
                buttonView.UpdateData(newButtonData);
                buttonView.Appear();

                _customButtons[newButtonData.id] = buttonView;
            }
        }

        private List<ButtonData> GetNewButtons()
        {
            List<ButtonData> newButtons = new List<ButtonData>();

            foreach (ButtonData buttonData in _model.ButtonsData)
            {
                ButtonData viewedButtonData = _lastHandledCustomButtons.Find(bd => bd.id == buttonData.id);

                if (viewedButtonData == null)
                    newButtons.Add(buttonData);
            }

            return newButtons;
        }

        private void UpdateRemainingButtons()
        {
            foreach (ButtonData idleButtonData in _lastHandledCustomButtons)
            {
                if (_customButtons.ContainsKey(idleButtonData.id))
                {
                    CustomButton idleButton = _customButtons[idleButtonData.id];
                    idleButton.UpdateData(idleButtonData);
                }
            }
        }

        public void Dispose()
        {
            _model.OnDataUpdated -= OnButtonsDataUpdated;
            _model.OnSingleDataUpdated -= OnSingleDataUpdate;
        }
    }
}