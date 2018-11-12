namespace insubordination.rule.plugin
{
    using System.Threading.Tasks;

    using insubordination.model;
    using insubordination.rule.engine;
    using insubordination.rule.engine.attribute;

    [FriendlyName("Duck")]
    [Priority(0)]
    public class DuckRule : Rule
    {
        public override async Task<bool> MatchAsync<T>(T t)
        {
            await Task.CompletedTask;

            var animal = t as Animal;

            return animal?.CanFly == true;
        }
    }
}
