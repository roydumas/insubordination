namespace insubordination.model
{
    public interface IAnimalRepository
    {
        Animal GetAnimalByName(string name);
    }
}
