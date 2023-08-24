using Login_MVC_.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using RestSharp;
using System.Threading.Tasks;
using System.Web.Helpers;
using System.Net;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Reflection;

namespace Login_MVC_.Controllers
{
    public class AccountController : Controller
    {
    

        SqlConnection con = new SqlConnection();
        SqlCommand com = new SqlCommand();
        SqlDataReader dr;

        // GET: Account
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        void connectionString()
        {
            con.ConnectionString = "data source=DGIT-D-0017\\SQLEXPRESS ; database=WPF; integrated security =SSPI";

        }
        //async Task<string> tokenAsync()
        //{
        //    var options = new RestClientOptions("https://login.microsoftonline.com")
        //    {
        //        MaxTimeout = -1,
        //    };
        //    var client = new RestClient(options);
        //    var request = new RestRequest("/ffdefe31-ec1a-445a-8303-b1a05ec6c0b8/oauth2/v2.0/token", Method.Post);
        //    request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
        //    request.AddHeader("Cookie", "fpc=AueCt3L-ymZIoBYyB3FQ4msJBzfDAgAAANQBbNwOAAAA; stsservicecookie=estsfd; x-ms-gateway-slice=estsfd");
        //    request.AddParameter("Grant_Type", "client_credentials");
        //    request.AddParameter("Client_ID", "aa6a2f61-ad08-4dba-b67f-7e29fb25802a");
        //    request.AddParameter("Client_Secret", "Wbh8Q~Ik4_E1JAr0Pp4AzT4Zm.t8rxzvLqkJyaYX");
        //    request.AddParameter("Scope", "https://operations-dcspluat-1.crm5.dynamics.com/.default");
        //    RestResponse response = await client.ExecuteAsync(request);
        //    return (response.Content);
        //}

        string rtoken()
        {

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            var client = new RestClient("https://login.microsoftonline.com");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddHeader("Cookie", "fpc=AueCt3L-ymZIoBYyB3FQ4msJBzfDAgAAANQBbNwOAAAA; stsservicecookie=estsfd; x-ms-gateway-slice=estsfd");
            request.AddParameter("Grant_Type", "client_credentials");
            request.AddParameter("Client_ID", "aa6a2f61-ad08-4dba-b67f-7e29fb25802a");
            request.AddParameter("Client_Secret", "Wbh8Q~Ik4_E1JAr0Pp4AzT4Zm.t8rxzvLqkJyaYX");
            request.AddParameter("Scope", "https://operations-dcspluat-1.crm5.dynamics.com/.default");
            //request.AddParameter("Client_Authentication", "Send client credentials in body");
            IRestResponse response = client.Execute(request);
            Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(response.Content);
            var token = myDeserializedClass.access_token;
            //string tokens = tokenAsync().Result;
            return token;





        }
        [HttpPost]
        public async Task<ActionResult> VerifyAsync(Account acc)
        {
            if (ModelState.IsValid)
            {
                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Post, "https://login.microsoftonline.com/ffdefe31-ec1a-445a-8303-b1a05ec6c0b8/oauth2/v2.0/token");
                request.Headers.Add("Cookie", "fpc=AueCt3L-ymZIoBYyB3FQ4msJBzfDAQAAAGpibtwOAAAA; stsservicecookie=estsfd; x-ms-gateway-slice=estsfd");

                var collection = new List<KeyValuePair<string, string>>();
                collection.Add(new KeyValuePair<string, string>("Grant_Type", "client_credentials"));
                collection.Add(new KeyValuePair<string, string>("Client_ID", "aa6a2f61-ad08-4dba-b67f-7e29fb25802a"));
                collection.Add(new KeyValuePair<string, string>("Client_Secret", "Wbh8Q~Ik4_E1JAr0Pp4AzT4Zm.t8rxzvLqkJyaYX"));
                collection.Add(new KeyValuePair<string, string>("Scope", "https://operations-dcspluat-1.crm5.dynamics.com/.default"));

                var content = new FormUrlEncodedContent(collection);
                request.Content = content;

                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();
                string responseContent = await response.Content.ReadAsStringAsync();
                Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(responseContent);

                //  Console.WriteLine(await response.Content.ReadAsStringAsync());
                //Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(response.Content);
                var token = myDeserializedClass.access_token;


                var client1 = new HttpClient();

                string md5Hash = CalculateMD5Hash(acc.Password);

                var request1 = new HttpRequestMessage(HttpMethod.Get, "https://operations-dcspluat-1.api.crm5.dynamics.com/api/data/v9.2/dcs_usermanagements?$filter= dcs_username eq '" + acc.username + "' and dcs_password eq '" + md5Hash + "'");
                request1.Headers.Add("Authorization", token);
                request1.Headers.Add("Cookie", "ARRAffinity=4ab478d8e96967df29ecf94d2c836791f87a2e539fbbecf262fd75f31124f4f65ab954b42cfa12ad9f33ddc80a07e15dd633bd57139a65479392e46e5242529408DB9E48DD1C3F961437112219; ReqClientId=92812ba6-d33b-4612-8613-7feb1ba5208e; orgId=954b4115-32ce-ed11-aed1-002248568b9f");
                var response1 = await client1.SendAsync(request1);
                response1.EnsureSuccessStatusCode();
                string responseContent1 = await response1.Content.ReadAsStringAsync();

                Rootobject myDeserializedClass1 = JsonConvert.DeserializeObject<Rootobject>(responseContent1);


                foreach (var item in myDeserializedClass1.value)
                {
                    string dcs_employeeid = item.dcs_employeeid;
                    string dcs_usermanagementid = item.dcs_usermanagementid;
                    string dcs_username = item.dcs_username;
                    string dcs_password = item.dcs_password;

                    if (dcs_username == acc.username && dcs_password == md5Hash)
                    {
                        return View("Create");
                    }

                    // Print other fields
                }


                return View("Error");
            }
            return View("Login");
        }

        private string CalculateMD5Hash(string password)
        {


            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(password);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                StringBuilder sb = new StringBuilder();

                foreach (byte b in hashBytes)
                {
                    sb.Append(b.ToString("x2"));
                }

                return sb.ToString();
            }
            //throw new NotImplementedException();
        }
    }
}