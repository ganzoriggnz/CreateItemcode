using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreateItemCode.Models
{
    public class clsUser
    {
        private int id;
        public int ID
        {
            get { return this.id; }
            set { this.id = value; }
        }

        private string loginName;
        public string LoginName
        {
            get { return this.loginName; }
            set { this.loginName = value; }
        }

        private string uname;
        public string Uname
        {
            get { return this.uname; }
            set { this.uname = value; }
        }

        private string pass;
        public string Pass
        {
            get { return this.pass; }
            set { this.pass = value; }
        }

        private string heltes;
        public string Heltes
        {
            get { return this.heltes; }
            set { this.heltes = value; }
        }

        private string position;
        public string Position
        {
            get { return this.position; }
            set { this.position = value; }
        }

        private bool isActive;
        public bool IsActive
        {
            get { return this.isActive; }
            set { this.isActive = value; }
        }

        private DateTime createdate;
        public DateTime Createdate
        {
            get { return this.createdate; }
            set { this.createdate = value; }
        }

        private string role;
        public string Role
        {
            get { return this.role; }
            set { this.role = value; }
        }

        private string workertype;
        public string Workertype
        {
            get { return this.workertype; }
            set { this.workertype = value; }
        }
    }
}
