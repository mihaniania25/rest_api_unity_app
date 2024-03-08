using System;
using System.Collections.Generic;

namespace TABApps.TestTask
{
    public class AppModel
    {
        public event Action OnDataUpdated;
        public event Action<ButtonData> OnSingleDataUpdated;

        private PropagationField<string> _errorMessage = new PropagationField<string>();
        public PropagationField<string> ErrorMessage => _errorMessage;

        public List<ButtonData> ButtonsData { get; private set; }

        public void SetButtonsData(List<ButtonData> buttonsData)
        {
            ButtonsData = buttonsData;
            OnDataUpdated?.Invoke();
        }

        public void UpdateButtonData(ButtonData buttonData)
        {
            ButtonData dataToUpdate = ButtonsData.Find(bd => bd.id == buttonData.id);

            dataToUpdate?.Update(buttonData);

            OnSingleDataUpdated?.Invoke(buttonData);
        }

        public void RemoveButtonData(string id)
        {
            ButtonData dataToRemove = ButtonsData.Find(bd => bd.id == id);

            if (dataToRemove != null)
            {
                ButtonsData.Remove(dataToRemove);
                OnSingleDataUpdated?.Invoke(dataToRemove);
            }
        }
    }
}