namespace OpenRasta.Binding
{
    public class BindingResult
    {
        public BindingResult(bool successfull, object instance)
        {
            this.Successful = successfull;

            if (successfull)
            {
                this.Instance = instance;
            }
        }

        public object Instance { get; private set; }

        public bool Successful { get; private set; }

        public static BindingResult Failure()
        {
            return new BindingResult(false, null);
        }

        public static BindingResult Success(object instance)
        {
            return new BindingResult(true, instance);
        }
    }
}