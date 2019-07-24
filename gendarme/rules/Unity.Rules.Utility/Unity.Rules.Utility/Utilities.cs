namespace Unity.Rules.Utility
{
    using Mono.Cecil;

    /// <summary>
    /// Utility functions for the Unity.Rules.Maintainability project
    /// </summary>
    public static class Utilities
    {
        /// <summary>
        /// Resolves the inheritance of a TypeDefinition to determine if it's a MonoBehaviour
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsMonoBehaviour(TypeDefinition type)
        {
            // we checked the whole inheritance hierarchy, and we're at the top. No more type. It's not a MonoBehaviour
            if (type?.BaseType == null)
            {
                return false;
            }

            // the current base type is a MonoBehaviour
            if (type.BaseType.FullName == "UnityEngine.MonoBehaviour")
            {
                return true;
            }

            // we still don't know
            return IsMonoBehaviour(type.BaseType.Resolve());
        }
    }
}
