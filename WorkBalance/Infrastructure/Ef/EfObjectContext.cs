using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.EntityClient;
using System.Data.Objects;
using System.Linq;
using System.Text;

namespace WorkBalance.Infrastructure.Ef
{
    public class EfObjectContext : DbContext, IObjectContext
    {
        protected EfObjectContext()
        {
        }

        protected EfObjectContext(DbCompiledModel model)
            : base(model)
        {
        }

        public EfObjectContext(string nameOrConnectionString)
            :base(nameOrConnectionString)
        {
        }

        public EfObjectContext(string nameOrConnectionString, DbCompiledModel model)
            :base(nameOrConnectionString, model)
        {
            
        }

        public EfObjectContext(DbConnection existingConnection, bool contextOwnsConnection)
            : base(existingConnection, contextOwnsConnection)
        {
        }

        public EfObjectContext(DbConnection existingConnection, DbCompiledModel model, bool contextOwnsConnection)
            : base(existingConnection, model, contextOwnsConnection)
        {
        }

        public EfObjectContext(ObjectContext objectContext, bool dbContextOwnsObjectContext)
            :base(objectContext, dbContextOwnsObjectContext)
        {
        }

        public void Commit()
        {
            SaveChanges();
        }
    }
}
