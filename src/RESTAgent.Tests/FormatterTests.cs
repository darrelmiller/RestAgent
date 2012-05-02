using System.Net.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tavis.Tools;

namespace Tavis
{
	[TestClass]
	public class FormatterTests
	{
		[TestMethod]
		public void TestPlainTextFormatterCanBeUsedInIsolation()
		{
			// Arrange
			var content = new StringContent("Great galloping rhinoceroses!");
			
			// Act
		    var objectContent = new SimpleObjectContent<string>(content,new PlainTextFormatter());
		    var result = objectContent.ReadAsync().Result;

			// Assert
			Assert.AreEqual("Great galloping rhinoceroses!", result);
		}
	}
}
