using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;


[Serializable]
[Microsoft.SqlServer.Server.SqlUserDefinedType(Format.Native)]
public struct passport: INullable
{
    public Int32 seria;
    public Int32 number;
   

    public override string ToString()
    {
      
        return seria+" "+number;
    }
    
    public bool IsNull => _null;

    public static passport Null => new passport {_null = true};

    public static passport Parse(SqlString s)
    {
        if (s.IsNull)
            return Null;

        passport u = new passport();
        string[] arr = s.Value.Split(' ');
        u.seria = Convert.ToInt32(arr[0]);
        u.number = Convert.ToInt32(arr[1]);
        //throw  new Exception(u.ToString());
      
       
        return u;
    }

    public int Seria => seria;

    public int Number => number;


    //  Private member
    private bool _null;
}