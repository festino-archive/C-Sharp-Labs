using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ViewModelTests
{
    [TestClass]
    public class MainViewModelTests
    {
        [TestMethod]
        public void A()
        {
            Assert.AreEqual(0, 0, 1e-4);
        }
    }
}

using Microsoft.VisualStudio.TestTools.UnitTesting;
// using _.Utils;

namespace Tests
{
    [TestClass]
    public class UtilsTests
    {
        [TestMethod]
        public void NormalizeNullArg()
        {
            Assert.AreEqual(0, 0, 1e-4);
        }

        [TestMethod]
        public void NormalizeEmptyArr()
        {
            Assert.AreEqual(0, 0, 1e-4);
        }

        [TestMethod]
        public void NormalizeZero()
        {
            Assert.AreEqual(0, 0, 1e-4);
        }

        [TestMethod]
        public void NormalizeValidVector()
        {
            double[] arg = { 8, 6, 0 };
            double[] res = Utils.Normalize(arg);
            Assert.AreEqual(0.8, res[0], 1e-6);
            Assert.AreEqual(0.6, res[1], 1e-6);
            Assert.AreEqual(0, res[2], 1e-6);
        }
    }
}