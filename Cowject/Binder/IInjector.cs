namespace Cowject
{
    using System;
    using System.Collections.Generic;

    internal interface IInjector
    {
        object Inject(object obj);

        object Inject(object obj, List<object> parameters);

        T Get<T>(object name = null) where T : class;

        T Get<T>(IEnumerable<object> parameters, object name = null) where T : class;

        object Get(Type type, object name = null);

        object Get(Type type, IEnumerable<object> parameters, object name = null);
    }
}
