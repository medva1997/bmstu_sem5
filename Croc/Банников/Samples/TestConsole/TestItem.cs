using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestConsole
{
    public class TestItem
    {
        public string LongWord;
        public string ShortWord;
        public bool Result;

        public TestItem(string sw, string lw, bool res)
        {
            ShortWord = sw;
            LongWord = lw;
            Result = res;
        }

        /*
         *     Assert.IsTrue(Worker.CouldBuild("КОК", "КОКИЛЬ"));
            Assert.IsFalse(Worker.CouldBuild( "КОКИЛЬ", "КОК"));
            Assert.IsFalse(Worker.CouldBuild("КОК", "ОКО"));
            Assert.IsFalse(Worker.CouldBuild("ОКО", "КОК"));
            Assert.IsTrue(Worker.CouldBuild("ИГРА", "ИРГА"));
            Assert.IsTrue(Worker.CouldBuild("КОЛ", "КОЛ"));
            Assert.IsTrue(Worker.CouldBuild("КОЛОКОЛ", "КОЛОКОЛ"));
            Assert.IsTrue(Worker.CouldBuild(null, "СЛОВО"));
            Assert.IsFalse(Worker.CouldBuild( "СЛОВО", null));
         */
         /// <summary>
         /// Данные для тестирования
         /// </summary>
         /// <returns></returns>
        public static List<TestItem> GetData()
        {
            var list = new List<TestItem>();
            list.Add(new TestItem("КОК", "КОКИЛЬ", true));
            list.Add(new TestItem("КОКИЛЬ", "КОК", false));
            list.Add(new TestItem("КОК", "ОКО", false));
            list.Add(new TestItem("ОКО", "КОК", false));
            list.Add(new TestItem("ИГРА", "ИРГА", true));
            list.Add(new TestItem(null, "ИРГА", true));
            list.Add(new TestItem("ЧТОТО", null, false));
            return list;
        }
    }
}
