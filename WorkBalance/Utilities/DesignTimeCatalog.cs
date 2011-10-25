using System;
using System.ComponentModel.Composition.Primitives;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Linq.Expressions;
using System.Windows;
using System.ComponentModel;


namespace WorkBalance.Utilities
{
    public class DesignTimeCatalog : ComposablePartCatalog, INotifyComposablePartCatalogChanged
    {
        private readonly ComposablePartCatalog _inner;
        private readonly INotifyComposablePartCatalogChanged _innerNotifyChange;

        public DesignTimeCatalog(ComposablePartCatalog inner)
        {
            _inner = inner;
            _innerNotifyChange = inner as INotifyComposablePartCatalogChanged;
        }

        public event EventHandler<ComposablePartCatalogChangeEventArgs> Changed
        {
            add
            {
                if (_innerNotifyChange != null)
                    _innerNotifyChange.Changed += value;
            }
            remove
            {
                if (_innerNotifyChange != null)
                    _innerNotifyChange.Changed -= value;
            }
        }

        public event EventHandler<ComposablePartCatalogChangeEventArgs> Changing
        {
            add
            {
                if (_innerNotifyChange != null)
                    _innerNotifyChange.Changing += value;
            }
            remove
            {
                if (_innerNotifyChange != null)
                    _innerNotifyChange.Changing -= value;
            }
        }

        public override System.Collections.Generic.IEnumerable<Tuple<ComposablePartDefinition, ExportDefinition>> GetExports(ImportDefinition definition)
        {
            var exports = base.GetExports(definition);
            if (GalaSoft.MvvmLight.ViewModelBase.IsInDesignModeStatic)
            {
                var designExports = exports.Where(export => (export.Item2.Metadata.ContainsKey(DesignTimeExportAttribute.DesignTimeMetadataName) &&
                    ((bool)export.Item2.Metadata[DesignTimeExportAttribute.DesignTimeMetadataName]) == true));

                if (designExports.Count() > 0)
                {
                    exports = designExports;
                }
                else
                {
                    exports = from export in exports
                              let ed = export.Item2
                              where !ed.Metadata.ContainsKey(DesignTimeExportAttribute.DesignTimeMetadataName)
                              select export;
                }
            }
            else
            {
                exports = from export in exports
                          let ed = export.Item2
                          where !ed.Metadata.ContainsKey(DesignTimeExportAttribute.DesignTimeMetadataName) || ((bool)ed.Metadata[DesignTimeExportAttribute.DesignTimeMetadataName]) == false
                          select export;
            }
            var a = exports.ToArray();
            return exports;
        }

        public override IQueryable<ComposablePartDefinition> Parts
        {
            get { return _inner.Parts; }
        }
    }
}