using System;

namespace SimpleMonitoring.Communication.TCP.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public abstract class RoutingAttribute : Attribute
    {
        public string Routing { get; }

        public RoutingAttribute(string path) => Routing = path;
    }
}
