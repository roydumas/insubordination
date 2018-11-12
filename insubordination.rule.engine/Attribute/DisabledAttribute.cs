namespace insubordination.rule.engine.attribute
{
    using System;

    public class DisabledAttribute : Attribute
    {
        public string Reason { get; set; }

        public DisabledAttribute(string reason)
        {
            Reason = reason;
        }
    }
}