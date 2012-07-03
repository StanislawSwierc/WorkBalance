using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WorkBalance.Domain;

namespace WorkBalance.Services
{
    public interface IEditActivityService
    {
        void EditActivity(IDomainContext context, Activity activity);
    }
}
