using System;
using System.Collections.Generic;
using System.Linq;

namespace Plethora.Context.Action
{
    internal class TemplateActionFactory : IActionFactory
    {
        private readonly Dictionary<string, List<IActionTemplate>> templates = new Dictionary<string, List<IActionTemplate>>();

        public void RegisterActionTemplate(IActionTemplate template)
        {
            //Validation
            if (template == null)
                throw new ArgumentNullException("template");


            string contextName = template.ContextName;

            List<IActionTemplate> list;
            if (!templates.TryGetValue(contextName, out list))
            {
                list = new List<IActionTemplate>();
                templates.Add(contextName, list);
            }

            list.Add(template);
        }

        public void DeregisterActionTemplate(IActionTemplate template)
        {
            //Validation
            if (template == null)
                throw new ArgumentNullException("template");


            List<IActionTemplate> list;
            if (templates.TryGetValue(template.ContextName, out list))
                list.Remove(template);
        }

        public IEnumerable<IAction> GetActions(IDictionary<string, ContextInfo[]> contextsByName)
        {
            List<IAction> actionList = new List<IAction>();

            foreach (var pair in contextsByName)
            {
                var contextName = pair.Key;
                var contexts = pair.Value;

                if (contexts.Length == 0)
                {
                    //Do nothing
                }
                else
                {
                    List<IActionTemplate> list;
                    if (templates.TryGetValue(contextName, out list))
                    {
                        IEnumerable<IAction> actions = list
                            .Select(template => template.CreateAction(contexts))
                            .Where(action => action != null);

                        actionList.AddRange(actions);
                    }
                }
            }

            actionList.TrimExcess();
            return actionList;
        }
    }
}