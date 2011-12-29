using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using System.Windows.Input;

namespace WorkBalance.Utilities
{
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    sealed class KeyGestureCommandExportAttribute : ExportAttribute, IKeyGestureCommandMetadata
    {
        public KeyGestureCommandExportAttribute(Key key, ModifierKeys modifierKeys)
            : base(typeof(ICommand))
        {
            Key = key;
            ModifierKeys = modifierKeys;
        }
        public Key Key { get; private set; }
        public ModifierKeys ModifierKeys { get; private set; }
    }

    public interface IKeyGestureCommandMetadata
    {
        Key Key { get; }
        ModifierKeys ModifierKeys { get; }
    }
}
