using System;
using System.Globalization;
using System.Linq;

using Gendarme.Framework;

using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Unity.Rules.Performance
{
    [Problem("Object.FindObjectOfType(), Object.FindObjectsOfType() and GameObject.Find(name) are time consuming methods. You shouldnt use them each frame, in the Update() methods")]
    [Solution("Use those methods in Start() or Awake(), and cache a reference to the found object.")]
    public class AvoidUsingFindObjectInUpdateRule : Rule, IMethodRule
    {
        private readonly string[] methodNames = {
                                                    "Update",
                                                    "LateUpdate",
                                                    "FixedUpdate"
                                                };

        public RuleResult CheckMethod( MethodDefinition method )
        {
            if ( !Utilities.IsMonoBehaviour( method.DeclaringType ) ) return RuleResult.DoesNotApply;

            if ( methodNames.Contains( method.Name ) )
            {
                if ( !method.HasBody ) return RuleResult.DoesNotApply;

                // check for GameObject.Find's kind methods in those methods
                foreach ( Instruction instruction in method.Body.Instructions )
                {
                    Code code = instruction.OpCode.Code;
                    if ( code != Code.Call ) continue;

                    MethodReference def = instruction.Operand as MethodReference;
                    if ( def == null ) continue;

                    if ( def.DeclaringType.FullName.Equals( "UnityEngine.Object" ) &&
                         def.Name.Equals( "FindObjectOfType" ) )
                    {
                        string message = String.Format( CultureInfo.CurrentCulture,
                                                        "The {0}.{1} method is known to cause some serious performance loss if used in Update, LateUpdate or FixedUpdate." +
                                                        "\nFind the reference you want to lookup in Start() or Awake() instead, and cache it in a local member.",
                                                        "UnityObject",
                                                        "FindObjectOfType()" );
                        Runner.Report( method, instruction, Severity.Critical, Confidence.Total, message );

                        continue;
                    }

                    if ( def.DeclaringType.FullName.Equals( "UnityEngine.Object" ) &&
                         def.Name.Equals( "FindObjectsOfType" ) )
                    {
                        string message = String.Format( CultureInfo.CurrentCulture,
                                                        "This {0}.{1} is known to cause some serious performance loss if used in Update, LateUpdate or FixedUpdate." +
                                                        "\nFind the reference you want to lookup in Start() or Awake() instead, and cache it in a local member.",
                                                        "UnityObject",
                                                        "FindObjectsOfType()" );
                        Runner.Report( method, instruction, Severity.Critical, Confidence.Total, message );

                        continue;
                    }

                    if ( def.DeclaringType.FullName.Equals( "UnityEngine.GameObject" ) && def.Name.Equals( "Find" ) )
                    {
                        string message = String.Format( CultureInfo.CurrentCulture,
                                "This {0}.{1} is known to cause some serious performance loss if used in Update, LateUpdate or FixedUpdate." +
                                "\nFind the reference you want to lookup in Start() or Awake() instead, and cache it in a local member.",
                                "GameObject",
                                "Find()" );
                        Runner.Report( method, instruction, Severity.Critical, Confidence.Total, message );

                        continue;
                    }
                }
            }

            return Runner.CurrentRuleResult;
        }
    }
}
