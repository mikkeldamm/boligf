using System.Linq;
using System.Net.Http.Formatting;
using System.Web.Http;
using Newtonsoft.Json.Serialization;
using Owin;

namespace Boligf.Api.Configuration
{
    public static class WebApiConfig
    {
        public static void Setup(IAppBuilder app)
        {
            var httpConfiguration = new HttpConfiguration();

            httpConfiguration.MapHttpAttributeRoutes();
            httpConfiguration.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            var jsonFormatter = httpConfiguration.Formatters.OfType<JsonMediaTypeFormatter>().First();
            jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            app.UseWebApi(httpConfiguration);
        }
    }
}
