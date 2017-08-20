using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scarlet.Utilities
{
    public static class UtilConstants
    {
        public static string SEARCH_NOT_FOUND_STR = "*-+{}SEARCH_NOT_FOUND?/\\";
    }

    public enum SearchType
    {
        BreadthFirst,
        DepthFirst,
        SingleFolder,
    }
}
