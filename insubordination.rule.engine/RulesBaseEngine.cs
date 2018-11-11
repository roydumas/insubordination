namespace insubordination.rule.engine
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using insubordination.rule.engine.attribute;

    public sealed class RulesBaseEngine
    {
        private IList<Rule> Rules { get; set; }

        public RulesBaseEngine()
        {
            Rules = new List<Rule>();
        }

        public void LoadTypes(Assembly assembly)
        {
            var types = assembly.GetTypes();

            foreach (var type in types)
            {
                if (!type.IsSubclassOf(typeof(Rule))) continue;

                var priority = uint.MaxValue;
                var priorityAttribute =
                    (PriorityAttribute) System.Attribute.GetCustomAttribute(type, typeof(PriorityAttribute));

                if (priorityAttribute != null)
                {
                    priority = priorityAttribute.Priority;
                }

                var friendlyNameAttribute =
                    (FriendlyNameAttribute) System.Attribute.GetCustomAttribute(type, typeof(FriendlyNameAttribute));

                var name = friendlyNameAttribute != null ? friendlyNameAttribute.Name : type.Name;

                var rule = (Rule)Activator.CreateInstance(type);
                rule.Priority = priority;
                rule.Name = name;
                
                Rules.Add(rule);                
            }

            Rules = Rules.OrderBy(o => o.Priority).ToList();
            
            CreateChain();
        }

        public IEnumerable<Rule> Fire<T>(T t)
        {
            return ProcessChain(t);
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

        private IEnumerable<Rule> ProcessChain<T>(T t)
        {
            var failedRules = new List<Rule>();
            var currentRule = Rules.First();

            do
            {
                if (!currentRule.IsMatch(t))
                {
                    failedRules.Add(currentRule);
                }

                currentRule = currentRule.Successor;
            } while (currentRule != null);

            return failedRules;
        }
    }
}
