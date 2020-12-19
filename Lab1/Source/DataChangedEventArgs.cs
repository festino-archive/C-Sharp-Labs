namespace Lab1
{
    public enum ChangeInfo
    {
        ItemChanged, Add, Remove, Replace
    }
    public delegate void DataChangedEventHandler(object source, DataChangedEventArgs args);
    public class DataChangedEventArgs
    {
        public ChangeInfo Type { get; set; }
        public string Info { get; set; }

        public DataChangedEventArgs(ChangeInfo type, string info)
        {
            Type = type;
            Info = info;
        }

        public override string ToString()
        {
            return $"{{Type={Type}, Info=\"{Info}\"}}";
        }
    }
}
