using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Client.Services
{
    public class ProjectManager
    {
      private readonly HttpClient _httpClient;
        public ProjectManager(HttpClient httpClient)
        {
         _httpClient = httpClient;
            
        }
    }
}