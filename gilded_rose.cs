using System;
using System.Collections.Generic;

namespace GildedRose
{
	public class GildedRoseStore
	{
		private readonly List<Item> items;

		public IList<Item> Items
		{
			get { return items; }
		}

		public GildedRoseStore()
			: this(CreateInitialStock())
		{
		}

		public GildedRoseStore(IEnumerable<Item> items)
		{
			this.items = new List<Item>(items);
		}

		private static IList<Item> CreateInitialStock()
		{
			return new List<Item>
			{
				new Item { Name = "+5 Dexterity Vest", SellIn = 10, Quality = 20 },
				new Item { Name = "Aged Brie", SellIn = 2, Quality = 0 },
				new Item { Name = "Elixir of the Mongoose", SellIn = 5, Quality = 7 },
				new Item { Name = "Sulfuras, Hand of Ragnaros", SellIn = 0, Quality = 80 },
				new Item { Name = "Backstage passes to a TAFKAL80ETC concert", SellIn = 15, Quality = 20 },
				new Item { Name = "Conjured Mana Cake", SellIn = 3, Quality = 6 },
			};
		}

		public void UpdateQuality()
		{
			for (var i = 0; i < Items.Count; i++)
			{
				var item = Items[i];
				var updater = GetUpdater(item.Name);

				updater.Update(item);
			}
		}

		/// <summary>
		/// This factory method creates new instances of the update classes every call.
		/// Reasons:
		/// + All updater's are self contained
		/// - New instances are used, but they could be reused
		/// + No management code for keeping the instances is required, making the code 'simpler'.
		/// </summary>
		private static ItemUpdater GetUpdater(string itemName)
		{
			switch (itemName)
			{
				case "Sulfuras, Hand of Ragnaros":
					return new LegendaryUpdated();
				case "Aged Brie":
					return new AgedBrieUpdater();
				case "Backstage passes to a TAFKAL80ETC concert":
					return new BackStageUpdate();
				default:
					return new NormalItemUpdater();
			}
		}

		/// <summary>
		/// This class handles both Quality and Sell in, which are two different responsibilities.
		/// The nice thing is that the knowledge of the product is contained in a single class.
		/// </summary>
		public abstract class ItemUpdater
		{
			protected abstract int GetNewQuality(Item item);

			protected abstract int GetNewSellIn(Item item);

			public void Update(Item item)
			{
				var quality = GetNewQuality(item);
				var sellIn = GetNewSellIn(item);

				item.Quality = quality;
				item.SellIn = sellIn;
			}
		}

		public class LegendaryUpdated : ItemUpdater
		{
			protected override int GetNewQuality(Item item)
			{
				return item.Quality;
			}

			protected override int GetNewSellIn(Item item)
			{
				return item.SellIn;
			}
		}

		public class NormalItemUpdater : ItemUpdater
		{
			protected override int GetNewQuality(Item item)
			{
				var delta = GetQualityDelta(item);

				var newQuality = item.Quality + delta;

				if (newQuality >= 50)
					return 50;
				if (newQuality <= 0)
					return 0;

				return newQuality;
			}

			protected virtual int GetQualityDelta(Item item)
			{
				var delta = -1;
				if (item.SellIn < 1)
				{
					delta = -2;
				}

				return delta;
			}

			protected override int GetNewSellIn(Item item)
			{
				return item.SellIn - 1;
			}
		}

		public class AgedBrieUpdater : NormalItemUpdater
		{
			protected override int GetQualityDelta(Item item)
			{
				var addition = 1;

				if (item.SellIn < 1)
				{
					addition = addition + 1;
				}

				return addition;
			}
		}

		public class BackStageUpdate : NormalItemUpdater
		{
			protected override int GetQualityDelta(Item item)
			{
				if (item.SellIn < 1)
				{
					return -item.Quality;
				}
				else
				{
					var addition = 1;
					if (item.SellIn < 11)
					{
						addition = addition + 1;
					}

					if (item.SellIn < 6)
					{
						addition = addition + 1;
					}

					return addition;
				}
			}
		}

		#region Solution direction 1.

		//public void UpdateQuality()
		//{
		//	for (var i = 0; i < Items.Count; i++)
		//	{
		//		var item = Items[i];
		//		var qualityUpdater = GetUpdateQuality(item.Name);
		//		var sellInUpdater = GetUpdateSellIn(item.Name);

		//		item.Quality = qualityUpdater(item);
		//		item.SellIn = sellInUpdater(item);
		//	}
		//}

		//public static Func<Item, int> GetUpdateQuality(string itemName)
		//{
		//	switch (itemName)
		//	{
		//		case "Sulfuras, Hand of Ragnaros":
		//			return (item) => item.Quality;
		//		case "Aged Brie":
		//			return i => {
		//				var q = UpdateBrieQuality(i);
		//				return ConfirmWithQualityBoundaries(q);
		//			};
		//		case "Backstage passes to a TAFKAL80ETC concert":
		//			return i =>
		//			{
		//				var q = UpdateBackStagePassesQuality(i);
		//				return ConfirmWithQualityBoundaries(q);
		//			};
		//		default:
		//			return i =>
		//			{
		//				var q = UpdateDefaultQuality(i);
		//				return ConfirmWithQualityBoundaries(q);
		//			};
		//	}
		//}

		//public static Func<Item,int> GetUpdateSellIn(string itemName)
		//{
		//	switch (itemName)
		//	{
		//		case "Sulfuras, Hand of Ragnaros":
		//			return item => item.SellIn;
		//		default:
		//			return UpdateSellIn;
		//	}
		//}

		//private static int UpdateSellIn(Item item)
		//{
		//	return item.SellIn - 1;
		//}

		//private static int ConfirmWithQualityBoundaries(int quality)
		//{
		//	if (quality > 50)
		//	{
		//		return 50;
		//	}
		//	if (quality < 0)
		//	{
		//		return 0;
		//	}

		//	return quality;
		//}

		//private static int UpdateDefaultQuality(Item item)
		//{
		//	var decrease = -1;
		//	if (item.SellIn < 1)
		//	{
		//		decrease = - 2;
		//	}

		//	return item.Quality + decrease;
		//}

		//private static int UpdateBackStagePassesQuality(Item item)
		//{
		//	if (item.SellIn < 1)
		//	{
		//		return 0;
		//	}
		//	else
		//	{
		//		var addition = 1;
		//		if (item.SellIn < 11)
		//		{
		//			addition = addition + 1;
		//		}

		//		if (item.SellIn < 6)
		//		{
		//			addition = addition + 1;
		//		}

		//		return item.Quality + addition;
		//	}
		//}

		//private static int UpdateBrieQuality(Item item)
		//{
		//	var addition = 1;

		//	if (item.SellIn < 1)
		//	{
		//		addition = addition + 1;
		//	}

		//	return item.Quality + addition;
		//}

		#endregion Solution direction 1.
	}

	/// <summary>
	/// You are NOT allowed to change this class; here be gremlins!
	/// </summary>
	public class Item
	{
		public string Name { get; set; }

		public int SellIn { get; set; }

		public int Quality { get; set; }
	}
}