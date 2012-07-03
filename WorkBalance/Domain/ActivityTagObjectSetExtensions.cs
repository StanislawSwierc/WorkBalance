using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using WorkBalance.Infrastructure;

namespace WorkBalance.Domain
{
    public static class ActivityTagObjectSetExtensions
    {
        public static ICollection<ActivityTag> GetOrCreate(this IObjectSet<ActivityTag> set, string[] names)
        {
            Contract.Requires<ArgumentNullException>(set != null, "set");
            Contract.Requires<ArgumentNullException>(names != null, "names");

            var dictionary = names.ToDictionary(tn => tn, tn => default(ActivityTag));

            foreach (var tag in set.Where(t => names.Contains(t.Name)))
            {
                dictionary[tag.Name] = tag;
            }

            foreach (var tagName in dictionary.Keys.Where(k => dictionary[k] == null).ToList())
            {
                var tag = new ActivityTag() { Name = tagName };
                dictionary[tagName] = tag;
                set.Add(tag);
            }

            return dictionary.Values.ToList();
        }
    }
}