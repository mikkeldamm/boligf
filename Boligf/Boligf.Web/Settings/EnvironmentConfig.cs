using System.Configuration;
using System.Web;

namespace Boligf.Web.Settings
{
	public class EnvironmentConfig : IHttpHandler
	{
		public bool IsReusable { get { return true; } }

		public string ApiDomain
		{
			get
			{
				var apiDomainConfig = ConfigurationManager.AppSettings["apiDomain"];
                return !string.IsNullOrEmpty(apiDomainConfig) ? apiDomainConfig : "localhost:17776";
			}
		}

		public void ProcessRequest(HttpContext context)
		{
			context.Response.ContentType = "application/x-javascript";
			context.Response.Write(GetJavascriptConfiguration());
		}

		private string GetJavascriptConfiguration()
		{
			return string.Format(
				@"var Boligf = Boligf || (Boligf = {{}});Boligf.Config = {{ ApiProtocol: 'http://', ApiDomainClean: '{0}', ApiDomain: '{0}/api', ApiAccess: function(hideApi) {{ return Boligf.Config.ApiProtocol + (hideApi ? Boligf.Config.ApiDomainClean : Boligf.Config.ApiDomain); }} }};",
				ApiDomain
			);
		}
	}
}
