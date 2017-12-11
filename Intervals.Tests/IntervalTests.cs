using System.Linq;
using NUnit.Framework;

namespace Intervals.Tests
{
	[TestFixture("linear")]
	[TestFixture("bst")]
	public class IntervalTests
	{
		private readonly string _collectionType;
		private IIntervalCollection _intervalCollection;

		public IntervalTests(string collectionType)
		{
			_collectionType = collectionType;
		}

		[SetUp]
		public void Setup()
		{
			switch (_collectionType)
			{
				case "linear":
					_intervalCollection = new LinearIntervalCollection();
					break;

				case "bst":
					_intervalCollection = new SortedTreeIntervalCollection();
					break;
			}
		}

		[Test]
		public void EmptyTest()
		{
			Assert.That(_intervalCollection.Items, Is.Empty);
		}

		[Test]
		public void SimpleAddTest()
		{
			_intervalCollection.Add(1, 5);

			Assert.That(_intervalCollection.Items, Is.Not.Null);

			var result = _intervalCollection.Items.ToList();
			Assert.That(result, Has.Count.EqualTo(1));
			Assert.That(result[0].Start, Is.EqualTo(1));
			Assert.That(result[0].Stop, Is.EqualTo(5));
		}

		[Test]
		public void AddContainedTest()
		{
			_intervalCollection.Add(1, 5);
			_intervalCollection.Add(1, 5);
			_intervalCollection.Add(2, 3);
			_intervalCollection.Add(2, 5);
			_intervalCollection.Add(1, 4);

			Assert.That(_intervalCollection.Items, Is.Not.Null);

			var result = _intervalCollection.Items.ToList();
			Assert.That(result, Has.Count.EqualTo(1));
			Assert.That(result[0].Start, Is.EqualTo(1));
			Assert.That(result[0].Stop, Is.EqualTo(5));
		}

		[Test]
		public void AddExtendTest()
		{
			_intervalCollection.Add(1, 5);
			_intervalCollection.Add(2, 8);

			Assert.That(_intervalCollection.Items, Is.Not.Null);

			var result = _intervalCollection.Items.ToList();
			Assert.That(result, Has.Count.EqualTo(1));
			Assert.That(result[0].Start, Is.EqualTo(1));
			Assert.That(result[0].Stop, Is.EqualTo(8));
		}

		[Test]
		public void AddAddTest()
		{
			_intervalCollection.Add(1, 5);

			// act
			_intervalCollection.Add(6, 8);

			Assert.That(_intervalCollection.Items, Is.Not.Null);

			var result = _intervalCollection.Items.ToList();
			Assert.That(result, Has.Count.EqualTo(2));
			Assert.That(result[0].Start, Is.EqualTo(1));
			Assert.That(result[0].Stop, Is.EqualTo(5));
			Assert.That(result[1].Start, Is.EqualTo(6));
			Assert.That(result[1].Stop, Is.EqualTo(8));
		}

		[Test]
		public void AddViaRemoveTest()
		{
			_intervalCollection.Add(1, 5);
			_intervalCollection.Add(6, 8);

			// act
			_intervalCollection.Add(2, 10);

			Assert.That(_intervalCollection.Items, Is.Not.Null);

			var result = _intervalCollection.Items.ToList();
			Assert.That(result, Has.Count.EqualTo(1));
			Assert.That(result[0].Start, Is.EqualTo(1));
			Assert.That(result[0].Stop, Is.EqualTo(10));
		}

		[Test]
		public void AddViaRemoveExtendTest()
		{
			_intervalCollection.Add(1, 5);
			_intervalCollection.Add(6, 8);

			// act
			_intervalCollection.Add(2, 7);

			Assert.That(_intervalCollection.Items, Is.Not.Empty);

			var result = _intervalCollection.Items.ToList();
			Assert.That(result, Has.Count.EqualTo(1));
			Assert.That(result[0].Start, Is.EqualTo(1));
			Assert.That(result[0].Stop, Is.EqualTo(8));
		}

		[Test]
		public void RemoveEmptyTest()
		{
			_intervalCollection.Remove(1, 5);

			Assert.That(_intervalCollection.Items, Is.Empty);
		}

		[Test]
		public void RemoveAfterTest()
		{
			_intervalCollection.Add(1, 5);
			_intervalCollection.Remove(6, 7);

			Assert.That(_intervalCollection.Items, Is.Not.Empty);

			var result = _intervalCollection.Items.ToList();
			Assert.That(result, Has.Count.EqualTo(1));
			Assert.That(result[0].Start, Is.EqualTo(1));
			Assert.That(result[0].Stop, Is.EqualTo(5));
		}

