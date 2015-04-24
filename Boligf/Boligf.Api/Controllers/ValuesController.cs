using System.Collections.Generic;
using System.Web.Http;
using Boligf.Application;
using d60.Cirqus.Views;
using d60.Cirqus.Views.ViewManagers;

namespace Boligf.Api.Controllers
{
    public class ValuesController : ApiController
    {
	    private readonly IViewManager<ProcessorConfiguration.CounterView> _counterView;

	    public ValuesController(IViewManager<ProcessorConfiguration.CounterView> counterView)
	    {
		    _counterView = counterView;
	    }

	    // GET api/values
        public IEnumerable<string> Get()
        {
            return new[] { "value1", "value2" };
        }

        // GET api/values/5
        public string Get(string id)
        {
	        var counterView = _counterView.Load(id);

	        return counterView.CurrentValue.ToString();
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
