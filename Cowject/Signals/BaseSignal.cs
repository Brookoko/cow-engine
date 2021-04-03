namespace Cowject.Signals
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    
    public abstract class BaseSignal
    {
        private Action<object[]> listener;
        
        public void Dispatch(object[] objects)
        {
            listener?.Invoke(objects);
        }
        
        public void AddListener(Action<object[]> action)
        {
            if (listener?.GetInvocationList().Contains(action) ?? false) return;
            listener += action;
        }
        
        public void RemoveListener(Action<object[]> action)
        {
            if (!listener?.GetInvocationList().Contains(action) ?? true) return;
            if (listener != null) listener -= action;
        }

        public abstract List<Type> GetTypes();
    }
}