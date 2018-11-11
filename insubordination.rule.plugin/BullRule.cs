namespace insubordination.rule.plugin
{
    using insubordination.model;
    using insubordination.rule.engine;
    using insubordination.rule.engine.attribute;

    [FriendlyName("Bull")]
    [Priority(1)]
    public class BullRule : Rule
    {
        public override bool IsMatch<T>(T t)
        {
            var animal = t as Animal;

            if (animal == null) return false;

            return animal.Name == "Bull";
        }
    }
}
