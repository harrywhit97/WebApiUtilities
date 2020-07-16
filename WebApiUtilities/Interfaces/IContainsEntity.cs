using System;
using System.Collections.Generic;
using System.Text;

namespace WebApiUtilities.Interfaces
{
    public interface IContainsEntity<T>
    {
        public T Entity { get; set; }
    }
}
