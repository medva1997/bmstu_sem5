//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SampleGame.DB
{
    using System;
    using System.Collections.Generic;
    
    public partial class Dictionary
    {
        public string Word { get; set; }
        public string Description { get; set; }
        public Nullable<bool> Approved { get; set; }
        public string Author { get; set; }
        public int LongWord { get; set; }
        public int LongCount { get; set; }
        public Nullable<double> Extra { get; set; }
    }
}
