namespace insubordination.model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class AnimalRepository : IAnimalRepository
    {
        public async Task<Animal> GetAnimalByNameAsync(string name)
        {
            await Task.CompletedTask;

            var list = new List<Animal> {new Animal {Name = "Bull"}};
            
            return list.FirstOrDefault(l => l.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase));
        }
    }
}
