using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tavis.Tools;

namespace Tavis {
    [TestClass]
    public class LinkTests {

        [TestMethod]
        public void CreateEmptyLink() {
            //Arrange

            //Act
            var link = new Link();

            //Assert
            Assert.IsNotNull(link);

        }






        [TestMethod]
        public void InitializeLinkWithString() {
            //Arrange

            //Act

            var link = new Link() { Target = new Uri("http://example.org/")};

            //Assert
            Assert.AreEqual(link.Target.ToString(), "http://example.org/");

        }


        [TestMethod]
        public void AddParameter() {
            //Arrange
            var link = new Link() { Target = new Uri("http://localtmserver:8700/api/locweldlocal/Assemblies?RootAssembly=true{&workorder}") };
            
            //Act
            link.SetParameter("workorder","W1542");

            var req = link.CreateRequest();
            //Assert
            Assert.AreEqual("http://localtmserver:8700/api/locweldlocal/Assemblies?RootAssembly=true&workorder=W1542".ToLowerInvariant(), req.RequestUri.ToString().ToLowerInvariant());

        }

        

    }
}
