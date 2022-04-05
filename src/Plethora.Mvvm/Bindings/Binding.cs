using System;
using System.Collections.Generic;
using System.Text;

namespace Plethora.Mvvm.Bindings
{
    public static class Binding
    {
        private static IGetterProvider getterProvider = new CachedGetterProvider(new GetterProvider());

        public static IBindingObserver CreateObserver<T>(T item, IEnumerable<BindingElementDefinition> elements)
        {
            IBindingObserver root = null;
            IBindingObserver leaf = null;

            foreach (var element in elements)
            {
                var observer = element.CreateObserver(getterProvider);

                if (root == null)
                {
                    root = observer;
                }

                observer.SetParent(leaf);
                leaf = observer;
            }

            IBindingObserver bindingObserver;
            if (ReferenceEquals(root, leaf))
            {
                bindingObserver = root;
            }
            else
            {
                bindingObserver = new BindingObserver(root, leaf);
            }
            bindingObserver.SetObserved(item);
            return bindingObserver;
        }

        private enum ParsingState
        {
            None,
            InIndexer,
        }

        public static IEnumerable<BindingElementDefinition> Parse(string bindingPath)
        {
            // TODO: Only supports single-argument indexers. Should also support types and multi-argument indexers
            // See: https://docs.microsoft.com/en-us/dotnet/desktop/wpf/data/binding-declarations-overview?view=netdesktop-6.0#binding-path-syntax

            StringBuilder sb = new StringBuilder();
            Stack<ParsingState> stateStack = new Stack<ParsingState>();
            foreach (char c in bindingPath)
            {
                if (c == '.')
                {
                    if (sb.Length != 0)
                    {
                        var propertyName = sb.ToString();
                        sb.Clear();
                        yield return new PropertyBindingElementDefinition(propertyName);
                    }
                }
                else if (c == '[')
                {
                    if (sb.Length != 0)
                    {
                        var propertyName = sb.ToString();
                        sb.Clear();
                        yield return new PropertyBindingElementDefinition(propertyName);
                    }

                    stateStack.Push(ParsingState.InIndexer);
                }
                else if (c == ']')
                {
                    if (stateStack.Pop() != ParsingState.InIndexer)
                    {
                        // TODO: Better error message
                        throw new Exception("Bad juju");
                    }

                    var indexerValue = sb.ToString();
                    sb.Clear();

                    var indexerArgument = new IndexerBindingElementDefinition.Argument(indexerValue);
                    yield return new IndexerBindingElementDefinition(new[] { indexerArgument });
                }
                else
                {
                    sb.Append(c);
                }
            }

            if (sb.Length != 0)
            {
                var propertyName = sb.ToString();
                sb.Clear();
                yield return new PropertyBindingElementDefinition(propertyName);
            }
        }
    }
}
