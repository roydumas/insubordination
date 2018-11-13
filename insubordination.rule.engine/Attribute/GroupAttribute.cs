namespace insubordination.rule.engine.attribute
{
    using System;

    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class GroupAttribute : Attribute
    {
        public string Name { get; }

        public GroupAttribute(string name)
        {
            Name = name;
        }
    }
}
