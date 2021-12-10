using System;

namespace SimpleMonitoring.Communication.TCP.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class JsonRoutingAttribute : RoutingAttribute
    {
        public string Value { get; }

        public JsonRoutingAttribute(string path, string value) : base(path) => Value = value;
    }
}
