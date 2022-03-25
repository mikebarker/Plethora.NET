using System;
using System.Collections.Generic;
using System.Text;

namespace Plethora.Mvvm.Binding
{
    public static class Binding
    {
        public static IBindingObserver CreateObserver<T>(T item, string bindingPath)
        {
            var elements = Parse(bindingPath);

            IBindingSetter<T> root = null;
            IBindingObserver leaf = null;

            Type valueType = null;
            foreach (var element in elements)
            {
                Type observedType = (root == null)
                    ? typeof(T)
                    : valueType;

                var observer = element.CreateObserver(observedType);

                if (root == null)
                {
                    root = (IBindingSetter<T>)observer;
                }

                observer.SetParent(leaf);
                leaf = observer;

                valueType = observer.GetType().GetGenericArguments()[1];
            }

            var bindingObserver = BindingObserver.Create(root, leaf);
            ((IBindingSetter)bindingObserver).SetObserved(item);
            return bindingObserver;
        }

        private enum ParsingState
        {
            None,
            InIndexer,
        }

        public static IEnumerable<BindingElement> Parse(string bindingPath)
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
                        yield return new PropertyBindingElement(propertyName);
                    }
                }
                else if (c == '[')
                {
                    if (sb.Length != 0)
                    {
                        var propertyName = sb.ToString();
                        sb.Clear();
                        yield return new PropertyBindingElement(propertyName);
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

                    var indexerArgument = new IndexerArgument(indexerValue);
                    yield return new IndexerBindingElement(new[] { indexerArgument });
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
                yield return new PropertyBindingElement(propertyName);
            }
        }
    }
}
