using System;
using Boligf.Domain.Events;
using d60.Cirqus.Aggregates;
using d60.Cirqus.Events;

namespace Boligf.Domain
{
    public class Association : AggregateRoot, 
		IEmit<AssociationCreated>
    { 
	    public string Name { get; set; }

		protected override void Created()
		{
			Emit(new AssociationCreated());
		}

		public void Apply(AssociationCreated e)
		{
			// Instead of this we ofcourse needs a new event for setting name
			// And it can still be in the command
			Name = "IAmCreated" + DateTime.Now.Millisecond;
		}
    }
}
