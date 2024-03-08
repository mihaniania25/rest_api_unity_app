using UnityEngine;

namespace TABApps.TestTask
{
    public class OperationWaitingViewer : MonoBehaviour
    {
        [SerializeField] private GameObject _waitingViewObject;

        public void SetWaitingViewActive(bool isActive)
        {
            _waitingViewObject.SetActive(isActive);
        }
    }
}