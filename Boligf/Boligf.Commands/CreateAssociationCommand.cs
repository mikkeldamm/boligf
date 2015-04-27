using System;
using Boligf.Domain;
using d60.Cirqus.Commands;

namespace Boligf.Commands
{
    public class CreateAssociationCommand : Command<Association>
    {
	    private readonly string _name;

	    public CreateAssociationCommand(string aggregateRootId, string name) : base(aggregateRootId)
	    {
		    _name = name;
	    }

	    public override void Execute(Association aggregateRoot)
	    {

	    }
    }
}
