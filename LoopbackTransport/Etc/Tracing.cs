using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace LoopbackTransport
{

    public static class Tracing
    {
        public static TraceSwitch LBSwitch;

        static Tracing()
        {
            LBSwitch = new TraceSwitch("Loopback", "Loopback transport traces.", "Verbose");
            System.Diagnostics.Trace.AutoFlush = true;
        }
    }

}
