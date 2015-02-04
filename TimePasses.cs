using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GildedRose.Tests
{
	[TestFixture]
	public class TimePasses : CodedRoseTestBase
	{
		public static IEnumerable<int> Days()
		{
			for(int i = 0; i <= 20; i++)
			{
				yield return i;
			}
		}

		[Test]
		[TestCaseSource("Days")]
		public void PassTime(int days)
		{
			AssertAlwaysConditions();
			for(int i = 0; i < days; i ++)
			{
				NextDay();
			}
		}
	}
}
