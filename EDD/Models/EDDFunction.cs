namespace EDD.Models
{
    public abstract class EDDFunction
    {
        public abstract string FunctionName { get; }
        public abstract string FunctionDesc { get; }
        public abstract string FunctionUsage { get; }

        public abstract string[] Execute(ParsedArgs args);
    }
}
