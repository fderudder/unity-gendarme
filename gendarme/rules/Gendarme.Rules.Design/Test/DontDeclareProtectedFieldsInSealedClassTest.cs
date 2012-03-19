// 
// Unit tests for DoNotDeclareProtectedMembersInSealedTypeRule
//
// Authors:
//	Nidhi Rawal <sonu2404@gmail.com>
//	Sebastien Pouliot  <sebastien@ximian.com>
//
// Copyright (c) <2007> Nidhi Rawal
// Copyright (C) 2008 Novell, Inc (http://www.novell.com)
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;

using Gendarme.Rules.Design;

using NUnit.Framework;
using Test.Rules.Definitions;
using Test.Rules.Fixtures;

namespace Test.Rules.Design {

	[TestFixture]
	public class DoNotDeclareProtectedMembersInSealedTypeTest : TypeRuleTestFixture<DoNotDeclareProtectedMembersInSealedTypeRule> {

		[Test]
		public void DoesNotApply ()
		{
			// delegates are always sealed - but the rule does not apply to them
			AssertRuleDoesNotApply (SimpleTypes.Delegate);
			// enums are always sealed - but the rule does not apply to them
			AssertRuleDoesNotApply (SimpleTypes.Enum);
			// interfaces are not sealed - and the rule does not apply to them
			AssertRuleDoesNotApply (SimpleTypes.Interface);
			// struct are always sealed - but we can't declare protected fields in them
			AssertRuleDoesNotApply (SimpleTypes.Structure);
		}

		public sealed class SealedClassWithProtectedField {
			protected int i;
			protected double d;
		}

		[Test]
		public void SealedClassWithProtectedFieldTest ()
		{
			AssertRuleFailure<SealedClassWithProtectedField> (2);
		}

		public sealed class SealedClassWithoutProtectedFields {
			public string s;
			private float f;
		}
		
		[Test]
		public void SealedClassWithoutProtectedFieldsTest ()
		{
			AssertRuleSuccess<SealedClassWithoutProtectedFields> ();
		}

		public sealed class SealedClassWithProtectedMethod {
			protected int GetInt ()
			{
				return 42;
			}
		}

		[Test]
		public void SealedClassWithProtectedMethodTest ()
		{
			AssertRuleFailure<SealedClassWithProtectedMethod> (1);
		}

		public sealed class SealedClassWithoutProtectedMethods {
			public string GetInfo ()
			{
				return String.Empty;
			}
		}

		[Test]
		public void SealedClassWithoutProtectedMethodsTest ()
		{
			AssertRuleSuccess<SealedClassWithoutProtectedMethods> ();
		}

		public class UnsealedClass {
			protected double d;
			protected int j;

			protected void Show ()
			{
				Console.WriteLine ("{0} - {1}", j, d);
			}
		}

		[Test]
		public void Unsealed ()
		{
			AssertRuleDoesNotApply<UnsealedClass> ();
		}

		public abstract class AbstractClass {
			protected abstract string GetIt ();
		}

		public sealed class SealedClass : AbstractClass {

			public int GetInt ()
			{
				return 42;
			}

			protected override string GetIt ()
			{
				return String.Empty;
			}

			public override string ToString ()
			{
				return base.ToString ();
			}
		}

		[Test]
		public void Override ()
		{
			AssertRuleDoesNotApply<AbstractClass> ();
			AssertRuleSuccess<SealedClass> ();
		}
	}
}
