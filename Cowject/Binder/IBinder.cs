namespace Cowject
{
    using System;

    public interface IBinder
    {
        IBinder Bind<T>();

        IBinder Bind(Type type);

        IBinder WithName(object name);

        IBinder To<T>() where T : class, new();

        IBinder To(Type type);

        void ToInstance(object obj);

        void ToSingleton();
    }
}
