using System;

namespace ClipHost.ServiceModel
{
    public class TableUpAttribute : Attribute
    {
        private readonly int order;

        public TableUpAttribute(int order)
        {
            this.order = order;
        }

        public int Order => order;
    }
}