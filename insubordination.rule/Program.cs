namespace insubordination.rule
{
    using System;
    using System.Linq;
    using System.Reflection;

    using Microsoft.Extensions.DependencyInjection;

    using insubordination.model;
    using insubordination.rule.engine;
    using insubordination.rule.plugin;

    class Program
    {
        static void Main(string[] args)
        {
            var rbe = new RulesBaseEngine();

            var animal = new Animal
            {
                Name = "Bull",
                CanFly = true,
                IsBirdKingdom = true,
                IsMammal = true,
                HasFourLegs = true
            };

            rbe.SetContainer(ConfigureServices());
            rbe.LoadTypes(Assembly.GetAssembly(typeof(BullRule)));
            
            var failedRules = rbe.Fire(animal);
            
            Console.WriteLine($"Failed Rules: { failedRules.Count() }");
            Console.ReadKey();
        }

        private static IServiceProvider ConfigureServices()
        {
            //setup our DI
            var serviceProvider = new ServiceCollection()
                .AddScoped<IAnimalRepository, AnimalRepository>()
                .AddScoped<IAnimalService, AnimalService>()
                .BuildServiceProvider();

            return serviceProvider;
        }
    }
}
