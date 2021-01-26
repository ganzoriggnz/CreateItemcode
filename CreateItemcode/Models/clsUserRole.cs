using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreateItemCode.Models
{
    public class clsUserRole
    {
        private int id;
        public int ID
        {
            get { return this.id; }
            set { this.id = value; }
        }

        private int userid;
        public int Userid
        {
            get { return this.userid; }
            set { this.userid = value; }
        }

        private string buleg;
        public string Buleg
        {
            get { return this.buleg; }
            set { this.buleg = value; }
        }

        private string bulegname;
        public string Bulegname
        {
            get { return this.bulegname; }
            set { this.bulegname = value; }
        }
    }
}
