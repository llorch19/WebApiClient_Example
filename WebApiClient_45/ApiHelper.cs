using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WebApiClient
{
    public class ApiHelper
    {

        private string m_BaseAddress = "http://localhost:2684";
        private string api_URL = "api/Customer";
        public HttpClient GetHttpClient()
        {
            return new HttpClient() { BaseAddress = new Uri(m_BaseAddress) };
        }

        /// <summary>
        /// 使用 POST 新增資料
        /// </summary>
        /// <returns></returns>
        public int NewCustomer(string custName)
        {
            Customer customer = new Customer() { CustName = custName };
            using (var client = GetHttpClient())
            {
                var result = client.PostAsJsonAsync(api_URL, customer).Result;
                try
                {
                    result.EnsureSuccessStatusCode();
                    return result.Content.ReadAsAsync<int>().Result;
                }
                catch
                {
                    return -1;
                }
            }
        }

        /// <summary>
        /// 使用 GET 讀取單一筆資料
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public Customer SingleCustomer(int Id)
        {
            using (var client = GetHttpClient())
            {
                var result = client.GetAsync(api_URL + "/" + Id).Result;
                try
                {
                    result.EnsureSuccessStatusCode();
                    return result.Content.ReadAsAsync<Customer>().Result;
                }
                catch
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// 使用 GET 讀取所有資料
        /// </summary>
        /// <returns></returns>
        public List<Customer> AllCustomer()
        {
            using (var client = GetHttpClient())
            {
                var result = client.GetAsync(api_URL).Result;
                try
                {
                    result.EnsureSuccessStatusCode();
                    return result.Content.ReadAsAsync<List<Customer>>().Result;
                }
                catch
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// 使用 PUT 修改資料
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        public bool UpdateCustomer(Customer customer)
        {
            using (var client = GetHttpClient())
            {
                string url = string.Format("{0}/{1}", api_URL, customer.Id);
                var result = client.PutAsJsonAsync(url, customer).Result;
                try
                {
                    result.EnsureSuccessStatusCode();
                    return result.Content.ReadAsAsync<bool>().Result;
                }
                catch
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// 使用 DELETE 刪除單一筆資料
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        public bool DeleteSingleCustomer(int customerId)
        {
            using (var client = GetHttpClient())
            {
                string url = string.Format("{0}/{1}", api_URL, customerId);
                var result = client.DeleteAsync(url).Result;
                try
                {
                    result.EnsureSuccessStatusCode();
                    return result.Content.ReadAsAsync<bool>().Result;
                }
                catch
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// 使用 DELETE 刪除所有資料
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        public bool DeleteAllCustomer()
        {
            using (var client = GetHttpClient())
            {
                var result = client.DeleteAsync(api_URL).Result;
                try
                {
                    result.EnsureSuccessStatusCode();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

    }
}
