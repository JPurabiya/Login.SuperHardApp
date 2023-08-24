namespace Login_MVC_.Controllers
{
   
        public class Root
        {
            public string token_type { get; set; }
            public int expires_in { get; set; }
            public int ext_expires_in { get; set; }
            public string access_token { get; set; }
        }
    
}