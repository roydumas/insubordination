namespace insubordination.model
{
    using System.Threading.Tasks;

    public interface IAnimalRepository
    {
        Task<Animal> GetAnimalByNameAsync(string name);
    }
}
