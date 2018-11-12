namespace insubordination.rule.plugin
{
    using System.Threading.Tasks;

    using insubordination.model;
    using insubordination.rule.engine;
    using insubordination.rule.engine.attribute;
    
    [FriendlyName("Bull")]
    [Priority(1)]
    public class BullRule : Rule
    {
        private readonly IAnimalService _animalService;

        public BullRule(IAnimalService animalService)
        {
            _animalService = animalService;
        }

        public override async Task<bool> MatchAsync<T>(T t)
        {
            var animal = t as Animal;

            if (animal == null) return false;

            var returnedAnimal = await _animalService.GetAnimalByNameAsync("Bull");
            return animal.Name == returnedAnimal.Name;
        }
    }
}
