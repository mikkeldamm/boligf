using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Boligf.Api.Filters
{
	public class ValidateModelFilterAttribute : ActionFilterAttribute
	{
		public HttpRequestMessage TestRequestMessage { get; set; }

		public override void OnActionExecuting(HttpActionContext actionContext)
		{
			PerformValidation(actionContext, TestRequestMessage ?? actionContext.Request);
		}

		private static void PerformValidation(HttpActionContext actionContext, HttpRequestMessage request)
		{
			if (actionContext.ModelState.IsValid == false)
			{
				actionContext.Response = request.CreateErrorResponse(HttpStatusCode.BadRequest, actionContext.ModelState);
			}
		}
	}
}