using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreateItemCode.Models
{
    public class clsInfo
    {
        private int id;
        public int ID
        {
            get { return this.id; }
            set { this.id = value; }
        }

        private string code;
        public string Code
        {
            get { return this.code; }
            set { this.code = value; }
        }

        private string name;
        public string Name
        {
            get { return this.name; }
            set { this.name = value; }
        }

        
        public string CodeName
        {
            get { return this.code + " - " + name; }
        }

        private string nameen;
        public string Nameen
        {
            get { return this.nameen; }
            set { this.nameen = value; }
        }

        private string l_name;
        public string L_name
        {
            get { return this.l_name; }
            set { this.l_name = value; }
        }

        private string angilal;
        public string Angilal
        {
            get { return this.angilal; }
            set { this.angilal = value; }
        }

        private int sortid;
        public int Sortid
        {
            get { return this.sortid; }
            set { this.sortid = value; }
        }

        private string other1;
        public string Other1
        {
            get { return this.other1; }
            set { this.other1 = value; }
        }

        private string other2;
        public string Other2
        {
            get { return this.other2; }
            set { this.other2 = value; }
        }

        private string comment;
        public string Comment
        {
            get { return this.comment; }
            set { this.comment = value; }
        }

        private DateTime cdate;
        public DateTime Cdate
        {
            get { return this.cdate; }
            set { this.cdate = value; }
        }

        private int version;
        public int Version
        {
            get { return this.version; }
            set { this.version = value; }
        }

        private DateTime vdate;
        public DateTime Vdate
        {
            get { return this.vdate; }
            set { this.vdate = value; }
        }

        private string omnocode;
        public string Omnocode
        {
            get { return this.omnocode; }
            set { this.omnocode = value; }
        }

        private string gobicode;
        public string Gobicode
        {
            get { return this.gobicode; }
            set { this.gobicode = value; }
        }

        private string goyocode;
        public string Goyocode
        {
            get { return this.goyocode; }
            set { this.goyocode = value; }
        }

        private string onlinenum;
        public string Onlinenum
        {
            get { return this.onlinenum; }
            set { this.onlinenum = value; }
        }

        private string code26;
        public string Code26
        {
            get { return this.code26; }
            set { this.code26 = value; }
        }

        private string createdU;
        public string CreatedU
        {
            get { return this.createdU; }
            set { this.createdU = value; }
        }

        private string editU;
        public string EditU
        {
            get { return this.editU; }
            set { this.editU = value; }
        }

        private string edate;
        public string Edate
        {
            get { return this.edate; }
            set { this.edate = value; }
        }

    }
}
