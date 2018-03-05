namespace Laba6
{
    class First: СonveyerBase<string>
    {
        public First()
        {
            Name = "1";
            Wait = false;
        }
        protected override string Work(string data)
        {
            int len = data.Length;
            for (int i = len-1; i >=0; i--)
            {
                data += data[i];
            }
            return data;

        }
    }
    
    class Second : СonveyerBase<string>
    {
        public Second()
        {
            Name = "2";
        }

        protected override string Work(string data)
        {
            return data.Substring(0, data.Length/2) + "****";
        }
    }
    
    class Third : СonveyerBase<string>
    {
        public Third()
        {
            Name = "3";
        }

        
        
        protected override string Work(string data)
        {
            string line="";
            for (int i = 0; i < data.Length; i+=2)
            {
                line += data[i];
            }
            return line;

        }
    }
    
}