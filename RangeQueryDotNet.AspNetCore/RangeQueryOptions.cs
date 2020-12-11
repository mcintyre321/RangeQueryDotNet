using System;

namespace RangeQueryDotNet.AspNetCore
{
    public class RangeQueryOptions
    {
        public string RangeKey { get; set; }
        public Func<(string query, string responseContent), string> Transform { get; set; }
    }
}