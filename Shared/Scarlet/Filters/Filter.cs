using System;

namespace Scarlet.Filters
{
    ///  
    /// <summary>
    ///  
    ///  This is an interface meant to
    ///  wrap all filters in the Filters namespace.
    ///  
    ///  * * * Some Filters require use of just the
    ///        Feed(T Input) method. Others require
    ///        a given Feed(T Input, T Rate).
    ///        See the documentation for the given
    ///        filter to determine necessary information.
    /// </summary>
    /// 
    /// FILTER
    ///  Filter.cs
    ///  Written by Jaden Bottemiller
    ///  May 31, 2017
    ///  
    ///  Unit testing took place on:
    ///  [   NOT YET   ]
    ///  
    public interface IFilter<T> where T : IComparable
    {
        void Feed(T Input, T Rate); // Feeds filter with input and rate.
        void Feed(T Input); // Feeds filter with just an input.
        T GetOutput();
    }
}
