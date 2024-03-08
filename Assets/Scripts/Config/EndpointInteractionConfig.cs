using UnityEngine;

namespace TABApps.TestTask
{
    [CreateAssetMenu(fileName = "EndpointInteractionConfig", menuName = "Config/Endpoint Interaction Config")]
    public class EndpointInteractionConfig : ScriptableObject
    {
        public ApiEndpointData ApiEndpointData;
    }
}