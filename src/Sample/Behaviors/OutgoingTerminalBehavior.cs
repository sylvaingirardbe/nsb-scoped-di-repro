using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NServiceBus.Pipeline;

namespace Sample.Behaviors;

public class OutgoingTerminalBehavior : 
    Behavior<IOutgoingLogicalMessageContext>
{
    public override Task Invoke(IOutgoingLogicalMessageContext context, Func<Task> next)
    {
        var terminalContext = context.Builder.GetRequiredService<ITerminalContext>();
        context.Headers["x-terminal"] = terminalContext.Terminal.ToString();

        return next();
    }
}