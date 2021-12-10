using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Windows;

namespace SimpleMonitoring.Utilites.Extensions
{
    public static class TypeExtensions
    {
        public static bool IsValid<T>(this T e)
        {
            return (e != null);
        }
        public static bool IsSerializable<T>(this T e)
        {
            return ((e is ISerializable) || (Attribute.IsDefined(typeof(T), typeof(SerializableAttribute))));
        }
        public static byte[] GetBinaryData(this HashAlgorithm e, object Obj)
        {
            BinaryFormatter format = new BinaryFormatter();
            using (var stream = new MemoryStream())
            {
                format.Serialize(stream, Obj);
                return stream.ToArray();
            }
        }
        public static bool IsFromTask(this MethodInfo m)
        {
            return m.ReturnType.IsAssignableFrom(typeof(Task));
        }
        public static bool IsFromTTask(this MethodInfo m)
        {
            return m.ReturnType.BaseType?.IsAssignableFrom(typeof(Task)) ?? false;
        }
        public static IEnumerable<T> GetChildren<T>(this FrameworkElement frameworkElement, bool recurse)
        {
            if (frameworkElement != null)
            {
                foreach (var c in LogicalTreeHelper.GetChildren(frameworkElement))
                {
                    if (c.GetType().IsSubclassOf(typeof(T)) || c.GetType() == typeof(T))
                        yield return (T)c;

                    if (recurse && c.GetType().IsSubclassOf((typeof(FrameworkElement))) || c.GetType() == typeof(FrameworkElement))
                    {
                        foreach (var gc in (c as FrameworkElement).GetChildren<T>(true))
                        {
                            if (gc.GetType().IsSubclassOf(typeof(T)) || gc.GetType() == typeof(T))
                                yield return (T)gc;
                        }
                    }
                }
            }
        }
    }
}
