using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;

namespace WorkBalance.Utilities
{
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    sealed class DesignTimeExportAttribute : ExportAttribute
    {
        public const string DesignTimeMetadataName = "DesignTime";

        public DesignTimeExportAttribute(Type contractType)
            : base(contractType)
        {
        }

        // This is a named argument
        public bool DesignTime { get; set; }
    }
}
