using System;

namespace TodoList.Core.Security
{
    public class Token
    {
        public string Value { get; set; }
        public int ExpiresIn { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public TokenResolved User { get; set; }
    }
}