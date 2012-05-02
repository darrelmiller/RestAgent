using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace Tavis.Tools {
    [TestClass]
    public class LinkHistoryTests {

        [TestMethod]
        public void AddLinkToHistory() {
            //Arrange
            var linkHistory = new LinkHistory();

            //Act
            linkHistory.AddToHistory(new Link() { Target = new Uri("http://example.org/")});

            //Assert
            Assert.AreEqual("http://example.org/", linkHistory.CurrentLocation.Target.AbsoluteUri);
        }

        [TestMethod]
        public void SupportsMultipleLinks() {
            //Arrange
            var linkHistory = new LinkHistory();
            linkHistory.AddToHistory(new Link() { Target = new Uri("http://example.org/")});
            

            //Act
            linkHistory.AddToHistory(new Link() { Target = new Uri("http://example.org/foo")});

            //Assert
            Assert.AreEqual("http://example.org/foo", linkHistory.CurrentLocation.Target.AbsoluteUri);
        }

        [TestMethod]
        public void GoBackRemovesFromHistory() {
            //Arrange
            var linkHistory = new LinkHistory();
            linkHistory.AddToHistory(new Link() { Target = new Uri("http://example.org/")});
            linkHistory.AddToHistory(new Link() { Target = new Uri("http://example.org/foo")});

            //Act
            linkHistory.GoBack();

            //Assert
            Assert.AreEqual("http://example.org/", linkHistory.CurrentLocation.Target.AbsoluteUri);
        }

        [TestMethod]
        public void SupportsOnly10Links() {
            //Arrange
            var linkHistory = new LinkHistory();

            //Act
            linkHistory.AddToHistory(new Link() { Target = new Uri("http://example.org/1")});
            linkHistory.AddToHistory(new Link() { Target = new Uri("http://example.org/2")});
            linkHistory.AddToHistory(new Link() { Target = new Uri("http://example.org/3")});
            linkHistory.AddToHistory(new Link() { Target = new Uri("http://example.org/4")});
            linkHistory.AddToHistory(new Link() { Target = new Uri("http://example.org/5")});
            linkHistory.AddToHistory(new Link() { Target = new Uri("http://example.org/6")});
            linkHistory.AddToHistory(new Link() { Target = new Uri("http://example.org/7")});
            linkHistory.AddToHistory(new Link() { Target = new Uri("http://example.org/8")});
            linkHistory.AddToHistory(new Link() { Target = new Uri("http://example.org/9")});
            linkHistory.AddToHistory(new Link() { Target = new Uri("http://example.org/10")});
            linkHistory.AddToHistory(new Link() { Target = new Uri("http://example.org/11")});
            linkHistory.AddToHistory(new Link() { Target = new Uri("http://example.org/12")});
            linkHistory.AddToHistory(new Link() { Target = new Uri("http://example.org/13")});
            

            //Assert
            Assert.AreEqual( 10, linkHistory.Count());
            Assert.AreEqual("http://example.org/13", linkHistory.CurrentLocation.Target.AbsoluteUri);
        }
    
    }

}
