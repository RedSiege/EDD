namespace EDD.Models
{
    public abstract class EDDFunction
    {
        public abstract string FunctionName { get; }

        public abstract string[] Execute(ParsedArgs args);
    }
}
