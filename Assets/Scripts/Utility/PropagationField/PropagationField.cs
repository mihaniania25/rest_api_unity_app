using System;
using System.Collections.Generic;
using UnityEngine;

namespace TABApps.TestTask
{
    [Serializable]
    public class PropagationField<T>
    {
        [SerializeField] protected T _value;

        private List<Action<T>> _handlers = new List<Action<T>>();

        public PropagationField()
        {

        }

        public PropagationField(T initValue)
        {
            _value = initValue;
        }

        public T Value
        {
            get => GetValue();
            set
            {
                SetValue(value);
                Propagate();
            }
        }

        protected virtual T GetValue()
        {
            return _value;
        }

        protected virtual void SetValue(T value)
        {
            _value = value;
        }

        public void Propagate()
        {
            List<Action<T>> handlers = new List<Action<T>>(_handlers);
            handlers.ForEach(h => h?.Invoke(Value));
        }

        public void Subscribe(Action<T> handler, bool propagateImmediately = true)
        {
            _handlers.Add(handler);

            if (propagateImmediately)
                handler?.Invoke(Value);
        }

        public void Unsubscribe(Action<T> handler)
        {
            _handlers.Remove(handler);
        }

        public void SetValueSilently(T value)
        {
            SetValue(value);
        }
    }
}