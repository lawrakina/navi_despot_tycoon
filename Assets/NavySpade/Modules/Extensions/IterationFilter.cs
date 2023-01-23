using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Extensions
{
    public static class IterationFilterExtensions
    {
        public static bool NeedIgnore<T>(this IterationFilter<T> filter, T component)
        {
            return filter.Ignoring != null && filter.Ignoring.Contains(component) ||
                   filter.InvokeConditions(component) == false;
        }
    }
    
    public class IterationFilter<T>
    {
        public bool IsFirstResult;
        [CanBeNull] public HashSet<T> Ignoring;
        [CanBeNull] public List<Predicate<T>> Conditions { get; private set; }

        public IterationFilter<T> First()
        {
            First(true);
            return this;
        }
        
        public IterationFilter<T> First(bool isFirst)
        {
            IsFirstResult = isFirst;
            return this;
        }

        public IterationFilter<T> Ignore(HashSet<T> array)
        {
            if(Ignoring == null)
            {
                Ignoring = array;
                return this;
            }

            foreach (var t in array)
            {
                Ignoring.Add(t);
            }
            
            return this;
        }

        public IterationFilter<T> Where(Predicate<T> condition)
        {
            Conditions ??= new List<Predicate<T>>();
            Conditions.Add(condition);
            
            return this;
        }

        /// <summary>
        /// If any condition dont work this return false.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool InvokeConditions(T obj)
        {
            if (Conditions == null)
            {
                return true;
            }

            for (var i = 0; i < Conditions.Count; i++)
            {
                var condition = Conditions[i];

                if (condition.Invoke(obj) == false)
                    return false;
            }

            return true;
        }
    }
}