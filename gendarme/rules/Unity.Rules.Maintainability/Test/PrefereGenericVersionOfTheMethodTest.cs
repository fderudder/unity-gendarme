using NUnit.Framework;

using Test.Rules.Definitions;
using Test.Rules.Fixtures;

using Unity.Rules.Maintainability;

using UnityEngine;

namespace Tests.Unity.Rules.Maintenability
{
    [TestFixture]
    public class PrefereGenericVersionOfTheMethodTest : MethodRuleTestFixture<PrefereGenericVersionOfTheMethodRule>
    {
        [Test]
        public void EmptyMethodTest()
        {
            AssertRuleDoesNotApply( SimpleMethods.EmptyMethod );
        }

        [Test]
        public void TypeOfCall()
        {
            AssertRuleFailure<FailClass>( "TypeOfCallGetComponent" );
            AssertRuleFailure<FailClass>( "TypeOfCallGetComponents" );
            AssertRuleFailure<FailClass>( "TypeOfCallGetComponentInChildren" );
            AssertRuleFailure<FailClass>( "TypeOfCallGetComponentsInChildre" );
            AssertRuleFailure<FailClass>( "TypeOfCallAddComponent" );
            AssertRuleFailure<FailClass>( "TypeOfCallCreateInstance" );
        }

        [Test]
        public void StringCall()
        {
            AssertRuleFailure<FailClass>( "StringCallGetComponent" );
            AssertRuleFailure<FailClass>( "StringCallAddComponent" );
            AssertRuleFailure<FailClass>( "StringCallCreateInstance" );
        }

        [Test]
        public void GenericCall()
        {
            AssertRuleSuccess<SuccessClass>( "GenericCallGetComponent" );
            AssertRuleSuccess<SuccessClass>( "GenericCallGetComponents" );
            AssertRuleSuccess<SuccessClass>( "GenericCallGetComponentInChildren" );
            AssertRuleSuccess<SuccessClass>( "GenericCallGetComponentsInChildren" );
            AssertRuleSuccess<SuccessClass>( "GenericCallAddComponent" );
            AssertRuleSuccess<SuccessClass>( "GenericCallCreateInstance" );
        }
    }

    public class FailClass : MonoBehaviour
    {
        private void TypeOfCallGetComponent()
        {
            Rigidbody body = ( Rigidbody ) GetComponent( typeof ( Rigidbody ) );
        }

        private void TypeOfCallGetComponents()
        {
            Rigidbody[] bodies = (Rigidbody[])GetComponents( typeof( Rigidbody ) );
        }

        private void TypeOfCallGetComponentInChildren()
        {
            Rigidbody body = (Rigidbody)GetComponentInChildren( typeof( Rigidbody ) );
        }

        private void TypeOfCallGetComponentsInChildre()
        {
            Rigidbody[] bodies = (Rigidbody[])GetComponentsInChildren( typeof( Rigidbody ) );
        }

        private void TypeOfCallAddComponent()
        {
            Rigidbody body = (Rigidbody) gameObject.AddComponent( typeof( Rigidbody ) );
        }

        private void TypeOfCallCreateInstance()
        {
            DatScript script = (DatScript)ScriptableObject.CreateInstance( typeof( DatScript ) );
        }

        private void StringCallGetComponent()
        {
            Rigidbody body = ( Rigidbody ) GetComponent( "Rigidbody" );
        }

        private void StringCallCreateInstance()
        {
            DatScript script = ( DatScript ) ScriptableObject.CreateInstance( "DatScript" );
        }
    }

    public class SuccessClass : MonoBehaviour
    {
        private void GenericCallGetComponent()
        {
            Rigidbody body = GetComponent<Rigidbody>();
        }

        private void GenericCallGetComponents()
        {
            Rigidbody[] bodies = GetComponents<Rigidbody>();
        }

        private void GenericCallGetComponentInChildren()
        {
            Rigidbody body = GetComponentInChildren<Rigidbody>();
        }

        private void GenericCallGetComponentsInChildren()
        {
            Rigidbody[] bodies = GetComponentsInChildren<Rigidbody>();
        }

        private void GenericCallAddComponent()
        {
            Rigidbody body = gameObject.AddComponent<Rigidbody>();
        }

        private void GenericCallCreateInstance()
        {
            DatScript script = ScriptableObject.CreateInstance<DatScript>();
        }
    }

    public class DatScript : ScriptableObject
    {
        private void OnDisable()
        {
            Debug.Log( "Disabled" );
        }
    }
}