using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using ContactSvc.Models;
using ContactSvc.Service;
namespace ContactSvc.Controllers
{
    public class ContactController : ApiController
    {
        private ContactSvcRepository contactsvcrepo;
        public ContactController()
        {
            this.contactsvcrepo = new ContactSvcRepository();
        }
        public Contact[] Get()
        {
            //define a class dervied from Svc interface whose method would provide the service
            /*return new Contact[]
             {
                new Contact
                {
                    Id = "1",
                    Name = "Glenn Block"
                },
                new Contact
                {
                    Id = "2",
                    Name = "Dan Roth"
                }

            };
             * */

             
             return contactsvcrepo.GetContact();
        }
        public HttpResponseMessage Post(Contact contact)
        {

            HttpResponseMessage response = null;
            switch (contact.Eops)
            {
                case eoperation.create:
                    {
                        this.contactsvcrepo.SaveContact(contact);
                        response = Request.CreateResponse<Contact>(System.Net.HttpStatusCode.Created, contact);
                        break;
                    }
                case eoperation.modify:
                    {
                        response = Put(contact);
                        break;
                    }
                case eoperation.delete:
                    {
                        response = Delete(contact);
                        break;
                    }
            }
            return response;

        }
        public HttpResponseMessage Put(Contact contact)
        {

            this.contactsvcrepo.UpdateContact(contact);



            var response = Request.CreateResponse<Contact>(System.Net.HttpStatusCode.Created, contact);



            return response;

        }
        public HttpResponseMessage Delete(Contact contact)
        {

            this.contactsvcrepo.RemoveContact(contact);



            var response = Request.CreateResponse<Contact>(System.Net.HttpStatusCode.Created, contact);



            return response;

        }
    }
}
