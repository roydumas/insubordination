namespace insubordination.rule.engine
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    using insubordination.rule.engine.attribute;

    public sealed class RulesBasedEngine
    {
        private readonly bool _processAllRules;
        
        private IServiceProvider ServiceProvider { get; set; }
        private IList<Rule> Rules { get; set; }
        
        public IList<string> DisabledRules { get; }
        public IList<string> FailedRules { get; }
        public IList<string> PassedRules { get; }

        public RulesBasedEngine(bool processAllRules = true)
        {
            _processAllRules = processAllRules;

            Rules = new List<Rule>();

            DisabledRules = new List<string>();
            FailedRules = new List<string>();
            PassedRules = new List<string>();
        }

        public void SetContainer(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }

        public void Load(Assembly assembly)
        {
            var types = assembly.GetTypes();

            foreach (var type in types)
            {
                if (!type.IsSubclassOf(typeof(Rule))) continue;

                var friendlyNameAttribute =
                    (FriendlyNameAttribute)Attribute.GetCustomAttribute(type, typeof(FriendlyNameAttribute));

                var name = friendlyNameAttribute != null ? friendlyNameAttribute.Name : type.Name;

                var disableAttribute =
                    (DisabledAttribute)Attribute.GetCustomAttribute(type, typeof(DisabledAttribute));

                if (disableAttribute != null)
                {
                    DisabledRules.Add(name);
                    continue;
                }

                var priority = uint.MaxValue;
                var priorityAttribute =
                    (PriorityAttribute) Attribute.GetCustomAttribute(type, typeof(PriorityAttribute));

                if (priorityAttribute != null)
                {
                    priority = priorityAttribute.Priority;
                }
                
                IList<object> parameters = new List<object>();

                if (ServiceProvider != null)
                {
                    parameters = ResolveDependencies(type);
                }

                var rule = (Rule)Activator.CreateInstance(type, parameters.ToArray());
                rule.Priority = priority;
                rule.Name = name;
                
                Rules.Add(rule);                
            }

            Rules = Rules.OrderBy(o => o.Priority).ToList();
            
            CreateChain();
        }
        
        /// <summary>
        /// Returns rules that didn't pass
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns>
        /// if all rules passed, then an empty collection, 
        /// otherwise, the list of  rules that did not pass.
        /// </returns>
        public async Task<IEnumerable<Rule>> FireAsync<T>(T t)
        {
            return await ProcessChainAsync(t);
        }

        private void CreateChain()
        {
            var rule = Rules.First();

            for (var i = 1; i < Rules.Count; i++)
            {
                var successor = Rules[i];
                rule.SetSuccessor(successor);
                rule = successor;
            }
        }

        private async Task<IEnumerable<Rule>> ProcessChainAsync<T>(T t)
        {
            var notMatchedRules = new List<Rule>();
            var currentRule = Rules.First();

            do
            {
                var match = await currentRule.MatchAsync(t);
                if (match == false)
                {
                    FailedRules.Add(currentRule.Name);
                    notMatchedRules.Add(currentRule);
                    if (_processAllRules == false) break;
                }
                else
                {
                    PassedRules.Add(currentRule.Name);
                }
                currentRule = currentRule.Successor;
            } while (currentRule != null);
            
            return notMatchedRules;
        }

        private IList<object> ResolveDependencies(Type type)
        {
            var list = new List<object>();
            var constructors = type.GetConstructors();

            foreach (var constructorInfo in constructors)
            {
                foreach (var parameterInfo in constructorInfo.GetParameters())
                {
                    var service = ServiceProvider.GetService(parameterInfo.ParameterType);

                    if (service != null)
                    {
                        list.Add(service);
                    }
                }
            }

            return list;
        }
    }
}
