namespace insubordination.rule.engine
{
    using System.Threading.Tasks;

    public abstract class Rule
    {
        public string Name { get; set; }
        public string Group { get; set; }
        public uint Priority { get; set; }
        internal Rule Successor { get; private set; }

        internal void SetSuccessor(Rule successor)
        {
            Successor = successor;
        }

        public abstract Task<bool> MatchAsync<T>(T t);        
    } 
}
