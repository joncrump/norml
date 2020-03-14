using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Norml.Tests.Common.Helpers
{
    public class NUnitAssertAdapter : IAssertAdapter
    {
        public void AreEqual(object expected, object actual, string message = null)
        {
            Assert.That(actual, Is.EqualTo(expected), message);
        }

        public void IsTrue(bool condition, string message = null)
        {
            Assert.That(condition, Is.True, message);
        }

        public void AreEqual<TItem>(TItem expected, TItem actual, string message = null)
        {
            Assert.That(actual, Is.EqualTo(expected), message);
        }

        public void AreNotEqual<TItem>(TItem expected, TItem actual, string message = null)
        {
            Assert.That(actual, Is.Not.EqualTo(expected), message);
        }

        public void IsFalse(bool condition, string message = null)
        {
            Assert.That(condition, Is.False, message);
        }

        public void Inconclusive(string message)
        {
            Assert.Inconclusive(message);
        }

        public void IsNotNull(object value, string message = null)
        {
            Assert.That(value, Is.Not.Null);
        }

        public void IsNull(object value, string message = null)
        {
            Assert.That(value, Is.Null);
        }

        public void AreEqual<TValue, TKey>(IEnumerable<TValue> expected, IEnumerable<TValue> actual, Func<TValue, TKey> orderSelector = null,
            IEqualityComparer<TValue> comparer = null)
        {
            if (expected == null)
            {
                Assert.IsNull(actual);
                return;
            }

            if (expected != null)
            {
                Assert.IsNotNull(actual);
            }

            Assert.AreEqual(expected.Count(), actual.Count());

            if (orderSelector != null)
            {
                var sortedExpected = expected.OrderBy(orderSelector);
                var sortedActual = actual.OrderBy(orderSelector);
                AssertEnumerable(sortedExpected, sortedActual, comparer);
                return;
            }

            AssertEnumerable(expected, actual, comparer);
        }

        private void AssertEnumerable<TValue>(IEnumerable<TValue> expected, IEnumerable<TValue> actual,
            IEqualityComparer<TValue> comparer)
        {
            for (var index = 0; index < expected.Count(); index++)
            {
                if (comparer != null)
                {
                    AssertWithComparer(expected.ElementAt(index), actual.ElementAt(index), comparer);
                }
                else
                {
                    Assert.AreEqual(expected.ElementAt(index), actual.ElementAt(index));
                }
            }
        }

        private void AssertWithComparer<TValue>(TValue expected, TValue actual, IEqualityComparer<TValue> comparer)
        {
            if (!comparer.Equals(expected, actual))
            {
                throw new AssertionException("Objects are not equal via comparer");
            }
        }
    }
}