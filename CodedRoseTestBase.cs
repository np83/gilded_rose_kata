using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GildedRose.Tests
{
	[TestFixture]
	public abstract class CodedRoseTestBase
	{
		[Test]
		public void SetUp()
		{
			GildedRose.Frame.Items.Clear();
			foreach (var item in GildedRose.Frame.CreateDayOne())
			{
				GildedRose.Frame.Items.Add(item);
			}
		}

		[Test]
		public void NextDay()
		{
			AssertAlwaysConditions();

			//•"Aged Brie" actually increases in Quality the older it gets
			var qualityOfBrieBefore = GildedRose.Frame.Items.First(x => x.Name == "Aged Brie").Quality;
			var qualityOfSulfurasBefore = GildedRose.Frame.Items.First(x => x.Name == "Sulfuras, Hand of Ragnaros").Quality;

			GildedRose.Frame.UpdateQuality();

			var qualityOfBrieAfter = GildedRose.Frame.Items.First(x => x.Name == "Aged Brie").Quality;
			var qualityOfSulfurasAfter = GildedRose.Frame.Items.First(x => x.Name == "Sulfuras, Hand of Ragnaros").Quality;

			//•"Aged Brie" actually increases in Quality the older it gets
			if (qualityOfBrieBefore < 50)
			{
				Assert.That(qualityOfBrieAfter, Is.GreaterThan(qualityOfBrieBefore), "Quality of brie should increase as days pass.");
			}

			//•"Sulfuras", being a legendary item, never has to be sold or decreases in Quality
			Assert.That(qualityOfSulfurasAfter, Is.EqualTo(qualityOfSulfurasBefore), "Quality of Sulfuras should never change.");

			//•"Backstage passes", like aged brie, increases in Quality as it's SellIn value approaches; Quality increases by 2 when there are 10 days or less and by 3 when there are 5 days or less but Quality drops to 0 after the concert
			ValidateBackstagePassQuality();

			AssertAlwaysConditions();
		}

		public void ValidateBackstagePassQuality()
		{
			var item = GildedRose.Frame.Items.First(x => x.Name == "Backstage passes to a TAFKAL80ETC concert");

			//•"Backstage passes", 
			// like aged brie, increases in Quality as it's SellIn value approaches; 
			// Quality increases by 2 when there are 10 days or less and by 3 when there are 5 days or less but Quality drops to 0 after the concert

			var expectedQualityAtDay = ExpectedBackStageQualityBySellIn();

			var expectedQuality = expectedQualityAtDay[item.SellIn >= 0 ? item.SellIn : -1];
			Assert.That(item.Quality, Is.EqualTo(expectedQuality), "Expected a different quality for the backstage pass.");
		}

		private static Dictionary<int,int> ExpectedBackStageQualityBySellIn()
		{
			var expectedQualityAtDay = new Dictionary<int, int>();
			var currentQuality = Frame.CreateDayOne().First(x => x.Name == "Backstage passes to a TAFKAL80ETC concert").Quality;

			int numberOfDaysLeft = 15;

			while (numberOfDaysLeft >= 0)
			{
				expectedQualityAtDay[numberOfDaysLeft] = currentQuality;

				if (numberOfDaysLeft < 0)
				{
					currentQuality = 0;
				}
				else if (numberOfDaysLeft <= 5)
				{
					currentQuality = currentQuality + 3;
				}
				else if (numberOfDaysLeft <= 10)
				{
					currentQuality = currentQuality + 2;
				}
				else
				{
					currentQuality = currentQuality + 1;
				}

				numberOfDaysLeft--;
			}
			expectedQualityAtDay[-1] = 0;
			return expectedQualityAtDay;
		}
		
		public static void ValidateNeverNegative()
		{
			foreach (var item in GildedRose.Frame.Items)
			{
				Assert.That(item.Quality, Is.GreaterThanOrEqualTo(0));
			}
		}

		public static void ValidateNeverOver50ExceptForSulfuras()
		{
			foreach(var item in GildedRose.Frame.Items)
			{
				if (item.Name == "Sulfuras, Hand of Ragnaros")
				{
					Assert.That(item.Quality, Is.EqualTo(80));
				}
				else
				{
					Assert.That(item.Quality, Is.LessThanOrEqualTo(50));
				}
			}
		}

		public void AssertAlwaysConditions()
		{
			ValidateNeverNegative();
			ValidateNeverOver50ExceptForSulfuras();
		}
	}
}
