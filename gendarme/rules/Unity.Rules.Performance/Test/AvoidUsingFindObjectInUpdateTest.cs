using NUnit.Framework;

using Test.Rules.Definitions;
using Test.Rules.Fixtures;

using Unity.Rules.Performance;
using UnityEngine;

namespace Tests.Unity.Rules.Performance
{
    [TestFixture]
    public class AvoidUsingFindObjectInUpdateTest : MethodRuleTestFixture<AvoidUsingFindObjectInUpdateRule>
    {
        [Test]
        public void EmptyMethodTest()
        {
            AssertRuleDoesNotApply( SimpleMethods.EmptyMethod );
            AssertRuleDoesNotApply<DaClass>( "ATestMethod" );
        }

        [Test]
        public void FindObjectsFailTest()
        {
            AssertRuleFailure<UnityObjectFindObjectsOfType>( "Update" );
            AssertRuleFailure<UnityObjectFindObjectsOfType>( "FixedUpdate" );
            AssertRuleFailure<UnityObjectFindObjectsOfType>( "LateUpdate" );
        }

        [Test]
        public void FindObjectFailTest()
        {
            AssertRuleFailure<UnityObjectFindObjectOfType>( "Update" );
            AssertRuleFailure<UnityObjectFindObjectOfType>( "FixedUpdate" );
            AssertRuleFailure<UnityObjectFindObjectOfType>( "LateUpdate" );
        }

        [Test]
        public void GameObjectFindFailTest()
        {
            AssertRuleFailure<GameObjectFind>( "Update" );
            AssertRuleFailure<GameObjectFind>( "FixedUpdate" );
            AssertRuleFailure<GameObjectFind>( "LateUpdate" );
        }
    }

    public class DaClass
    {
        public bool ATestMethod()
        {
            const int anInt = 5;
            bool anEnt = anInt > 5;

            return anEnt;
        }

    }

    public class UnityObjectFindObjectsOfType : MonoBehaviour
    {
        void Update()
        {
            FindObjectsOfType( typeof ( MonoBehaviour ) );
        }

        void FixedUpdate()
        {
            FindObjectsOfType( typeof( MonoBehaviour ) );
        }

        void LateUpdate()
        {
            FindObjectsOfType( typeof( MonoBehaviour ) );
        }
    }
    
    public class UnityObjectFindObjectOfType : MonoBehaviour
    {
        void Update()
        {
            FindObjectOfType( typeof ( MonoBehaviour ) );
        }

        void FixedUpdate()
        {
            FindObjectOfType( typeof( MonoBehaviour ) );
        }

        void LateUpdate()
        {
            FindObjectOfType( typeof( MonoBehaviour ) );
        }
    }
    
    public class GameObjectFind : MonoBehaviour
    {
        void Update()
        {
            GameObject.Find( "maru" );
        }

        void FixedUpdate()
        {
            GameObject.Find( "maru" );
        }

        void LateUpdate()
        {
            GameObject.Find( "maru" );
        }
    }
}
