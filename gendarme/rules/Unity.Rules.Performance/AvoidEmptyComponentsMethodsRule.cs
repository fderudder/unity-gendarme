using System;
using System.Globalization;
using System.Linq;

using Gendarme.Framework;

using Mono.Cecil;

namespace Unity.Rules.Performance
{
    [Problem( "Empty methods coming from MonoBehaviour (such as Start() or Update() can causes overhead at runtime, which leads to a loss of performances." )]
    [Solution( "Delete the empty methods." )]
    public class AvoidEmptyComponentsMethodsRule : Rule, IMethodRule
    {
        private readonly string[] methodNames = {
                                                    "Update",
                                                    "LateUpdate",
                                                    "FixedUpdate",
                                                    "Awake",
                                                    "Start",
                                                    "Reset",
                                                    "OnMouseEnter",
                                                    "OnMouseOver",
                                                    "OnMouseExit",
                                                    "OnMouseDown",
                                                    "OnMouseUp",
                                                    "OnMouseUpAsButton",
                                                    "OnMouseDrag",
                                                    "OnTriggerEnter",
                                                    "OnTriggerExit",
                                                    "OnTriggerStay",
                                                    "OnCollisionEnter",
                                                    "OnCollisionExit",
                                                    "OnCollisionStay",
                                                    "OnControllerColliderHit",
                                                    "OnJointBreak",
                                                    "OnParticleCollision",
                                                    "OnBecameVisible",
                                                    "OnBecameInvisible",
                                                    "OnLevelWasLoaded",
                                                    "OnEnable",
                                                    "OnDisable",
                                                    "OnDestroy",
                                                    "OnPreCull",
                                                    "OnPreRender",
                                                    "OnPostRender",
                                                    "OnRenderObject",
                                                    "OnWillRenderObject",
                                                    "OnGUI",
                                                    "OnRenderImage",
                                                    "OnDrawGizmosSelected",
                                                    "OnDrawGizmos",
                                                    "OnApplicationPause",
                                                    "OnApplicationFocus",
                                                    "OnApplicationQuit",
                                                    "OnPlayerConnected",
                                                    "OnServerInitialized",
                                                    "OnConnectedToServer",
                                                    "OnPlayerDisconnected",
                                                    "OnDisconnectedFromServer",
                                                    "OnFailedToConnect",
                                                    "OnFailedToConnectToMasterServer",
                                                    "OnMasterServerEvent",
                                                    "OnNetworkInstantiate",
                                                    "OnSerializeNetworkView"
                                                };

        public RuleResult CheckMethod( MethodDefinition method )
        {
            if ( !Utilities.IsMonoBehaviour( method.DeclaringType ) ) return RuleResult.DoesNotApply;

            // check if the method name is one of the native methods of MonoBehaviour
            if( methodNames.Contains( method.Name ) )
            {
                if ( method.Body.CodeSize > 1 ) return RuleResult.Success;

                string message = String.Format( CultureInfo.CurrentCulture, "The method is empty, and can cause some performance overhead that you could get rid of." );
                Runner.Report( method, Severity.High, Confidence.High, message );
            }

            return RuleResult.Failure;
        }
    }
}