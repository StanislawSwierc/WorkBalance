using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WorkBalance.Domain
{
    public enum InterruptionType
    {
        /// <summary>
        /// Internal, immediate need to interrupt the activity at hand
        /// </summary>
        Internal,
        /// <summary>
        /// Interruptions in which we need to interact with other people
        /// </summary>
        External
    }
}
