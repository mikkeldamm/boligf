using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using d60.Cirqus;
using d60.Cirqus.Aggregates;
using d60.Cirqus.Commands;
using d60.Cirqus.Events;
using d60.Cirqus.Extensions;
using d60.Cirqus.MsSql.Config;
using d60.Cirqus.MsSql.Views;
using d60.Cirqus.Views.ViewManagers;
using d60.Cirqus.Views.ViewManagers.Locators;

namespace Boligf.Application
{
	public class ProcessorConfiguration
	{
		public static ICommandProcessor CommandProcessor { get; private set; }

		public static async Task Setup()
		{
			const string msSqlConnection = @"Data Source=(localdb)\v11.0;Initial Catalog=BoligfTest;Integrated Security=True;Connect Timeout=15;Encrypt=False;TrustServerCertificate=False";
			const string msSqlEventStoreTableName = "Events";

			IViewManager<CounterView> viewManager = new MsSqlViewManager<CounterView>(msSqlConnection, "Counters");

			CommandProcessor = d60.Cirqus.CommandProcessor
				.With()
				.EventStore(e => e.UseSqlServer(msSqlConnection, msSqlEventStoreTableName))
				.EventDispatcher(ed => ed.UseViewManagerEventDispatcher(viewManager))
				.Create();

			var counterView = viewManager.Load("id");
			var test1 = counterView.CurrentValue;

			var result = CommandProcessor.ProcessCommand(new IncrementCounter("id", 1));

			await viewManager.WaitUntilProcessed(result, TimeSpan.FromMinutes(2));

			var counterView2 = viewManager.Load("id");
			var test2 = counterView2.CurrentValue;
		}

		public class IncrementCounterCommand : ExecutableCommand
		{
			public override void Execute(ICommandContext context)
			{
				var counter = context.Load<Counter>("id");

				counter.Increment(Delta);
			}

			public int Delta { get; set; }
		}

		public class IncrementCounter : Command<Counter>
		{
			public IncrementCounter(string aggregateRootId, int delta)
				: base(aggregateRootId)
			{
				Delta = delta;
			}

			public int Delta { get; private set; }

			public override void Execute(Counter aggregateRoot)
			{
				aggregateRoot.Increment(Delta);
			}
		}

		public class CounterIncremented : DomainEvent<Counter>
		{
			public CounterIncremented(int delta)
			{
				Delta = delta;
			}

			public int Delta { get; private set; }
		}

		public class Counter : AggregateRoot, IEmit<CounterIncremented>
		{
			int _currentValue;

			public void Increment(int delta)
			{
				Emit(new CounterIncremented(delta));
			}

			public void Apply(CounterIncremented e)
			{
				_currentValue += e.Delta;
			}

			public int CurrentValue
			{
				get { return _currentValue; }
			}

			public double GetSecretBizValue()
			{
				return CurrentValue % 2 == 0
					? Math.PI
					: CurrentValue;
			}
		}

		public class CounterView : IViewInstance<InstancePerAggregateRootLocator>, ISubscribeTo<CounterIncremented>
		{
			public CounterView()
			{
				SomeRecentBizValues = new List<double>();
			}

			public string Id { get; set; }

			public long LastGlobalSequenceNumber { get; set; }

			public int CurrentValue { get; set; }

			public double SecretBizValue { get; set; }

			public List<double> SomeRecentBizValues { get; set; }

			public void Handle(IViewContext context, CounterIncremented domainEvent)
			{
				CurrentValue += domainEvent.Delta;

				var counter = context.Load<Counter>(domainEvent.GetAggregateRootId(), domainEvent.GetGlobalSequenceNumber());

				SecretBizValue = counter.GetSecretBizValue();

				SomeRecentBizValues.Add(SecretBizValue);

				// trim to 10 most recent biz values
				while (SomeRecentBizValues.Count > 10)
					SomeRecentBizValues.RemoveAt(0);
			}
		}
	}
}
