using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ContactSvc.Models;
namespace ContactSvc.Service
{
    public class ContactSvcRepository
    {
        private const string CacheKey = "ContactStore";
        private const string DataMgrKey = "DataManager";
        private const string DbMgrKey = "DBManager";
        private IDataManager m_cdatamgr;
        private IDBManager m_cdbmgr;
        public ContactSvcRepository()
        {

            SetCache();
            
        }
        public void SetCache()
        {
            var ctx = HttpContext.Current;
            string connstring = "Data Source=contactsvc18db.database.windows.net;Initial Catalog=ContactSvc18_db;Integrated Security=False;User ID=vikashviksit;Password=Gyanvi17;Connect Timeout=30;Encrypt=True;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            //string connstringlocal = "Data Source = (localdb)\v11.0; Initial Catalog = AzureStorageEmulatorDb40; Integrated Security = True; Connect Timeout = 30; Encrypt = False; TrustServerCertificate = False; ApplicationIntent = ReadWrite; MultiSubnetFailover = False";
            if (ctx != null)
            {
                if (ctx.Cache[DataMgrKey] == null)
                {
                    ctx.Cache[DataMgrKey] = new ContactDataManager(CacheKey);
                }
                if (ctx.Cache[DbMgrKey] == null)
                {
                    ctx.Cache[DbMgrKey] = new ContactDBManager(connstring); //TBD set connection string;
                }
                m_cdatamgr = (IDataManager)ctx.Cache[DataMgrKey];
                m_cdbmgr = (IDBManager)ctx.Cache[DbMgrKey];
                

            }
            else
            {
                m_cdatamgr = new ContactDataManager(CacheKey);
                m_cdbmgr = new ContactDBManager(connstring); ;                

            }
           
        }

        public Contact[] GetContact()
        {
            try
            {
                var ctx = HttpContext.Current;
                SetCache();
                Contact[] contacts = m_cdatamgr.Get(ref ctx, ref m_cdbmgr);
                if (contacts != null)
                {
                    return contacts;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(m_cdbmgr.GetException(ref ex));               
            }
            return new Contact[]
            {
                new Contact
                {
                    Id = 1,
                    FName = "Monika",
                    LName = "Singh",
                    Phone = "4444",
                    Email = "aaa@aa",
                    Status = estatus.Active,

                }
            };

        }
        public bool SaveContact(Contact contact)
        {
                bool bret = false;
                try
                {
                    var ctx = HttpContext.Current;
                    SetCache();
                    m_cdatamgr.Add(ref ctx, ref m_cdbmgr,ref contact);
                    bret = true;


                }
                catch (Exception ex)
                {
                        Console.WriteLine(m_cdbmgr.GetException(ref ex));
                        bret = false;
                }
                return bret;

        }
        public bool UpdateContact(Contact contact)
        {

            bool bret = false;
            try
            {
                var ctx = HttpContext.Current;
                SetCache();
                m_cdatamgr.Update(ref ctx, ref m_cdbmgr, ref contact);

            }
            catch (Exception ex)
            {
                Console.WriteLine(m_cdbmgr.GetException(ref ex));
                bret = false;
            }
            return bret;
            
         }
        public bool RemoveContact(Contact contact)
        {
            bool bret = false;
            try
            {
                var ctx = HttpContext.Current;
                SetCache();
                m_cdatamgr.Remove(ref ctx, ref m_cdbmgr, ref contact);

            }
            catch (Exception ex)
            {
                Console.WriteLine(m_cdbmgr.GetException(ref ex));
                bret = false;
            }
            return bret;

        }
    }
    
}