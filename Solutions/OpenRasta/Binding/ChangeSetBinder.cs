namespace OpenRasta.Binding
{
    #region Using Directives

    using System;

    using OpenRasta.Contracts.TypeSystem;
    using OpenRasta.Data;

    #endregion

    public class ChangeSetBinder<T> : KeyedValuesBinder where T : class
    {
        public ChangeSetBinder(IType type, string objectName) : base(type, objectName)
        {
        }

        public override BindingResult BuildObject()
        {
            return BindingResult.Success(new ChangeSet<T>(Builder));
        }

        public override bool SetInstance(object builtInstance)
        {
            // TODO: Flatten the object back into key/value pairs to support changeset for codecs that don't support IKeyedValuesMediaTypeReader
            throw new InvalidOperationException("Cannot use an object instance for a changeset parameter.");
        }
    }
}