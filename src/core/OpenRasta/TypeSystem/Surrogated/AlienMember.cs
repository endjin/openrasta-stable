namespace OpenRasta.TypeSystem.Surrogated
{
    public abstract class AlienMember : WrappedMember
    {
        private readonly IMember realMember;

        public AlienMember(IMember alienMember, IMember realMember) : base(alienMember)
        {
            this.realMember = realMember;
        }

        public override IProperty GetIndexer(string parameter)
        {
            return this.WrapProperty(base.GetIndexer(parameter));
        }

        public override IProperty GetProperty(string name)
        {
            return this.WrapProperty(base.GetProperty(name));
        }

        private IProperty WrapProperty(IProperty result)
        {
            if (result == null)
            {
                return null;
            }

            if (!(result is AlienOwnedProperty))
            {
                return new AlienOwnedProperty(this, this.realMember, result);
            }

            return result;
        }
    }
}