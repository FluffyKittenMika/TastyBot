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
            Type[] classes = GetClassTypesInNamespace(Assembly.GetExecutingAssembly(), "PictureAPIs.PictureAPIs");
            string methodName = $"Get{pictureTypeName}";

            if (classes == null)
            {
                Logging.LogErrorMessage("PictureAPIsHub", $"No pictureAPI found.").PerformAsyncTaskWithoutAwait();
                throw new NotImplementedException();
            }

            var args = SetUpArguments(optionalArgs);
            var method = GetMethodFromClassList(classes.ToList(), methodName);

            if (method == null)
            {
                Logging.LogErrorMessage("PictureAPIsHub", $"Method {methodName} does not exists in the included PictureAPI's.").PerformAsyncTaskWithoutAwait();
                throw new NotImplementedException();
            }

            return await (Task<Stream>)method.Invoke(null, args);
        }

        private object[] SetUpArguments(object[] arguments)
        {
            List<object> args = new List<object>()
            {
                _http
            };
            

            if (arguments != null)
            {
                foreach (var argument in arguments)
                {
                    args.Add(argument);
                }
            }

            return args.ToArray();
        }

        private Type[] GetClassTypesInNamespace(Assembly assembly, string @namespace)
        {
            return
              assembly.GetTypes()
                      .Where(t => string.Equals(t.Namespace, @namespace, StringComparison.Ordinal))
                      .ToArray();
        }

        private MethodInfo GetMethodFromClassList(List<Type> classes, string methodName)
        {
            MethodInfo method = null;
            foreach (var @class in classes)
            {
                var potentialMethod = @class.GetMethod(methodName);
                if (potentialMethod != null)
                {
                    method = potentialMethod;
                    break;
                }
            }
            return method;
        }
    }
}
