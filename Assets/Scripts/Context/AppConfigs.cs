using UnityEngine;

namespace TABApps.TestTask
{
    public class AppConfigs
    {
        private EndpointInteractionConfig _endpointInteraction;
        public EndpointInteractionConfig EndpointInteraction => _endpointInteraction ??= Resources.Load<EndpointInteractionConfig>(ResourcesPath.ENDPOINT_INTERACTION_CONFIG);
    }
}