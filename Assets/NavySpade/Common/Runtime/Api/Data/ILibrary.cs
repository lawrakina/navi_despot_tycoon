using System.Collections.Generic;

namespace NavySpade.Common.Runtime.Api.Data
{
    public interface ILibrary<out T>
    {
        IEnumerable<T> GetAll();
    }
}