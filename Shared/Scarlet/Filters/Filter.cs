using System;

namespace Scarlet.Filters
{
    ///  
    /// <summary>
    ///  FILTER
    ///  Filter.cs
    ///  Written by Jaden Bottemiller
    ///  May 31, 2017
    ///  
    ///  Unit testing took place on:
    ///  [   NOT YET   ]
    ///  
    ///  This is an interface meant to
    ///  wrap all filters in the Filters namespace.
    ///  
    ///  * * * Filters requires the RoboticsLibrary.Utility Namespace.
    ///  
    ///  * * * Some Filters require use of just the
    ///        Feed(T Input) method. Others require
    ///        a given Feed(T Input, T Rate).
    ///        See the documentation for the given
    ///        filter to determine necessary information.
    /// </summary>
    ///
    interface IFilter<T> where T : IComparable
    {

        void Feed(T Input, T Rate); // Feeds filter with input and rate.
        void Feed(T Input); // Feeds filter with just an input.

    }
}
