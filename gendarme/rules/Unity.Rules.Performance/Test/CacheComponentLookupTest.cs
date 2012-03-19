using NUnit.Framework;

using Test.Rules.Definitions;
using Test.Rules.Fixtures;

using Unity.Rules.Performance;
using UnityEngine;

namespace Tests.Unity.Rules.Performance
{
    [TestFixture]
    public class CacheComponentLookupTest : MethodRuleTestFixture<CacheComponentLookupRule>
    {

        [Test]
        public void EmptyMethodTest()
        {
            AssertRuleDoesNotApply( SimpleMethods.EmptyMethod );
        }

        [Test]
        public void MustBeCachedCheck()
        {
            AssertRuleFailure<MustBeCachedClass>("Update");
        }

        [Test]
        public void NoCacheNeededCheck()
        {
            AssertRuleSuccess<NoCacheNeeded>("Update");
        }

        [Test]
        public void TrickyCacheCheck()
        {
            AssertRuleFailure<TrickyCache>("Update");
            AssertRuleFailure<TrickyCache>("LateUpdate");
        }
    }

    public class MustBeCachedClass : MonoBehaviour
    {
        void Update()
        {
            this.transform.RotateAround( Vector3.zero, 5f );
        }
    }

    public class NoCacheNeeded : MonoBehaviour
    {
        private Transform m_transform;

        void Start()
        {
            m_transform = transform;
        }

        void Update()
        {
            m_transform.RotateAround( Vector3.zero, 5f );
        }
    }

    public class TrickyCache : MonoBehaviour
    {
        void Update()
        {
            Call_1( true );
        }

        void LateUpdate()
        {
            Call_1( false );
        }

        #region indirect calls

        void Call_1( bool directLookup )
        {
            Call_2( directLookup  );
        }

        void Call_2( bool directLookup )
        {
            Call_3( directLookup  );
        }

        void Call_3( bool directLookup )
        {
            Call_4( directLookup );
        }

        void Call_4( bool directLookup )
        {
            Call_5( directLookup );
        }

        void Call_5( bool directLookup )
        {
            if (directLookup) DirectLookupMethod();
            else IndirectLookupMethod();
        }

        #endregion

        void DirectLookupMethod()
        {
            camera.transform.Translate( Vector3.one );
        }

        void IndirectLookupMethod()
        {
            Transform theTransform = this.transform;
            theTransform.RotateAround( Vector3.zero, 5f );
        }
    }
}