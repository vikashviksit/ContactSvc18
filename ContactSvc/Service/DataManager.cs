﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContactSvc.Models;
using System.Data.SqlClient;
using System.Threading;
using System.Web;
namespace ContactSvc.Service
{
    interface IDBManager
    {

        void Add(ref Contact c);
        void Update(ref Contact c);
        void Remove(ref Contact c);
        Contact[] Get();
        string GetException(ref Exception parent_ex);
    }
    class ContactDBManager : IDBManager
    {
        private string m_connstring;
        private string m_error_message;

        public ContactDBManager(string connString)
        {
            //m_connstring = connString;
            m_connstring = connString;
        }
        //Client calling this class method will handle exception
        public void Add(ref Contact c)
        {
            try
            {
                if (c.Id == 0)
                {
                    c.Id = GenerateNewId();
                }
                SqlConnection sqlconn = new SqlConnection(m_connstring);//TBD can have a SQL connection pool with thread safe lock 
                                                                        //to get from pool and mark it as being used and return it instead of closing it

                string sqlins = "INSERT INTO ContactTable (Id,FName, LName, Phone,Email,Status) VALUES (@id, @fname, @lname, @phone,@email,@status)";
                SqlCommand command = new SqlCommand();
                command.Connection = sqlconn;
                command.Connection.Open();
                command.CommandText = sqlins;
                
                SqlParameter idparam = new SqlParameter("@id", c.Id);
                SqlParameter fnparam = new SqlParameter("@fname", c.FName);
                SqlParameter lnparam = new SqlParameter("@lname", c.LName);
                SqlParameter phoneparam = new SqlParameter("@phone", c.Phone);
                SqlParameter emailparam = new SqlParameter("@email", c.Email);
                SqlParameter statusparam = new SqlParameter("@status", c.Status);

                command.Parameters.AddRange(new SqlParameter[] { idparam, fnparam, lnparam, phoneparam, emailparam, statusparam });
                command.ExecuteNonQuery();
                command.Connection.Close();

            }
            catch (Exception ex)
            {
                m_error_message = ex.Message.ToString();
                throw;
            }

        }
        public void Update(ref Contact c)
        {
            try
            {
                SqlConnection sqlconn = new SqlConnection(m_connstring);//TBD can have a SQL connection pool with thread safe lock 
                                                                        //to get from pool and mark it as being used and return it instead of closing it

                string sqlupdt = "Update ContactTable Set FName=@fname, LName=@lname, Phone=@phone,Email=@email,Status=@status where Id=@id";
                SqlCommand command = new SqlCommand();
                command.Connection = sqlconn;
                command.Connection.Open();
                command.CommandText = sqlupdt;

                SqlParameter idparam = new SqlParameter("@id", c.Id);
                SqlParameter fnparam = new SqlParameter("@fname", c.FName);
                SqlParameter lnparam = new SqlParameter("@lname", c.LName);
                SqlParameter phoneparam = new SqlParameter("@phone", c.Phone);
                SqlParameter emailparam = new SqlParameter("@email", c.Email);
                SqlParameter statusparam = new SqlParameter("@status", c.Status);

                command.Parameters.AddRange(new SqlParameter[] { idparam, fnparam, lnparam, phoneparam, emailparam, statusparam });
                command.ExecuteNonQuery();
                command.Connection.Close();
            }
            catch (Exception ex)
            {
                m_error_message = ex.Message.ToString();
                throw;
            }

        }


        public void Remove(ref Contact c)
        {
            try
            {
                SqlConnection sqlconn = new SqlConnection(m_connstring);//TBD can have a SQL connection pool with thread safe lock 
                                                                        //to get from pool and mark it as being used and return it instead of closing it

                string sqlupdt = "Delete from ContactTable where Id=@id";
                SqlCommand command = new SqlCommand();
                command.Connection = sqlconn;
                command.Connection.Open();
                command.CommandText = sqlupdt;

                SqlParameter idparam = new SqlParameter("@id", c.Id);

                command.Parameters.AddRange(new SqlParameter[] { idparam });
                command.ExecuteNonQuery();
                command.Connection.Close();
            }
            catch (Exception ex)
            {
                m_error_message = ex.Message.ToString();
                throw;
            }
        }

