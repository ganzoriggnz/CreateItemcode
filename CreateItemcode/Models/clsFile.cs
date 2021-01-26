using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreateItemCode.Models
{
    public class clsFile
    {
        private int id;
        public int ID
        {
            get { return this.id; }
            set { this.id = value; }
        }

        private string filename;
        public string Filename
        {
            get { return this.filename; }
            set { this.filename = value; }
        }

        private int version;
        public int Version
        {
            get { return this.version; }
            set { this.version = value; }
        }

        private int mversion;
        public int Mversion
        {
            get { return this.mversion; }
            set { this.mversion = value; }
        }

        private string tuluv;
        public string Tuluv
        {
            get { return this.tuluv; }
            set { this.tuluv = value; }
        }

        private string createddate;
        public string Createddate
        {
            get { return this.createddate; }
            set { this.createddate = value; }
        }

        private string enddate;
        public string Enddate
        {
            get { return this.enddate; }
            set { this.enddate = value; }
        }

        private string comment;
        public string Comment
        {
            get { return this.comment; }
            set { this.comment = value; }
        }

        private int cuser;
        public int Cuser
        {
            get { return this.cuser; }
            set { this.cuser = value; }
        }

        private string uname;
        public string Uname
        {
            get { return this.uname; }
            set { this.uname = value; }
        }
    }
}
