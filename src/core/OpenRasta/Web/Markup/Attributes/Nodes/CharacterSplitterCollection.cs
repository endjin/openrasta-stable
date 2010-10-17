namespace OpenRasta.Web.Markup.Attributes.Nodes
{
    using System;
    using System.Collections.ObjectModel;

    public class CharacterSplitterCollection : Collection<string>
    {
        private readonly string separator;

        public CharacterSplitterCollection(string separator)
        {
            this.separator = separator;
        }

        protected override void InsertItem(int index, string item)
        {
            foreach (var i in item.Split(new[] { this.separator }, StringSplitOptions.RemoveEmptyEntries))
            {
                base.InsertItem(index, i);
            }
        }

        protected override void SetItem(int index, string item)
        {
            foreach (var i in item.Split(new[] { this.separator }, StringSplitOptions.RemoveEmptyEntries))
            {
                base.SetItem(index, i);
            }
        }
    }
}