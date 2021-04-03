namespace Cowject.Signals
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    
    public class Signal : BaseSignal
    {
        private Action listener;
        
        public void Dispatch()
        {
            listener?.Invoke();
            base.Dispatch(new object[] { });
        }

        public void AddListener(Action action)
        {
            if (listener?.GetInvocationList().Contains(action) ?? false) return;
            listener += action;
        }

        public void RemoveListener(Action action)
        {
            if (!listener?.GetInvocationList().Contains(action) ?? true) return;
            if (listener != null) listener -= action;
        }

        public override List<Type> GetTypes()
        {
            return new List<Type>();
        }
    }
    
    public class Signal<T> : BaseSignal
    {
        private Action<T> listener;
        
        public void Dispatch(T arg0)
        {
            listener?.Invoke(arg0);
            base.Dispatch(new object[] { arg0 });
        }

        public void AddListener(Action<T> action)
        {
            if (listener?.GetInvocationList().Contains(action) ?? false) return;
            listener += action;
        }

        public void RemoveListener(Action<T> action)
        {
            if (!listener?.GetInvocationList().Contains(action) ?? true) return;
            if (listener != null) listener -= action;
        }

        public override List<Type> GetTypes()
        {
            return new List<Type>
            {
                typeof(T)
            };
        }
    }
    
    public class Signal<T, T1> : BaseSignal
    {
        private Action<T, T1> listener;
        
        public void Dispatch(T arg0, T1 arg1)
        {
            listener?.Invoke(arg0, arg1);
            base.Dispatch(new object[] { arg0, arg1 });
        }

        public void AddListener(Action<T, T1> action)
        {
            if (listener?.GetInvocationList().Contains(action) ?? false) return;
            listener += action;
        }

        public void RemoveListener(Action<T, T1> action)
        {
            if (!listener?.GetInvocationList().Contains(action) ?? true) return;
            if (listener != null) listener -= action;
        }

        public override List<Type> GetTypes()
        {
            return new List<Type>
            {
                typeof(T),
                typeof(T1)
            };
        }
    }
    
    public class Signal<T, T1, T2> : BaseSignal
    {
        private Action<T, T1, T2> listener;
        
        public void Dispatch(T arg0, T1 arg1, T2 arg2)
        {
            listener?.Invoke(arg0, arg1, arg2);
            base.Dispatch(new object[] { arg0, arg1, arg2 });
        }

        public void AddListener(Action<T, T1, T2> action)
        {
            if (listener?.GetInvocationList().Contains(action) ?? false) return;
            listener += action;
        }

        public void RemoveListener(Action<T, T1, T2> action)
        {
            if (!listener?.GetInvocationList().Contains(action) ?? true) return;
            if (listener != null) listener -= action;
        }

        public override List<Type> GetTypes()
        {
            return new List<Type>
            {
                typeof(T),
                typeof(T1),
                typeof(T2)
            };
        }
    }
}