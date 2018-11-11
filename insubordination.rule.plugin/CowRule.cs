namespace insubordination.rule.plugin
{
    using insubordination.model;
    using insubordination.rule.engine;
    using insubordination.rule.engine.attribute;

    [FriendlyName("Cow")]
    [Priority(3)]
    public class CowRule : Rule
    {
        public override bool IsMatch<T>(T t)
        {
            var animal = t as Animal;

            if (animal == null) return false;

            return animal.IsMammal;
        }
    }
}
