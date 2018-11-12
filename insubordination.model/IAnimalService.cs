namespace insubordination.model
{
    using System.Threading.Tasks;

    public interface IAnimalService
    {
        Task<Animal> GetAnimalByNameAsync(string name);
    }
}
