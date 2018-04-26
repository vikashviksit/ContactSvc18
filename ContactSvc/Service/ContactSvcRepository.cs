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
        /*public ContactSvcRepository()
        {
            var ctx = HttpContext.Current;
            if (ctx != null)
            {
                return (Contact[])ctx.Cache[CacheKey];
            }
            
        }
         * */

        public Contact[] GetContact()
        {
            /*return new Contact[]
             {
                new Contact
                {
                    Id = 1,
                    FName = "Monika",
                    LName = "Singh",
                    Phone = "4444",
                    Email = "aaa@aa",
                    Status = estatus.online,

                },
                new Contact
                {
                   Id = 2,
                    FName = "Vikash",
                    LName = "Singh",
                    Phone = "3434",
                    Email = "aaa@bb",
                    Status = estatus.online,
                }
             * 

            };*/
            var ctx = HttpContext.Current;

            if (ctx != null)
            {
                return (Contact[])ctx.Cache[CacheKey];
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
            var ctx = HttpContext.Current;

            if (ctx != null)
            {
                try
                {
                    if (ctx.Cache[CacheKey] != null)
                    {
                        var currentData = ((Contact[])ctx.Cache[CacheKey]).ToList();
                        int newId = 1;
                        if (currentData.Count > 0)
                        {
                            Contact[] contactarray = (Contact[])ctx.Cache[CacheKey];
                            newId = contactarray[currentData.Count - 1].Id + 1;
                        }
                        contact.Id = newId;
                        currentData.Add(contact);

                        ctx.Cache[CacheKey] = currentData.ToArray();
                    }
                    else
                    {
                        ctx.Cache[CacheKey] = new Contact[] { contact };
                    }

                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    return false;
                }
            }

            return false;

        }
        public bool UpdateContact(Contact contact)
        {
            var ctx = HttpContext.Current;

            if (ctx != null)
            {
                try
                {
                    if (ctx.Cache[CacheKey] != null)
                    {
                        var currentData = ((Contact[])ctx.Cache[CacheKey]).ToList();
                        Contact[] contactarray = (Contact[])ctx.Cache[CacheKey];
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
                        ctx.Cache[CacheKey] = currentData.ToArray();
                        return true;
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    return false;
                }
            }
            return false;
         }
        public bool RemoveContact(Contact contact)
        {
            var ctx = HttpContext.Current;

            if (ctx != null)
            {
                try
                {
                    if (ctx.Cache[CacheKey] != null)
                    {
                        var currentData = ((Contact[])ctx.Cache[CacheKey]).ToList();                       
                        Contact[] contactarray = (Contact[])ctx.Cache[CacheKey];
                        for (int i = 0; i < currentData.Count; ++i)
                        {
                            if (contactarray[i].Id == contact.Id)
                            {
                                currentData.Remove(contactarray[i]);
                                break;
                            }

                        }
                        ctx.Cache[CacheKey] = currentData.ToArray();
                        return true;
                    }
                    
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    return false;
                }
            }

            return false;

        }
    }
}