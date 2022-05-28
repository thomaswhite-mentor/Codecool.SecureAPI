using Codecool.SecureApi.Tests.Extensions;
using Codecool.SecureAPI.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Codecool.SecureApi.Tests
{
    public class BaseUserControllerTest
    {
        protected UserViewModel GetExpectedUser(string resource)
        {
            var expectedStructureJson = Assembly.GetExecutingAssembly().GetEmbeddedResourceContent(resource);
            if (expectedStructureJson == null)
            {
                return new UserViewModel();
            }
            return JsonConvert.DeserializeObject<UserViewModel>(expectedStructureJson);
        }
    }
}
