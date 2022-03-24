using System;
using System.Collections.Generic;
using System.Text;

namespace Plethora.Mvvm.Binding
{
    public static class Binding
    {
        public static IBindingObserver<object> CreateObserver<T>(T item, string bindingPath)
        {
            var elements = Parse(bindingPath);

            IBindingSetter<object> root = null;
            IBindingObserver<object> leaf = null;
            foreach (var element in elements)
            {
                var observer = element.CreateObserver();

                if (root == null)
                {
                    root = observer;
                }

                observer.SetParent(leaf);
                leaf = observer;
            }

            var bindingObserver = new BindingObserver<object, object>(root, leaf);
            bindingObserver.SetObserved(item);
            return bindingObserver;
        }

        private enum ParsingState
        {
            None,
            InIndexer,
        }

        public static IEnumerable<BindingElement> Parse(string bindingPath)
        {
            // TODO: Only supports properties. Enhance to support indexers.
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
