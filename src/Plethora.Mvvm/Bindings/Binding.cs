using System;
using System.Collections.Generic;
using System.Text;

namespace Plethora.Mvvm.Bindings;

public static class Binding
{
    private static readonly IGetterProvider getterProvider = new CachedGetterProvider(new GetterProvider());

    public static IBindingObserver CreateObserver<T>(T item, IEnumerable<BindingElementDefinition> elements)
    {
        IBindingObserver? root = null;
        IBindingObserver? leaf = null;

        foreach (var element in elements)
        {
            var observer = element.CreateObserver(getterProvider);

            root ??= observer;

            observer.SetParent(leaf);
            leaf = observer;
        }

        if (root == null || leaf == null)
        {
            throw new InvalidOperationException("Binding must have at least one element.");
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

        StringBuilder sb = new();
        Stack<ParsingState> stateStack = new();
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
                yield return new IndexerBindingElementDefinition([indexerArgument]);
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