		[Test]
		public void RemoveRightAfterTest()
		{
			_intervalCollection.Add(1, 5);
			_intervalCollection.Remove(5, 6);

			Assert.That(_intervalCollection.Items, Is.Not.Empty);

			var result = _intervalCollection.Items.ToList();
			Assert.That(result, Has.Count.EqualTo(1));
			Assert.That(result[0].Start, Is.EqualTo(1));
			Assert.That(result[0].Stop, Is.EqualTo(5));
		}


		[Test]
		public void RemoveViaCutTest()
		{
			_intervalCollection.Add(1, 5);
			_intervalCollection.Remove(2, 5);

			Assert.That(_intervalCollection.Items, Is.Not.Empty);

			var result = _intervalCollection.Items.ToList();
			Assert.That(result, Has.Count.EqualTo(1));
			Assert.That(result[0].Start, Is.EqualTo(1));
			Assert.That(result[0].Stop, Is.EqualTo(2));
		}

		[Test]
		public void RemoveViaSplitTest()
		{
			_intervalCollection.Add(1, 5);

			// act
			_intervalCollection.Remove(2, 3);

			Assert.That(_intervalCollection.Items, Is.Not.Empty);

			var result = _intervalCollection.Items.ToList();
			Assert.That(result, Has.Count.EqualTo(2));
			Assert.That(result[0].Start, Is.EqualTo(1));
			Assert.That(result[0].Stop, Is.EqualTo(2));
			Assert.That(result[1].Start, Is.EqualTo(3));
			Assert.That(result[1].Stop, Is.EqualTo(5));
		}

		[Test]
		public void RemoveWithTailTest()
		{
			_intervalCollection.Add(1, 5);
			_intervalCollection.Add(6, 8);
			_intervalCollection.Add(9, 12);

			// act
			_intervalCollection.Remove(2, 11);

			Assert.That(_intervalCollection.Items, Is.Not.Empty);

			var result = _intervalCollection.Items.ToList();
			Assert.That(result, Has.Count.EqualTo(2));
			Assert.That(result[0].Start, Is.EqualTo(1));
			Assert.That(result[0].Stop, Is.EqualTo(2));
			Assert.That(result[1].Start, Is.EqualTo(11));
			Assert.That(result[1].Stop, Is.EqualTo(12));
		}


		[Test]
		public void RemoveSelfTest()
		{
			_intervalCollection.Add(1, 5);
			_intervalCollection.Remove(1, 5);

			Assert.That(_intervalCollection.Items, Is.Empty);
		}


		[Test]
		public void ExampleTest()
		{
			_intervalCollection.Add(1,5);

			var result = _intervalCollection.Items.ToList();
			Assert.That(result, Has.Count.EqualTo(1));
			Assert.That(result[0].Start, Is.EqualTo(1));
			Assert.That(result[0].Stop, Is.EqualTo(5));

			_intervalCollection.Remove(2, 3);

			result = _intervalCollection.Items.ToList();
			Assert.That(result, Has.Count.EqualTo(2));
			Assert.That(result[0].Start, Is.EqualTo(1));
			Assert.That(result[0].Stop, Is.EqualTo(2));
			Assert.That(result[1].Start, Is.EqualTo(3));
			Assert.That(result[1].Stop, Is.EqualTo(5));

			_intervalCollection.Add(6, 8);

			result = _intervalCollection.Items.ToList();
			Assert.That(result, Has.Count.EqualTo(3));
			Assert.That(result[0].Start, Is.EqualTo(1));
			Assert.That(result[0].Stop, Is.EqualTo(2));
			Assert.That(result[1].Start, Is.EqualTo(3));
			Assert.That(result[1].Stop, Is.EqualTo(5));
			Assert.That(result[2].Start, Is.EqualTo(6));
			Assert.That(result[2].Stop, Is.EqualTo(8));

			_intervalCollection.Remove(4, 7);

			result = _intervalCollection.Items.ToList();
			Assert.That(result, Has.Count.EqualTo(3));
			Assert.That(result[0].Start, Is.EqualTo(1));
			Assert.That(result[0].Stop, Is.EqualTo(2));
			Assert.That(result[1].Start, Is.EqualTo(3));
			Assert.That(result[1].Stop, Is.EqualTo(4));
			Assert.That(result[2].Start, Is.EqualTo(7));
			Assert.That(result[2].Stop, Is.EqualTo(8));

			_intervalCollection.Add(2, 7);

			result = _intervalCollection.Items.ToList();
			Assert.That(result, Has.Count.EqualTo(1));
			Assert.That(result[0].Start, Is.EqualTo(1));
			Assert.That(result[0].Stop, Is.EqualTo(8));
		}
	}
}