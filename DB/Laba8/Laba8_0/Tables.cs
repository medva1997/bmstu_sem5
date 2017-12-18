using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laba8_0
{
    [Table]
    public  class Company
    {
        [Column]
        public int CompID { get; set; }
        [Column]
        public int CompAdminMID { get; set; }
        [Column]
        public string CompName { get; set; }
        [Column]
        public string CompCity { get; set; }
        
    }

    [Table]
    public class M_BUSY
    {
        [Column]
        public int MID { get; set; }
        [Column]
        public System.DateTime Time { get; set; }
        [Column]
        public Nullable<bool> Status { get; set; }

    }
    [Table]
    public  class Manager
    {
        [Column(IsPrimaryKey = true)]
        public int MID { get; set; }
        [Column]
        public int CompID { get; set; }
        [Column]
        public string LastName { get; set; }
        [Column]
        public string FirstName { get; set; }
        [Column]
        public string SecondName { get; set; }
        [Column]
        public string Email { get; set; }
        [Column]
        public string Password { get; set; }
        [Column]
        public string PhoneNumber { get; set; }
        [Column]
        public Nullable<int> AdminID { get; set; }
       
    }
    [Table]
    public  class Meeting
    {
        [Column(IsPrimaryKey = true)]
        public int Meet_ID { get; set; }
        [Column]
        public int MID { get; set; }
        [Column]
        public int VID { get; set; }
        [Column]
        public System.DateTime TimeStart { get; set; }
        [Column]
        public System.DateTime TimeEnd { get; set; }
        [Column]
        public Nullable<bool> M_Confirmation { get; set; }
        [Column]
        public Nullable<bool> V_confirmation { get; set; }
        [Column]
        public string Location { get; set; }

       
    }
    [Table]
    public  class V_BUSY
    {
        [Column]
        public int VID { get; set; }
        [Column]
        public System.DateTime Time { get; set; }
        [Column]
        public Nullable<bool> Status { get; set; }
    }
    [Table(Name = "Visitor")]
    public  class Visitor
    {
        [Column(IsPrimaryKey = true)]
        public int VID { get; set; }
        [Column]
        public string LastName { get; set; }
        [Column]
        public string FirstName { get; set; }
        [Column]
        public string SecondName { get; set; }
        [Column]
        public string Email { get; set; }
        [Column]
        public string Password { get; set; }
        [Column]
        public string PhoneNumber { get; set; }

    }

}
