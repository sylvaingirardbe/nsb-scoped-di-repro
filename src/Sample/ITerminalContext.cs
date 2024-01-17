namespace Sample;

public interface ITerminalContext
{
    public Terminal Terminal { get; set; }
}

public class TerminalContext : ITerminalContext
{
    public Terminal Terminal { get; set; }
} 