        public Contact[] Get()
        {
            try
            {
                SqlDataReader reader = null;
                SqlConnection sqlconn = new SqlConnection(m_connstring);//TBD can have a SQL connection pool with thread safe lock 
                                                                        //to get from pool and mark it as being used and return it instead of closing it

                string sqlselect = "Select * from ContactTable";
                SqlCommand command = new SqlCommand();
                command.Connection = sqlconn;
                command.Connection.Open();
                command.CommandText = sqlselect;
                List<Contact> listContact = new List<Contact>();
                Contact c = null;
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    c = new Contact();
                    c.Id = Convert.ToInt32(reader.GetValue(0));
                    c.FName = reader.GetValue(1).ToString();
                    c.LName = reader.GetValue(2).ToString();
                    c.Phone = reader.GetValue(3).ToString();
                    c.Email = reader.GetValue(4).ToString();
                    c.Status = (estatus)Convert.ToInt32(reader.GetValue(5));
                    listContact.Add(c);
                }
                sqlconn.Close();
                return listContact.ToArray();
            }
            catch (Exception ex)
            {
                m_error_message = ex.Message.ToString();
                throw;
            }
        }
        public string GetException(ref Exception ex)
        {
            if(ex != null)
            {
                m_error_message += "Parent Exception message:{" + ex.Message + "}";
            }
            return m_error_message.ToString();
        }
        public int GenerateNewId()
        {
            int nId = 0;
            try
            {
                SqlDataReader reader = null;
                SqlConnection sqlconn = new SqlConnection(m_connstring);//TBD can have a SQL connection pool with thread safe lock 
                                                                        //to get from pool and mark it as being used and return it instead of closing it

                string sqlselect = "SELECT  MAX(Id) As ID from ContactTable";
                SqlCommand command = new SqlCommand();
                command.Connection = sqlconn;
                command.Connection.Open();
                command.CommandText = sqlselect;               
                reader = command.ExecuteReader();
                while (reader.Read())
                {                   
                    nId = Convert.ToInt32(reader.GetValue(0));                    
                }
                sqlconn.Close();
                
            }
            catch (Exception ex)
            {
                m_error_message = ex.Message.ToString();
                throw;
            }
            return nId == 0 ? nId = 1 : nId;
        }
    }

    /*interface ICacheManager
    {
        
        Contact[] Cache
        {
            get;
            set;
           
        }
        bool Add(ref ICacheManager cache,ref IDBManager dbmgr);
        bool Update(ref ICacheManager cache, ref IDBManager dbmgr);
        bool Remove(ref ICacheManager cache, ref IDBManager dbmgr);

        Contact[] Get();
    }
    */
    interface IDataManager
    {
       
        void Add(ref HttpContext ctxcache, ref IDBManager dbmgr, ref Contact contact);
        void Update(ref HttpContext ctxcache, ref IDBManager dbmgr, ref Contact contact);
        void Remove(ref HttpContext ctxcache, ref IDBManager dbmgr, ref Contact contact);
        Contact[] Get(ref HttpContext ctxcache, ref IDBManager dbmgr);
    }


    class ContactDataManager : IDataManager
    {


        private ReaderWriterLockSlim m_Rwlock;
        private string m_cachekey;
        

        public ContactDataManager(string cachekey)
        {
            m_cachekey = cachekey;
            m_Rwlock = new ReaderWriterLockSlim();

        }

        public void Add(ref HttpContext cctx, ref IDBManager dbmgr,ref Contact contact)
        {
                       
            try
            {
                m_Rwlock.EnterWriteLock();
                int newId = 0;
                if (m_cachekey != "" && cctx != null && cctx.Cache[m_cachekey] != null)
                {
                    var currentData = ((Contact[])cctx.Cache[m_cachekey]).ToList();
                    newId = 1;
                    if (currentData.Count > 0)
                    {
                        Contact[] contactarray = (Contact[])cctx.Cache[m_cachekey];
                        newId = contactarray[currentData.Count - 1].Id + 1;
                    }
                    contact.Id = newId;
                    currentData.Add(contact);
                    cctx.Cache[m_cachekey] = currentData.ToArray();
                }                
                dbmgr.Add(ref contact);

            }                       
            finally
            {
                m_Rwlock.ExitWriteLock();
            }
            //catch (Exception ex)
            //{
            //    dbmgr.GetException(ref ex);
            //}          
            
        }
        public void Update(ref HttpContext cctx, ref IDBManager dbmgr, ref Contact contact)
        {
            try
            {
                m_Rwlock.EnterWriteLock();
                if (m_cachekey != "" && cctx != null && cctx.Cache[m_cachekey] != null)
                {
                    var currentData = ((Contact[])cctx.Cache[m_cachekey]).ToList();
                    Contact[] contactarray = (Contact[])cctx.Cache[m_cachekey];
                    for (int i = 0; i < currentData.Count; ++i)
                    {
                        if (contactarray[i].Id == contact.Id)
                        {
                            if (contactarray[i].FName != contact.FName)
                            {
                                contactarray[i].FName = contact.FName;
                            }
                            if (contactarray[i].LName != contact.LName)
                            {
                                contactarray[i].LName = contact.LName;
                            }
                            if (contactarray[i].Phone != contact.Phone)
                            {
                                contactarray[i].Phone = contact.Phone;
                            }
                            if (contactarray[i].Email != contact.Email)
                            {
                                contactarray[i].Email = contact.Email;
                            }
                            if (contactarray[i].Status != contact.Status)
                            {
                                contactarray[i].Status = contact.Status;
                            }
                            break;
                        }

                    }
                    cctx.Cache[m_cachekey] = currentData.ToArray();
                }
                dbmgr.Update(ref contact);
            }
            finally
            {
                m_Rwlock.ExitWriteLock();
            }

        }
        public void Remove(ref HttpContext cctx, ref IDBManager dbmgr, ref Contact contact)
        {
            try
            {
                m_Rwlock.EnterWriteLock();
                if (m_cachekey != "" && cctx != null && cctx.Cache[m_cachekey] != null)
                {
                    var currentData = ((Contact[])cctx.Cache[m_cachekey]).ToList();
                    currentData.Remove(contact);
                    cctx.Cache[m_cachekey] = currentData.ToArray();
                }
                dbmgr.Remove(ref contact);
            }
            finally
            {
                m_Rwlock.ExitWriteLock();
            }
        }

        public Contact[] Get(ref HttpContext cctx, ref IDBManager dbmgr)
        {

            try
            {
                m_Rwlock.EnterReadLock();
                Contact[] contacts = null;
                /*                
                if (m_cachekey != "" && cctx != null && cctx.Cache[m_cachekey] != null)
                {
                    contacts = (Contact[])cctx.Cache[m_cachekey];
                    if(contacts != null)
                    {
                        return contacts;
                    }
                }
                */
                //Update cache if its empty;
                contacts = dbmgr.Get();
                //Write to cache
                if (cctx != null)
                {
                    cctx.Cache[m_cachekey] = contacts;
                }
                
                return contacts;

            }
            finally
            {
                m_Rwlock.ExitReadLock();
               
            }

            //catch (Exception ex)
            //{

            //    dbmgr.GetException(ref ex);
            //}

            //return null;

        }
    }

}
