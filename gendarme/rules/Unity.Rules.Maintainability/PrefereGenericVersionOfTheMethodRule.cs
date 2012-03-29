using System;
using System.Globalization;
using System.Linq;

using Gendarme.Framework;

using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Unity.Rules.Maintainability
{
    [Problem("")]
    [Solution("")]
    public class PrefereGenericVersionOfTheMethodRule : Rule, IMethodRule
    {
        private static readonly string[] methodNames = {
                                                           "GetComponent",
                                                           "GetComponentInChildren",
                                                           "GetComponents",
                                                           "GetComponentsInChildren",
                                                           "AddComponent",
                                                           "CreateInstance"
                                                       };

        public RuleResult CheckMethod( MethodDefinition method )
        {
            if(!Applicable( method )) return RuleResult.DoesNotApply;

            foreach ( Instruction instruction in method.Body.Instructions )
            {
                Code code = instruction.OpCode.Code;
                if ( code != Code.Callvirt && code != Code.Call ) continue;

                MethodReference methodReference = instruction.Operand as MethodReference;
                if ( methodReference == null ) continue;
                if ( !methodNames.Contains( methodReference.Name ) ) continue;

                if ( methodReference.DeclaringType.FullName.StartsWith( "UnityEngine" ) )
                {
                    if ( methodReference.CallingConvention == MethodCallingConvention.Generic ) continue;

                    string message = String.Format( CultureInfo.CurrentCulture, "The method {0} could be replaced by its Generic counterpart.", methodReference.Name );
                    Runner.Report( method, instruction, Severity.Low, Confidence.Total, message );
                }
            }

            return Runner.CurrentRuleResult;
        }

        static private bool Applicable( MethodDefinition method )
        {
            if ( !Utilities.IsMonoBehaviour( method.DeclaringType ) ) return false;

            if ( !method.HasBody ) return false;

            return true;
        }
    }
}
