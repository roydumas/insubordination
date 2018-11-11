namespace insubordination.rule.engine.attribute
{
    using System;

    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class PriorityAttribute : Attribute
    {
        public uint Priority { get; }

        public PriorityAttribute(uint priority)
        {
            Priority = priority;
        }
    }
}
