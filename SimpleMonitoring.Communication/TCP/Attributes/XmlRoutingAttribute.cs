using System;

namespace SimpleMonitoring.Communication.TCP.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class XmlRoutingAttribute : RoutingAttribute
    {
        public XmlRoutingAttribute(string path) : base(path) { }
    }
}
