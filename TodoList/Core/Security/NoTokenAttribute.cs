using System;

namespace TodoList.Core.Security
{
    [AttributeUsageAttribute(AttributeTargets.Method)]
    public class NoTokenAuthAttribute : Attribute
    {
        
    }
}