using GildedRose;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunningTheShop
{
	class Program
	{
		static void Main(string[] args)
		{
			var store = new GildedRoseStore();

			while (true)
			{
				DisplayStoreContent(store);
				var readLine = Console.ReadLine();

				if (readLine != null && (readLine.Contains('q') || readLine.Contains('Q')))
				{
					Console.WriteLine("Thank you for your visit, goodbye.");
					return;
				}
				else
				{
					Console.WriteLine();
					Console.WriteLine("A day has passed, the inventory has changed.");
					store.UpdateQuality();
				}
			}
		}

		private static void DisplayStoreContent(GildedRoseStore store)
		{
			Console.WriteLine();
			Console.WriteLine("The current inventory holds {0} items:", store.Items.Count);
			Console.WriteLine(new String('-', 50));
			foreach(var item in store.Items)
			{
				Console.WriteLine("Of quality {0}\t to be sold before {1}\t {2}", item.Quality, item.SellIn, item.Name);
			}
			Console.WriteLine(new String('-', 50));
		}
	}
}
