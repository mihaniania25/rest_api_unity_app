using UnityEngine;

namespace TABApps.TestTask
{
    public class DataInteractor
    {
        private long NOT_FOUND_ERR_CODE = 404;

        private PropagationField<bool> _isBusy = new PropagationField<bool>(false);
        public PropagationField<bool> IsBusy => _isBusy;

        private IWebRequestor _webRequestor;

        private AppConfigs _configs => AppContext.Configs;
        private AppModel _model => AppContext.Model;

        public void Setup()
        {
            SetupWebRequestor();
        }

        private void SetupWebRequestor()
        {
            EndpointInteractionConfig endpointInteractionConfig = _configs.EndpointInteraction;
            ApiEndpointData apiEndpointData = endpointInteractionConfig.ApiEndpointData;

            WebRequestorGetter webRequestorGetter = new WebRequestorGetter();
            _webRequestor = webRequestorGetter.GetWebRequestor();
            _webRequestor.Setup(apiEndpointData);
        }

        public void TryCreateButton()
        {
            if (_isBusy.Value == true)
                return;

            _isBusy.Value = true;
            _webRequestor.Post(HandleDefaultWebRequest);
        }

        public void TryDeleteButton(string id)
        {
            if (_isBusy.Value == true)
                return;

            _isBusy.Value = true;
            _webRequestor.Delete(id, HandleDefaultWebRequest);
        }

        public void TryUpdateButton(string id, string bodyPart)
        {
            if (_isBusy.Value == true)
                return;

            _isBusy.Value = true;
            _webRequestor.Put(id, bodyPart, HandleDefaultWebRequest);
        }

        public void RefreshButton(string id)
        {
            if (_isBusy.Value == true)
                return;

            _isBusy.Value = true;
            _webRequestor.Get(id, HandleRefreshSingleButtonRequest);
        }

        public void RefreshAllButtons()
        {
            if (_isBusy.Value == true)
                return;

            _isBusy.Value = true;
            _webRequestor.Get(HandleRefreshButtonsRequest);
        }

        private void HandleDefaultWebRequest(WebRequestResult requestResult)
        {
            _model.ErrorMessage.Value = requestResult.Succeed ? "" : requestResult.Message;
            _isBusy.Value = false;
        }

        private void HandleRefreshSingleButtonRequest(WebRequestResult requestResult)
        {
            bool succeed = requestResult.Succeed || requestResult.ErrorCode == NOT_FOUND_ERR_CODE;

            _model.ErrorMessage.Value = succeed ? "" : requestResult.Message;

            if (succeed == false)
                return;

            if (requestResult.ErrorCode == NOT_FOUND_ERR_CODE)
                _model.RemoveButtonData(requestResult.ItemID);
            else
            {
                try
                {
                    ButtonData buttonData = JsonUtility.FromJson<ButtonData>(requestResult.Message);
                    _model.UpdateButtonData(buttonData);
                }
                catch
                {
                    _model.ErrorMessage.Value = "Failed to parse data from server.";
                }
            }

            _isBusy.Value = false;
        }

        private void HandleRefreshButtonsRequest(WebRequestResult requestResult)
        {
            _model.ErrorMessage.Value = requestResult.Succeed ? "" : requestResult.Message;

            if (requestResult.Succeed == false)
                return;

            try
            {
                ButtonsDataSet buttonsDataSet = JsonUtility.FromJson<ButtonsDataSet>("{\"ButtonsData\":" + requestResult.Message + "}");
                _model.SetButtonsData(buttonsDataSet.ButtonsData);
            }
            catch
            {
                _model.ErrorMessage.Value = "Failed to parse data from server.";
            }

            _isBusy.Value = false;
        }
    }
}