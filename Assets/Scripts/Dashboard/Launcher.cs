using UnityEngine;

namespace TABApps.TestTask
{
    public class Launcher : MonoBehaviour
    {
        [SerializeField] private DashboardView _dashboardView;

        private DashboardManager _dashboardManager;

        private void Start()
        {
            _dashboardManager = new DashboardManager();
            _dashboardManager.Setup(_dashboardView);
        }

        private void OnDestroy()
        {
            _dashboardManager.Dispose();
            _dashboardManager = null;
        }
    }
}