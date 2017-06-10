using System;
using System.Collections.Generic;
using System.Text;

/*  
 *  FILTER
 *  Filter.cs
 *  Written by Jaden Bottemiller
 *  May 31, 2017
 *  
 *  Unit testing took place on:
 *  [   NOT YET   ]
 *  
 *  This is an interface meant to
 *  wrap all filters in the Filters namespace.
 *  
 *  * * * Filters requires the Utility Namespace
 *  
 */

namespace RoboticsLibrary.Filters
{
    interface IFilter<T> where T : IComparable
    {

        void Feed(T Input, T Rate);
        void Feed(T Input);

    }
}
