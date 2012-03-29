using Mono.Cecil;

namespace Unity.Rules.Maintainability
{
    public class Utilities
    {
        public static bool IsMonoBehaviour( TypeDefinition type )
        {
            // we checked the whole inheritance hierarchy, and we're at the top. No more type. It's not a MonoBehaviour
            if ( type == null || type.BaseType == null ) return false;

            TypeDefinition typeBaseDefinition = type.BaseType.Resolve();
            if ( typeBaseDefinition == null ) return false;

            // the current basetype is a MonoBehaviour
            if ( typeBaseDefinition.FullName == "UnityEngine.MonoBehaviour" ) return true;

            // we still dont know
            return IsMonoBehaviour( typeBaseDefinition );
        }
    }
}
