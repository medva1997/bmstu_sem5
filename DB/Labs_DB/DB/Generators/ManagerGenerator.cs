
using DB.Tables;

namespace DB.Generators
{
    class ManagerGenerator:  BaseGenerator<Manager>
    {
        public ManagerGenerator(int nUsers, string path, int nCompanies)
        {
            UserGenerator gen = new UserGenerator(path, nUsers);
            for (int i = 0; i < nUsers; i++)
            {
                if (i<nCompanies)
                {
                    Lst.Add(new Manager() { Mid = i + 1, CompId = i + 1, User = gen.GetList[i] });
                    continue;
                    
                }                
                Lst.Add(new Manager() { Mid = i + 1, CompId = Rnd.Next(nCompanies)+1,User = gen.GetList[i] });
            }
        }
    }
}
