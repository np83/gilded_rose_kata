using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GildedRose.Tests
{
	[TestFixture]
	public class LogBookCreation
	{
		[Test]
		public void WriteLogBook()
		{
			var fileName = @"..\..\logbook.txt";
			var contentBefore = System.IO.File.ReadAllText(fileName);
			System.IO.File.Copy(fileName, @"..\..\logbook_before.txt", true);

			var store = new GildedRoseStore();

			var logWriter = new LogBookWriter(store, new DateTime(1812,3, 2), fileName);

			logWriter.WriteHeader();
			logWriter.InventoryReport();

			// Pass the days.
			for (int i = 0; i < 50; i++)
			{
				store.UpdateQuality();
				logWriter.AppendDayReport();
				logWriter.InventoryReport();
			}

			// Check if the report is still the same.
			var contentAfter = System.IO.File.ReadAllText(fileName);
			Assert.That(contentBefore, Is.EqualTo(contentAfter));
		}
	}
}
