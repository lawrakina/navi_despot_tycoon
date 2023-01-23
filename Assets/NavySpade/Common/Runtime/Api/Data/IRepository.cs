using System.Collections.Generic;

namespace NavySpade.Common.Runtime.Api.Data
{
    public interface IRepository<T>
    {
        void Add(T element);

        void Remove(T element);

        void Clear();

        IEnumerable<T> GetAll();
    }
}