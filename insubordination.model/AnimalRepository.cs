namespace insubordination.model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class AnimalRepository : IAnimalRepository
    {
        public Animal GetAnimalByName(string name)
        {
            var list = new List<Animal> {new Animal {Name = "Bull"}};
            
            return list.FirstOrDefault(l => l.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase));
        }
    }
}
