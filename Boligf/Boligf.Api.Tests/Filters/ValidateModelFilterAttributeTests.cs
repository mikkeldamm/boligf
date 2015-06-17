using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using Boligf.Api.Filters;
using Shouldly;
using Xunit;

namespace Boligf.Api.Tests.Filters
{
	public class ValidateModelFilterAttributeTests
	{
		[Fact]
		public void OnActionExecuting_ModelIsInvalid_ReturnBadRequest()
		{
			var filter = new ValidateModelFilterAttribute { TestRequestMessage = new HttpRequestMessage() };
			
			var httpActionContext = new HttpActionContext();
            httpActionContext.ModelState.AddModelError("Error", "Fail");

			filter.OnActionExecuting(httpActionContext);

			httpActionContext.Response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        }

		[Fact]
		public void OnActionExecuting_ModelIsValid_ReturnDefaultRequest()
		{
			var filter = new ValidateModelFilterAttribute { TestRequestMessage = new HttpRequestMessage() };

			var httpActionContext = new HttpActionContext();

			filter.OnActionExecuting(httpActionContext);

			httpActionContext.Response.ShouldBe(null);
		}
	}
}
