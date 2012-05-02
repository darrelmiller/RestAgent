using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tavis {
    [TestClass]
    public class SemanticRegistryTest {
        [TestMethod]
        public void RegisterAndRetrieveLink() {
            
            // Arrange
            var registry = new SemanticsRegistry();
            registry.RegisterLinkType<Link>("foo");

            //Act
            var link = registry.CreateLink("foo");
            
            //Assert
            Assert.IsNotNull(link);
        }

        [TestMethod]
        public void RegisterAndRetrieveDerivedLink() {

            // Arrange
            var registry = new SemanticsRegistry();
            registry.RegisterLinkType<FooLink>("foo");

            //Act
            var link = registry.CreateLink("foo");

            //Assert
            Assert.IsNotNull(link);
            Assert.IsInstanceOfType(link,typeof(FooLink));
        }

    }

    public class FooLink : Link {

        public FooLink() {
            
        }
    }
}
