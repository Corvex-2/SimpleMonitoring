using SimpleMonitoring.Communication.TCP.Attributes;
using SimpleMonitoring.Communication.TCP.Messages;
using SimpleMonitoring.Utilites.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace SimpleMonitoring.Communication.TCP.Shared.Protocols
{
    public abstract class MessageDispatcher<TMessageType> where TMessageType : class, new()
    {
        readonly List<(RoutingAttribute routing, Func<TMessageType, Task<TMessageType>> targetMethod)> _handlers = new List<(RoutingAttribute routing, Func<TMessageType, Task<TMessageType>> targetMethod)>();

        public void Bind<TProtocol>(Channel<TProtocol, TMessageType> Channel) where TProtocol : Protocol<TMessageType>, new() => Channel.OnMessage(async m =>
            {
                var response = await DispatchAsync(m).ConfigureAwait(false);
                try
                {
                    if (response != null)
                        await Channel.SendAsync(response).ConfigureAwait(false); //THIS WILL NOT WORK.
                }
                catch (Exception ex)
                {
                    //Log.Create(ex.Message, LoggingOptions.AllowConsole | LoggingOptions.AllowGlobal | LoggingOptions.AllowFile);
                }
            });
        public void Bind<TController>()
        {
            foreach (var func in typeof(TController).GetMethods(BindingFlags.Public | BindingFlags.Static).Where(x => (x.GetParameters().Count() == 1 && x.GetParameters()[0].ParameterType?.BaseType == typeof(Message) && HasRouting(x) && (x.IsFromTask() || x.IsFromTTask()))).ToArray())
            {
                var wrapper = new Func<TMessageType, Task<TMessageType>>(async msg =>
                {
                    var @param = Deserialize(func.GetParameters()[0].ParameterType, msg);
                    try
                    {
                        if (func.IsFromTask())
                        {
                            var t = (func.Invoke(null, new object[] { @param }) as Task);
                            if (t != null)
                                await t;
                            return null;
                        }
                        else
                        {
                            var t = (await (func.Invoke(null, new object[] { @param }) as dynamic) as dynamic);
                            if (t != null)
                                return Serialize(t as dynamic);
                            else
                                return null;
                        }
                    }
                    catch (Exception ex)
                    {
                        //Log.Create(ex.Message, LoggingOptions.AllowConsole | LoggingOptions.AllowFile | LoggingOptions.AllowGlobal | LoggingOptions.AllowHandlers);
                        return null;
                    }

                });

                if (GetRouting(func, out var route))
                    AddHandler(route, wrapper);
            }
        }

        public async Task<TMessageType> DispatchAsync(TMessageType Message)
        {
            foreach (var (routing, target) in _handlers)
            {
                if (MatchRouting(routing, Message))
                    return await target(Message);
            }
            //No Handlers
            return null;
        }
        public virtual void Register<TParam, TResult>(Func<TParam, Task<TResult>> target)
        {
            var Wrapper = new Func<TMessageType, Task<TMessageType>>(async xml =>
            {
                var @param = Deserialize<TParam>(xml);
                var result = await target(param);
                if (result != null)
                    return Serialize(result);
                else
                    return null;
            });

            if (GetRouting(target.Method, out var route))
                AddHandler(route, Wrapper);
            else
                throw new Exception("Missing RoutingAttribute on target.");
        }
        public virtual void Register<TParam>(Func<TParam, Task> target)
        {
            var Wrapper = new Func<TMessageType, Task<TMessageType>>(async xml =>
            {
                var @param = Deserialize<TParam>(xml);
                await target(param);
                return null;
            });

            if (GetRouting(target.Method, out var route))
                AddHandler(route, Wrapper);
            else
                throw new Exception("Missing RoutingAttribute on target.");
        }

        protected void AddHandler(RoutingAttribute routing, Func<TMessageType, Task<TMessageType>> target) => _handlers.Add((routing, target));
        protected abstract bool MatchRouting(RoutingAttribute routing, TMessageType messageType);
        protected abstract TMessageType Serialize<TResult>(TResult result);
        protected abstract TParam Deserialize<TParam>(TMessageType Message);
        protected abstract object Deserialize(Type paramType, TMessageType message);
        protected abstract bool GetRouting(MethodInfo methodInfo, out RoutingAttribute route);

        private bool HasRouting(MethodInfo methodInfo) => methodInfo.GetCustomAttributes<RoutingAttribute>().Count() > 0;
    }
}
