using System;
using System.Linq;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Web.Http;
using Boligf.Api.Filters;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Owin;

namespace Boligf.Api.Configuration
{
	public static class WebApiConfig
	{
		public static void Setup(IAppBuilder app)
		{
			var httpConfiguration = new HttpConfiguration();

			httpConfiguration.DependencyResolver = GlobalConfiguration.Configuration.DependencyResolver;
			httpConfiguration.Filters.Add(new ValidateModelFilterAttribute());
			httpConfiguration.Filters.Add(new HandleNullReturnFilterAttribute());

			httpConfiguration.MapHttpAttributeRoutes();
			httpConfiguration.Routes.MapHttpRoute(
				name: "DefaultApi",
				routeTemplate: "api/{controller}/{id}",
				defaults: new { id = RouteParameter.Optional }
			);

			httpConfiguration.Formatters.Add(new BrowserJsonFormatter());

			var jsonFormatter = httpConfiguration.Formatters.OfType<JsonMediaTypeFormatter>().First();
			jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

			app.UseWebApi(httpConfiguration);
		}

		public class BrowserJsonFormatter : JsonMediaTypeFormatter
		{
			public BrowserJsonFormatter()
			{
				SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));
				SerializerSettings.Formatting = Formatting.Indented;
			}

			public override void SetDefaultContentHeaders(Type type, HttpContentHeaders headers, MediaTypeHeaderValue mediaType)
			{
				base.SetDefaultContentHeaders(type, headers, mediaType);

				headers.ContentType = new MediaTypeHeaderValue("application/json");
			}
		}
	}
}
