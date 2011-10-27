using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WorkBalance.Domain;

namespace WorkBalance.Contracts
{
    public interface ISprintListener
    {
        void OnSprintStarted(Sprint sprint);
        void OnSprintEnded(Sprint sprint);
    }
}
