using System;

namespace Sample.Common.Domain
{
    [AttributeUsage(AttributeTargets.Class)]
    public class SequenceAttribute : Attribute
    {
        public SequenceAttribute(string sequencename)
        {
            Sequencename = sequencename;
        }

        public string Sequencename { get; private set; }
    }
}
