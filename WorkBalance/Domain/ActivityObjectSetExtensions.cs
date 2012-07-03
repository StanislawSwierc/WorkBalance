using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using WorkBalance.Infrastructure;

namespace WorkBalance.Domain
{
    public static class ActivityObjectSetExtensions
    {
        public static IQueryable<Activity> FetchWithTags(this IObjectSet<Activity> set)
        {
            Contract.Requires<ArgumentNullException>(set != null, "set");

            return set.FetchWith(a => a.Tags);
        }
    }
}