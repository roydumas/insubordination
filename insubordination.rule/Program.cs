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
            var rbe = new RulesBasedEngine();

            var animal = new Animal
            {
                Name = "Bull",
                CanFly = true,
                IsBirdKingdom = true,
                IsMammal = true,
                HasFourLegs = true
            };

            rbe.SetContainer(ConfigureServices());
            rbe.Load(Assembly.GetAssembly(typeof(BullRule)));
            
            var failedRules = rbe.FireAsync(animal).Result;
            
            Console.WriteLine($"Failed Rules: { string.Join(",", failedRules.Select(f => f.Name)) }");
            Console.WriteLine($"Disabled: {string.Join(",", rbe.DisabledRules)}");
            Console.WriteLine($"Passed: {string.Join(",", rbe.PassedRules)}");

            Console.ReadKey();
        }

        private static IServiceProvider ConfigureServices()
        {
            var serviceProvider = new ServiceCollection()
                .AddScoped<IAnimalRepository, AnimalRepository>()
                .AddScoped<IAnimalService, AnimalService>()
                .BuildServiceProvider();

            return serviceProvider;
        }
    }
}
