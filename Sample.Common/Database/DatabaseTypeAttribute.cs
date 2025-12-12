using System;

namespace Sample.Common.Database
{
    [AttributeUsage(AttributeTargets.Class)]
    public class DatabaseTypeAttribute : Attribute
    {
        public DatabaseTypeAttribute(DatabaseTypes databaseType)
        {
            DatabaseType = databaseType;
        }

        public DatabaseTypes DatabaseType { get; private set; }
    }
}
