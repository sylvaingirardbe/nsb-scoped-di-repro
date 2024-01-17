using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NServiceBus.Pipeline;

namespace Sample.Behaviors;

public class IncomingTerminalBehavior : 
    Behavior<IIncomingPhysicalMessageContext>
{
    public override Task Invoke(IIncomingPhysicalMessageContext context, Func<Task> next)
    {
        if(context.MessageHeaders.ContainsKey("x-terminal"))
        {
            var terminalContext = context.Builder.GetRequiredService<ITerminalContext>();
            var terminal = Enum.Parse<Terminal>(context.MessageHeaders["x-terminal"]);
            terminalContext.Terminal = terminal;
            return next();
        }
        else
        {
            throw new InvalidOperationException("No x-terminal header found");
        }
    }
}