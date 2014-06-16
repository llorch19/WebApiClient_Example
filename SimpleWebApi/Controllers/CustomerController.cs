using SimpleWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SimpleWebApi.Controllers
{
    public class CustomerController : ApiController
    {
        public static List<Customer> ALL_Customer = null;
        public CustomerController()
        {
            if (ALL_Customer == null)
            {
                ALL_Customer = new List<Customer>();
                ALL_Customer.Add(new Customer() { Id = 0, CustName = "TEST.com" });
            }
        }


        // GET api/<controller>
        //http://localhost:2684/Api/Customer/
        public IEnumerable<Customer> Get()
        {
            return ALL_Customer;
        }

         //GET api/<controller>/5
        //http://localhost:2684/Api/Customer/5
        public Customer Get(int id)
        {
            return ALL_Customer.FirstOrDefault(c => c.Id == id);
        }

        // POST api/<controller>
        public int Post([FromBody]Customer value)
        {
            int newId=ALL_Customer.Count+1;
            ALL_Customer.Add(new Customer() { Id = newId, CustName = value.CustName });
            return newId;
        }

        // PUT api/<controller>/5
        public bool Put(int id, Customer value)
        {
            Customer customer = ALL_Customer.FirstOrDefault(c => c.Id == id);
            if (customer == null)
                return false;
            customer.CustName = value.CustName;
            return true;
        }

        // DELETE api/<controller>/5
        public bool Delete(int id)
        {
            Customer customer = ALL_Customer.FirstOrDefault(c => c.Id == id);
            if (customer == null)
                return false;
            ALL_Customer.Remove(customer);
            return true;
        }

        public void Delete()
        {
            ALL_Customer.RemoveAll(r=>r.Id>=0);
        }
    }
}