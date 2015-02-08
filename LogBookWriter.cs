using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GildedRose.Tests
{
	public class LogBookWriter
	{
		private readonly GildedRoseStore store;
		private DateTime today;
		private readonly string file;

		public LogBookWriter(GildedRoseStore store, DateTime today, string file)
		{
			this.store = store;
			this.today = today;
			this.file = file;

			System.IO.File.WriteAllText(file, "");
		}

		public void WriteHeader()
		{
			var sb = new StringBuilder();
			sb.AppendLine("This is the logbook of the GildedRose.");
			AddRuler(sb);

			WriteToDisk(sb.ToString());
		}

		public void AppendDayReport()
		{
			today = today.AddDays(1);
			var sb = new StringBuilder();
			sb.AppendLine();
			sb.AppendLine();
			AddRuler(sb);
			sb.AppendLine(String.Format("Today is {0}, may it bring nothing but good.", today.ToLongDateString()));
			WriteToDisk(sb.ToString());
		}

		public void InventoryReport()
		{
			var sb = new StringBuilder();
			AddRuler(sb);
			sb.AppendLine(String.Format("The current inventory holds {0} items:", store.Items.Count));
			AddRuler(sb);
			foreach (var item in store.Items)
			{
				sb.AppendLine(String.Format("Of quality {0}\t to be sold before {1}\t {2}", item.Quality, item.SellIn, item.Name));
			}
			AddRuler(sb);

			WriteToDisk(sb.ToString());
		}

		private static void AddRuler(StringBuilder sb)
		{
			sb.AppendLine(new String('-', 50));
		}

		private void WriteToDisk(string text)
		{
			System.IO.File.AppendAllText(file, text);
		}
	}
}
