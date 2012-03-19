using System;

using NUnit.Framework;

using Test.Rules.Definitions;
using Test.Rules.Fixtures;

using Unity.Rules.Performance;

using UnityEngine;

namespace Tests.Unity.Rules.Performance
{
    [TestFixture]
    public class AvoidEmptyComponentsMethodsTest : MethodRuleTestFixture<AvoidEmptyComponentsMethodsRule>
    {
        [Test]
        public void EmptyMethodTest()
        {
            AssertRuleDoesNotApply( SimpleMethods.EmptyMethod );
        }

        [Test]
        public void FailClassMethodsTest()
        {
            AssertRuleFailure<FailClass>( "Start" );
            AssertRuleFailure<FailClass>( "Update" );
            AssertRuleFailure<FailClass>( "Awake" );
        }

        [Test]
        public void SuccessClassMethodsTest()
        {
            AssertRuleSuccess<SuccessClass>( "Start" );
            AssertRuleSuccess<SuccessClass>( "Update" );
            AssertRuleSuccess<SuccessClass>( "Awake" );
        }
    }

    public class FailClass : MonoBehaviour
    {
        private void Start()
        {
        }

        private void Update()
        {
        }

        private void Awake()
        {
        }
    }

    public class SuccessClass : MonoBehaviour
    {
        private int i;

        private void Start()
        {
            i = 0;
            Console.WriteLine( "stuff" );
        }
        private void Update()
        {
            Console.WriteLine( "update stuff" );
            i++;
        }
        private void Awake()
        {
            Console.WriteLine( "stuff" );
            Debug.Log( i );    
        }
    }
}
