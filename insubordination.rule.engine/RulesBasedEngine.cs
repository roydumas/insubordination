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
        
        public IList<Rule> DisabledRules { get; }
        public IList<Rule> FailedRules { get; }
        public IList<Rule> PassedRules { get; }

        public RulesBasedEngine(bool processAllRules = true)
        {
            _processAllRules = processAllRules;

            Rules = new List<Rule>();

            DisabledRules = new List<Rule>();
            FailedRules = new List<Rule>();
            PassedRules = new List<Rule>();
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

                var group = string.Empty;
                var groupAttribute =
                    (GroupAttribute)Attribute.GetCustomAttribute(type, typeof(GroupAttribute));

                if (groupAttribute != null)
                {
                    group = groupAttribute.Name;
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
                rule.Group = group;

                if (disableAttribute != null)
                {
                    DisabledRules.Add(rule);
                    continue;
                }

                Rules.Add(rule);                
            }

            if (Rules.Any() == false)
            {
                throw new ArgumentException(
                    "A referenced assembly must contain at least one subclass implementation of the abstract class Rule.");
            }
        }

        /// <summary>
        /// Returns rules that didn't pass
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="group"></param>
        /// <returns>
        /// if all rules passed, then an empty collection, 
        /// otherwise, the list of  rules that did not pass.
        /// </returns>
        public async Task<IEnumerable<Rule>> MatchAsync<T>(T t, string group = "")
        {
            Reset();
            var rules = CreateChain(group);
            return await ProcessChainAsync(t, rules);
        }

        private IEnumerable<Rule> CreateChain(string group = "")
        {
            var groupRules =
                string.IsNullOrEmpty(group) ? Rules : Rules.Where(r => r.Group == group).ToList();

            if (groupRules.Any() == false)
            {
                throw new ArgumentException($"There aren't any rules in the group [{group}]", nameof(group));
            }

            var rules = groupRules.OrderBy(gr => gr.Priority).ToList();
            var rule = rules.First();

            for (var i = 1; i < rules.Count; i++)
            {
                var successor = rules[i];
                rule.SetSuccessor(successor);
                rule = successor;
            }

            return rules;
        }

        private void Reset()
        {
            foreach (var rule in Rules)
            {
                rule.SetSuccessor(null);
            }

            DisabledRules.Clear();
            FailedRules.Clear();            
            PassedRules.Clear();
        }
        
        private async Task<IEnumerable<Rule>> ProcessChainAsync<T>(T t, IEnumerable<Rule> rules)
        {
            var notMatchedRules = new List<Rule>();
            var currentRule = rules.First();

            do
            {
                var match = await currentRule.MatchAsync(t);
                if (match == false)
                {
                    FailedRules.Add(currentRule);
                    notMatchedRules.Add(currentRule);

                    if (_processAllRules == false) break;
                }
                else
                {
                    PassedRules.Add(currentRule);
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
