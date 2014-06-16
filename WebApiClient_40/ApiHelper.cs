using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;

namespace WebApiClient
{
    public class ApiHelper
    {

        private string m_BaseAddress = "http://localhost:2684";
        private string api_URL = "api/Customer";


        private T ReadAsAsync<T>(string url) where T : new()
        {
            try
            {
                string responseContent = SendMessageToApiServer(url);
                if (responseContent != null)
                {
                    //StorageMan<T> sm = new StorageMan<T>();
                    //return sm.StringToObject(responseContent);
                    return JsonUtil<T>.StringToObject(responseContent);
                }

            }  // end try
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                //Console.ReadLine();
            }
            return default(T);
        }

        private string SendMessageToApiServer(string url)
        {
            HttpWebRequest request = (HttpWebRequest)System.Net.WebRequest.Create(url);
            request.Method = "GET";
            WebResponse response = request.GetResponse();

            //TODO
            Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string responseFromServer = reader.ReadToEnd();
            reader.Close();
            dataStream.Close();
            response.Close();
            return responseFromServer;
        }

        private class JsonUtil<T> where T : new()
        {
            public static string ObjectToString(T data)
            {
                return JsonConvert.SerializeObject(data);
            }

            public static StringContent ObjectToStringContent(T data)
            {
                return new System.Net.Http.StringContent(ObjectToString(data), Encoding.UTF8, "application/json");
            }

            public static T StringToObject(string json)
            {
                using (StringReader sr = new StringReader(json))
                {
                    using (JsonTextReader reader = new JsonTextReader(sr))
                    {
                        JsonSerializer serializer = new JsonSerializer();
                        object obj = serializer.Deserialize<T>(reader);

                        return (obj != null ? (T)obj : new T());
                    }
                }
            }
        }

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
            StringContent sendingData = JsonUtil<Customer>.ObjectToStringContent(customer);
            using (var client = GetHttpClient())
            {
                var result = client.PostAsync(api_URL, sendingData).Result;
                try
                {
                    result.EnsureSuccessStatusCode();
                    string readResult = result.Content.ReadAsStringAsync().Result;
                    return JsonUtil<int>.StringToObject(readResult);
                }
                catch(Exception ex)
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
                    string readResult = result.Content.ReadAsStringAsync().Result;
                    return JsonUtil<Customer>.StringToObject(readResult);
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
                    //return result.Content.ReadAsAsync<List<Customer>>().Result;
                    string readResult = result.Content.ReadAsStringAsync().Result;
                    return JsonUtil<List<Customer>>.StringToObject(readResult);
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
                StringContent sendingData = JsonUtil<Customer>.ObjectToStringContent(customer);
                var result = client.PutAsync(url, sendingData).Result;
                try
                {
                    result.EnsureSuccessStatusCode();
                    string readResult = result.Content.ReadAsStringAsync().Result;
                    return JsonUtil<bool>.StringToObject(readResult);
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
                    string readResult = result.Content.ReadAsStringAsync().Result;
                    return JsonUtil<bool>.StringToObject(readResult);
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
