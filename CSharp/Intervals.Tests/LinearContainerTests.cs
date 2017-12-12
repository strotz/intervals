using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Intervals.Tests
{
	[TestFixture]
	public class LinearContainerTests
	{
		private LinearIntervalCollection _intervalCollection;

		[SetUp]
		public void Setup()
		{
			_intervalCollection = new LinearIntervalCollection();
		}

		[Test]
		public void FindPredecessorEmptyTest()
		{
			var pre = _intervalCollection.FindPredecessor(3);
			Assert.That(pre, Is.Null);
		}

		[Test]
		public void FindPredecessorBeforeTest()
		{
			_intervalCollection.Add(1, 2); // first add is just simple add

			var pre = _intervalCollection.FindPredecessor(3);
			Assert.That(pre, Is.Not.Null);
			Assert.That(pre.Value.Start, Is.EqualTo(1));
		}

		[Test]
		public void FindPredecessorExactTest()
		{
			_intervalCollection.Add(3, 4); // first add is just simple add

			var pre = _intervalCollection.FindPredecessor(3);
			Assert.That(pre, Is.Not.Null);
			Assert.That(pre.Value.Start, Is.EqualTo(3));
		}

		[Test]
		public void FindPredecessorAfterTest()
		{
			_intervalCollection.Add(4, 5); // first add is just simple add

			var pre = _intervalCollection.FindPredecessor(3);
			Assert.That(pre, Is.Null);
		}

	}
}
