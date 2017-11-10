
using DB.Tables;

namespace DB.Generators
{
    class CompGenerator:BaseGenerator<Company>
    {
        private readonly string[] _names;
        private readonly string[] _cities;
        private readonly int _nN, _cN;
        public CompGenerator(string path, int nCompanies)
        {
            _names = System.IO.File.ReadAllLines(path + "Company.txt");
            _cities = System.IO.File.ReadAllLines(path + "Cities.txt");
            _nN = _names.Length;
            _cN = _cities.Length;
            for (int i = 0; i < nCompanies; i++)
            {
                Lst.Add(new Company(){CompId = i+1,CompAdminMid = i+1,CompCity = _cities[Rnd.Next(_cN)], CompName = _names[i]});

            }
            
        }
    }
}
