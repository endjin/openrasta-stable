namespace OpenRasta.TypeSystem.Surrogated
{
    using System.Collections.Generic;
    using System.Linq;

    public abstract class MemberWithSurrogates : WrappedMember
    {
        private readonly IMember wrappedMember;

        protected MemberWithSurrogates(IMember wrappedMember, IEnumerable<IType> alienTypes) : base(wrappedMember)
        {
            this.wrappedMember = wrappedMember;
            this.AlienTypes = alienTypes.Select(x => (IType)new AlienType(x, this)).ToList();
        }

        protected IEnumerable<IType> AlienTypes { get; set; }

        public override IProperty GetIndexer(string parameter)
        {
            return this.CachedProperty(
                parameter,
                () =>
                this.AlienTypes.Select(x => this.Reroot(x.GetIndexer(parameter))).FirstOrDefault() ??
                base.GetIndexer(parameter));
        }

        public override IProperty GetProperty(string name)
        {
            return CachedProperty(
                name,
                () => this.AlienTypes.Select(x => this.Reroot(x.GetProperty(name))).FirstOrDefault() ?? 
                base.GetProperty(name));
        }
    }
}