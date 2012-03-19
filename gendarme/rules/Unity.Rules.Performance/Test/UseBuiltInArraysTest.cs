
using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using Test.Rules.Fixtures;
using Unity.Rules.Performance;
using UnityEngine;

namespace Tests.Unity.Rules.Performance
{
    [TestFixture]
    public class UseBuiltInArraysTest  : TypeRuleTestFixture<UseBuiltInArraysRule>
    {
        [Test]
        public void ArrayListTest()
        {
            AssertRuleFailure<ArrayListClass>();
        }

        [Test]
        public void ArrayTest()
        {
            AssertRuleFailure<ArrayClass>();
        }

        [Test]
        public void BuiltInArray()
        {
            AssertRuleSuccess<BuiltInArrayClass>();
        }

        [Test]
        public void ListOrOtherGeneric()
        {
            AssertRuleSuccess<ListClass>();
        }
    }

    public class ArrayListClass : MonoBehaviour
    {
        private ArrayList m_arrayList;
    }

    public class ArrayClass : MonoBehaviour
    {
        private Array m_array;
    }

    public class BuiltInArrayClass : MonoBehaviour
    {
        private int[] m_array;
    }

    public class ListClass : MonoBehaviour
    {
        private List<int> m_list;
    }
}
