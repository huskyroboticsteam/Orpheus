using System;
using System.Collections.Generic;

namespace Scarlet.IO.BeagleBone
{
    public static class BBBPinManager
    {
        private static Dictionary<BBBPin, PinAssignment> Mappings;

        private class PinAssignment
        {
            private PinAssignment(BBBPin Pin, byte Mode)
            {

            }
        }
    }
}
