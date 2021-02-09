using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GXPEngine
{
    public static class CustomFunctions
    {

        public static void Print(Object msg)
        {
            msg = msg.ToString();
            Console.WriteLine(msg);
        }

    }
}
