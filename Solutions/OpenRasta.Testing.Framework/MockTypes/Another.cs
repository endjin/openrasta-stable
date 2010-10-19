namespace OpenRasta.Testing.Framework.MockTypes
{
    public class Another : IAnother
    {
        public Another(ISimple simple) { Dependent = simple; }
        public ISimple Dependent { get; set; }
    }
}