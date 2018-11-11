namespace insubordination.rule.engine.attribute
{
    using System;

    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class FriendlyNameAttribute : Attribute
    {
        public string Name { get; }

        public FriendlyNameAttribute(string name)
        {
            Name = name;
        }
    }
}
