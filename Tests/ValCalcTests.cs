using Microsoft.VisualStudio.TestTools.UnitTesting;
using Uzdevums.Controllers;

namespace UzdevumsTests
{
    [TestClass]
    public class ValCalcTests
    {
        [TestMethod]
        public void TestVatCalculation()
        {
            decimal TotalPrice = ProductController.CalcPriceWithVat(1, 1, 21);
            Assert.AreEqual(1.21M, TotalPrice);
        }
    }
}
