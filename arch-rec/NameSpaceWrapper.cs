
    public class NamespaceWrapper
    {
        public NameSpace NameSpace { get; set; }
        private int count;

        public int Count
        {
            get { return count; }
            set { count = value; }
        }

        public NamespaceWrapper(NameSpace ns)
        {
            NameSpace = ns;
            count = 1;
        }
    }
