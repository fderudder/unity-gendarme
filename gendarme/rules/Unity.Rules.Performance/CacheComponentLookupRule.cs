using System;
using System.Globalization;
using System.Linq;

using Gendarme.Framework;

using Mono.Cecil;
using Mono.Cecil.Cil;

// TODO : update the severity report with a severity threshold based of the number of occurences of Component lookups
namespace Unity.Rules.Performance
{

    [Problem( "Recurrent components calls in update can leads to performance loss." )]
    [Solution( "Cache the component call in a local member, on Start() or Awake()." )]
    public class CacheComponentLookupRule : Rule, IMethodRule
    {
        private static readonly string[] methodNames = {
                                                            "Update",
                                                            "LateUpdate",
                                                            "FixedUpdate",
                                                        };

        public RuleResult CheckMethod( MethodDefinition method )
        {
            // Check if we're in a MonoBehaviour
            if ( !Utilities.IsMonoBehaviour( method.DeclaringType ) ) return RuleResult.DoesNotApply;

            // Check if we're in an heavily used method
            if ( methodNames.Contains( method.Name ) )
            {
                ComputeNumberOfComponentLookup( method, 0 );
            }
            
            return Runner.CurrentRuleResult;
        }

        int ComputeNumberOfComponentLookup( MethodDefinition method , int level )
        {
            int lookups = 0;
            int currentLevel = level + 1;

            if ( method == null || !method.HasBody || currentLevel > 8 ) return lookups;

            // Check if there's an access to a component property
            // check for GetComponent<>
            foreach ( Instruction instruction in method.Body.Instructions )
            {
                Code code = instruction.OpCode.Code;
                if ( code != Code.Callvirt && code != Code.Call ) continue;

                MethodReference methodReference = instruction.Operand as MethodReference;
                if ( methodReference == null ) continue;

                // Debug.Log can lead to stackoverflow because of string building
                if ( methodReference.DeclaringType.FullName.Equals( "UnityEngine.Debug" ) ) return lookups;

                // 
                if ( methodReference.DeclaringType.FullName.Equals( "UnityEngine.Component" ) )
                {
                    lookups++;
                    // Write report about that defect
                    string message = String.Format( CultureInfo.CurrentCulture, "{0} Component lookup could be cached.", methodReference.Name );
                    Runner.Report( method, instruction, Severity.High, Confidence.Normal, message );
                }
                else
                {
                    MethodDefinition methodDefinition = methodReference.Resolve();
                    lookups += ComputeNumberOfComponentLookup( methodDefinition, currentLevel );
                }

            }
            
            return lookups;
        }
    }
}
