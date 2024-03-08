using UnityEngine;
using TMPro;

namespace TABApps.TestTask
{
    public class ErrorPrinter : MonoBehaviour
    {
        [SerializeField] private GameObject _errorView;
        [SerializeField] private TextMeshProUGUI _errorLabel;

        private AppModel _model => AppContext.Model;

        private void Awake()
        {
            _model.ErrorMessage.Subscribe(OnErrorMessageUpd);
        }

        private void OnErrorMessageUpd(string errorMessage)
        {
            _errorView.SetActive(string.IsNullOrEmpty(errorMessage) == false);
            _errorLabel.text = $"Error: {errorMessage}";
        }

        private void OnDestroy()
        {
            _model.ErrorMessage.Unsubscribe(OnErrorMessageUpd);
        }
    }
}