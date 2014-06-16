using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Formatting;

namespace WebApiClient
{
    class Program
    {
        static void Main(string[] args)
        {
            ApiHelper api = new ApiHelper();

            int newCustomerId=api.NewCustomer("Abc.com");
            Console.WriteLine(string.Format("new Customer Id: {0}", newCustomerId));

            int newCustomerId2 = api.NewCustomer("123.com");
            Console.WriteLine(string.Format("new Customer Id: {0}", newCustomerId2));

            List<Customer> allCustomer = api.AllCustomer();
            Console.WriteLine(string.Format("All Customer total: {0}", allCustomer.Count));

            Customer c1 = api.SingleCustomer(newCustomerId);
            Console.WriteLine(string.Format("Read Customer Id: {0} / {1}", c1.Id, c1.CustName));

            c1.CustName = "Abc.com.tw";
            bool result=api.UpdateCustomer(c1);
            Console.WriteLine(string.Format("Update Customer: {0}", result));

            Customer c1after = api.SingleCustomer(newCustomerId);
            Console.WriteLine(string.Format("After change name: {0}", c1after.CustName));

            bool delResult = api.DeleteSingleCustomer(newCustomerId);
            Console.WriteLine(string.Format("Delete a Customer: {0}", delResult));
            Console.WriteLine(string.Format("All Customer total: {0}", api.AllCustomer().Count));

            bool delAllResult = api.DeleteAllCustomer();
            Console.WriteLine(string.Format("Delete all Customers: {0}", delAllResult));
            Console.WriteLine(string.Format("All Customer total: {0}", api.AllCustomer().Count));
            
            Console.Read();
        }

    }



}
