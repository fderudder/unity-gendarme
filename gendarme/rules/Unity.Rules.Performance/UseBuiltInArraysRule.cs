
using System.Globalization;
using Gendarme.Framework;
using Mono.Cecil;

namespace Unity.Rules.Performance
{
    [Problem("Array() and ArrayList() are not as optimized as BuiltIn arrays or Generic collections.")]
    [Solution("Replace by BuiltIn arrays when you know the number of elements and it does not vary. Replace by a Generic data structure if the number of elements often changes.")]
    public class UseBuiltInArraysRule : Rule, ITypeRule
    {
        public RuleResult CheckType( TypeDefinition type )
        {
            if ( !Utilities.IsMonoBehaviour( type ) ) return RuleResult.DoesNotApply;

            foreach ( FieldDefinition field in type.Fields )
            {
                if ( field.FieldType.FullName.Equals( "System.Array" ) )
                {
                    string msg = string.Format( CultureInfo.InvariantCulture, "'{0}' is of type \"Array\". You may consider using a built in array instead for performance, or a Generic Collection for flexibility.", field.Name );
                    Runner.Report( field, Severity.High, Confidence.High, msg );
                }

                if(field.FieldType.FullName.Equals( "System.Collections.ArrayList" ))
                {
                    string msg = string.Format( CultureInfo.InvariantCulture, "'{0}' is of type \"ArrayList\". You may consider using a built in array instead for performance, or a Generic Collection for flexibility.", field.Name );
                    Runner.Report( field, Severity.High, Confidence.High, msg );
                }
            }

            return Runner.CurrentRuleResult;
        }
    }
}
