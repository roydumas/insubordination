namespace insubordination.model
{
    using System.Threading.Tasks;

    public class AnimalService : IAnimalService
    {
        private readonly IAnimalRepository _animalRepository;

        public AnimalService(IAnimalRepository animalRepository)
        {
            _animalRepository = animalRepository;
        }

        public async Task<Animal> GetAnimalByNameAsync(string name)
        {
            var animal = await _animalRepository.GetAnimalByNameAsync(name);
            return animal;
        }        
    }
}
