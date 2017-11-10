using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DB.Tables;

namespace DB.Generators
{
    class VisitorGenerator:BaseGenerator<Visitor>
    {
        public VisitorGenerator(int nUsers,string path)
        {
            UserGenerator gen= new UserGenerator(path,nUsers);
            for (int i = 0; i < nUsers; i++)
            {
                Lst.Add(new Visitor(){Vid = i+1,User = gen.GetList[i]});
            }
        }
    }
}
