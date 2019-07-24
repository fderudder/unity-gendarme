namespace Unity.Rules.Performance
{
    using System;
    using System.Globalization;
    using System.Linq;

    using Gendarme.Framework;

    using Mono.Cecil;
    
    using Unity.Rules.Utility;

    /// <summary>
    /// Empty Methods from MonoBehaviours have a slight performance hit. Better get rid of them
    /// </summary>
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
#if DEBUG // when built in debug with msbuild, the empty methods have 2 instructions, and 0 in release
                if ( method.Body.CodeSize > 2 ) return RuleResult.Success;
#else
                if ( method.Body.CodeSize > 1 ) return RuleResult.Success;
#endif

                string message = String.Format( CultureInfo.CurrentCulture, "The method is empty, and can cause some performance overhead that you could get rid of." );
                Runner.Report( method, Severity.High, Confidence.High, message );
            }

            return RuleResult.Failure;
        }
    }
}