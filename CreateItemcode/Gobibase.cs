using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Configuration;
using CreateItemCode.Models;

namespace CreateItemCode
{
    public partial class Gobibase : DataContext
    {
        static string conn = CFunctions.ConnectionStr;
        public Gobibase() : base(conn) { }
    }
}
