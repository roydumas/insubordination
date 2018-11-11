namespace insubordination.model
{
    public class AnimalService : IAnimalService
    {
        private readonly IAnimalRepository _animalRepository;

        public AnimalService(IAnimalRepository animalRepository)
        {
            _animalRepository = animalRepository;
        }

        public Animal GetAnimalByName(string name)
        {
            return _animalRepository.GetAnimalByName(name);
        }
    }
}
