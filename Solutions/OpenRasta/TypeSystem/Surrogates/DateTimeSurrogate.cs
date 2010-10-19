namespace OpenRasta.TypeSystem.Surrogates
{
    using System;

    public class DateTimeSurrogate : AbstractStaticSurrogate<DateTime>
    {
        private DateTime value = DateTime.MinValue;

        public int Day
        {
            get { return Value.Day; }
            set { Value = new DateTime(Value.Year, Value.Month, value, Value.Hour, Value.Minute, Value.Second, Value.Millisecond, Value.Kind); }
        }

        public int Hour
        {
            get { return Value.Hour; }
            set { Value = new DateTime(Value.Year, Value.Month, Value.Day, value, Value.Minute, Value.Second, Value.Millisecond, Value.Kind); }
        }

        public int Millisecond
        {
            get { return Value.Millisecond; }
            set { Value = new DateTime(Value.Year, Value.Month, Value.Day, Value.Hour, Value.Minute, Value.Second, value, Value.Kind); }
        }

        public int Minute
        {
            get { return Value.Minute; }
            set { Value = new DateTime(Value.Year, Value.Month, Value.Day, Value.Hour, value, Value.Second, Value.Millisecond, Value.Kind); }
        }

        public int Month
        {
            get { return Value.Month; }
            set { Value = new DateTime(Value.Year, value, Value.Day, Value.Hour, Value.Minute, Value.Second, Value.Millisecond, Value.Kind); }
        }

        public int Second
        {
            get { return Value.Second; }
            set { Value = new DateTime(Value.Year, Value.Month, Value.Day, Value.Hour, Value.Minute, value, Value.Millisecond, Value.Kind); }
        }

        public int Year
        {
            get { return Value.Year; }
            set { Value = new DateTime(value, Value.Month, Value.Day, Value.Hour, Value.Minute, Value.Second, Value.Millisecond, Value.Kind); }
        }
    }
}