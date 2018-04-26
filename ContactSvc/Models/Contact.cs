using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ContactSvc.Models
{
    public enum estatus
    {
        Inactive = 0,
        Active
    };
    public enum eoperation
    {
        create = 0,
        modify,
        delete
    };
    public class Contact
    {
        ////public:
        //Contact(string name, string uid){
        //    m_name = name;
        //    m_uid = uid;
        //}
        ////private:
        private string fname;
        private string lname;
        private string email;
        private string phone;
        private estatus status;
        private int id;
        private eoperation eops;

        public int Id
        {
            get
            {
                return id;
            }
            set
            {
                id = value;

            }
        }
        public string FName
        {
            get
            {
                return fname;
            }
            set
            {
                fname = value;
            }
        }
        public string LName
        {
            get
            {
                return lname;
            }
            set
            {
                lname = value;
            }
        }
        public string Email
        {
            get
            {
                return email;
            }
            set
            {
                email = value;
            }
        }
        public string Phone
        {
            get
            {
                return phone;
            }
            set
            {
                phone = value;
            }
        }
        public estatus Status
        {
            get
            {
                return status;
            }
            set
            {
                status = value;
            }
        }

        public eoperation Eops
        {
            get
            {
                return eops;
            }
            set
            {
                eops = value;
            }
        }
        
    }
    }

