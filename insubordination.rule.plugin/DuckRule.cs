namespace insubordination.rule.plugin
{
    using insubordination.model;
    using insubordination.rule.engine;
    using insubordination.rule.engine.attribute;

    [FriendlyName("Duck")]
    [Priority(0)]
    public class DuckRule : Rule
    {
        public override bool IsMatch<T>(T t)
        {
            var animal = t as Animal;

            return animal?.CanFly == false;
        }
    }
}
