namespace insubordination.rule.plugin
{
    using System.Threading.Tasks;

    using insubordination.model;
    using insubordination.rule.engine;
    using insubordination.rule.engine.attribute;

    [FriendlyName("Cow")]
    [Priority(3)]
    public class CowRule : Rule
    {
        public override async Task<bool> MatchAsync<T>(T t)
        {
            await Task.CompletedTask;

            var animal = t as Animal;

            if (animal == null) return false;

            return animal.IsMammal;
        }
    }
}
