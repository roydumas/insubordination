namespace insubordination.rule.plugin
{
    using System.Threading.Tasks;

    using insubordination.model;
    using insubordination.rule.engine;
    using insubordination.rule.engine.attribute;
    
    [FriendlyName("Chicken")]
    [Priority(2)]
    public class ChickenRule : Rule
    {
        public override async Task<bool> MatchAsync<T>(T t)
        {
            await Task.CompletedTask;

            var animal = t as Animal;

            return animal?.IsBirdKingdom == false;
        }
    }
}
