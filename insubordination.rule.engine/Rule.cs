namespace insubordination.rule.engine
{
    public abstract class Rule
    {
        public string Name { get; set; }
        public uint Priority { get; set; }
        internal Rule Successor { get; private set; }

        internal void SetSuccessor(Rule successor)
        {
            Successor = successor;
        }

        public abstract bool IsMatch<T>(T t);        
    } 
}
