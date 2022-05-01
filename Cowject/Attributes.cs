namespace Cowject
{
    using System;
    using JetBrains.Annotations;

    [AttributeUsage(AttributeTargets.Property)]
    [MeansImplicitUse(ImplicitUseKindFlags.Assign)]
    public class InjectAttribute : Attribute
    {
        public object Name;
    }

    [AttributeUsage(AttributeTargets.Method)]
    [MeansImplicitUse]
    public class PostConstructAttribute : Attribute
    {
    }
}
