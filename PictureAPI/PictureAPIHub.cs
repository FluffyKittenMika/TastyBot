using Interfaces.Contracts.HeadpatPictures;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using Utilities.LoggingService;
using Utilities.TasksUtilities;

namespace PictureAPIs
{
    public class PictureAPIHub : IPictureAPIHub
    {
        private readonly HttpClient _http;
        public PictureAPIHub(HttpClient http)
        {
            _http = http;
        }

        public async Task<Stream> GetStreamByPictureTypeName(string pictureTypeName, object[] optionalArgs = null)
        {
            Type[] classes = GetTypesInNamespace(Assembly.GetExecutingAssembly(), "PictureAPIs.PictureAPIs");
            List<object> args = new List<object>()
            {
                _http
            };
            string methodName = $"Get{pictureTypeName}";
            Stream stream = null;

            if(optionalArgs != null)
            {
                foreach (var arg in optionalArgs)
                {
                    args.Add(arg);
                }
            }

            foreach (Type @class in classes) 
            {
                var method = @class.GetMethod(methodName);
                if(method != null)
                {
                    stream = await (Task<Stream>)method.Invoke(null, args.ToArray());
                    break;
                }
            }

            if(stream == null)
            {
                Logging.LogErrorMessage("PictureAPIsHub", $"Method {methodName} does not exists in the included PictureAPI's").PerformAsyncTaskWithoutAwait();
                throw new NotImplementedException();
            }

            return stream;
        }

        private Type[] GetTypesInNamespace(Assembly assembly, string nameSpace)
        {
            return
              assembly.GetTypes()
                      .Where(t => String.Equals(t.Namespace, nameSpace, StringComparison.Ordinal))
                      .ToArray();
        }
    }
}
