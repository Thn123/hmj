﻿using System;

namespace Hmj.Entity.ORMapping
{
    [AttributeUsage(AttributeTargets.Class)]
    public class TableNameAttribute : Attribute
    {
        public TableNameAttribute(string tableName)
        {
            this.TableName = tableName;
        }
        public string TableName { get; private set; }
    }
}
