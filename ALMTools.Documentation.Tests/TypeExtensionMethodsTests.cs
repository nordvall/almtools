using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ALMTools.Documentation.Tests
{
    [TestClass]
    public class TypeExtensionMethodsTests
    {
        [TestMethod]
        public void GetFriendlyTypeName_WhenCalledOnSimpleType_CorrectNameIsReturned()
        {
            Type type = typeof(Guid);
            string result = type.GetFriendlyTypeName();

            Assert.AreEqual("Guid", result);
        }

        [TestMethod]
        public void GetFriendlyTypeName_WhenCalledOnGenericList_CorrectNameIsReturned()
        {
            Type type = typeof(List<Guid>);
            string result = type.GetFriendlyTypeName();

            Assert.AreEqual("List<Guid>", result);
        }
    }
}
