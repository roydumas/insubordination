namespace insubordination.rule.plugin
{
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

        public override bool IsMatch<T>(T t)
        {
            var animal = t as Animal;

            if (animal == null) return false;

            var returnedAnimal = _animalService.GetAnimalByName("Bull");
            return animal.Name == returnedAnimal.Name;
        }
    }
}
