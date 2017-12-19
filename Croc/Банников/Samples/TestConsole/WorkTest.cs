using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit;
using NUnit.Framework;

namespace TestConsole
{

    [TestFixture(Author = "Банников С.Н.", Category ="Учебный", Description ="Учебные тесты")]
    public class WorkTest
    {
        [Test()]
        public void Test1()
        {
            Assert.AreEqual(2, Worker.Division(4, 2));
        }

        [Test()]
        public void Test2()
        {
            Assert.IsTrue(Worker.CouldBuild("КОК", "КОКИЛЬ"));
            Assert.IsFalse(Worker.CouldBuild( "КОКИЛЬ", "КОК"));
            Assert.IsFalse(Worker.CouldBuild("КОК", "ОКО"));
            Assert.IsFalse(Worker.CouldBuild("ОКО", "КОК"));
            Assert.IsTrue(Worker.CouldBuild("ИГРА", "ИРГА"));
            Assert.IsTrue(Worker.CouldBuild("КОЛ", "КОЛ"));
            Assert.IsTrue(Worker.CouldBuild("КОЛОКОЛ", "КОЛОКОЛ"));
            Assert.IsTrue(Worker.CouldBuild(null, "СЛОВО"));
            Assert.IsFalse(Worker.CouldBuild( "СЛОВО", null));
        }

        [Test()]
        [TestCase(4)]
        [TestCase(9)]
        [TestCase(16)]
        [TestCase(0)]
        [TestCase(-1)]
        public void Test3(double x)
        {
            Assume.That(x >= 0);
            Assert.AreEqual(Math.Sqrt(x), Worker.Sqrt(x));
        }

        [Test()]
        [TestCaseSource(typeof (TestItem), "GetData" )]        
        public void Test4(TestItem item)
        {
            string s = string.Format("Тут ошибка: {0} {1}", item.ShortWord ?? "<NULL>", item.LongWord ?? "<NULL>");
            if (item.Result)
            {
                Assert.IsTrue(Worker.CouldBuild(item.ShortWord, item.LongWord), s);
            }
            else
            {
                Assert.IsFalse(Worker.CouldBuild(item.ShortWord, item.LongWord), s);
            }
        }
    }
}

