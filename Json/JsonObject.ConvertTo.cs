using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Globalization;

namespace UyghurEditPP
{
    public partial class JsonObject
    {
        private enum ConvertMode
        {
            Array,
            List,
            Object
        }

        private static InvalidCastException InvalidCastException(InternalObject obj, Type type)
        {
        	return new InvalidCastException(string.Format("Unable to cast value of type {0} to type '{1}'",obj.Type,type.Name));
        }

        private static class ConvertTo
        {
            internal static object Convert(InternalObject obj, Type type)
            {
                if (type == typeof(IEnumerable))
                    return ConvertToIEnumerable(obj);
                return ConvertToObject(obj, type);
            }

            private class Context
            {
                public ConvertMode Mode;
                public JsonArray.Enumerator JsonArrayEnumerator;
                public SetterEnumerator SetterEnumerator;
                public object DstObject;
                public dynamic DstArray;
                public Type Type;
            }

            private static object ConvertToObject(InternalObject obj, Type type)
            {
                var stack = new Stack<Context>();
                var context = new Context();
                object result;

                Convert:
                if (type == typeof(object))
                {
                    result = ToValue(obj);
                    goto Return;
                }
                switch (obj.Type)
                {
                    case JsonType.Null:
                        result = System.Convert.ChangeType(ToValue(obj), type, CultureInfo.InvariantCulture);
                        break;
                    case JsonType.True:
                    case JsonType.False:
                        result = type == typeof(bool)
                            ? obj.Type == JsonType.True
                            : System.Convert.ChangeType(ToValue(obj), type, CultureInfo.InvariantCulture);
                        break;
                    case JsonType.String:
                        result = type == typeof(string) ? obj.String : System.Convert.ChangeType(ToValue(obj), type, CultureInfo.InvariantCulture);
                        break;
                    case JsonType.Array:
                        stack.Push(context);
                        if (!type.IsArray && !IsGenericList(type))
                            throw InvalidCastException(obj, type);
                        if (type.IsArray)
                        {
                            var element = type.GetElementType();
                            context = new Context
                            {
                                Mode = ConvertMode.Array,
                                Type = element,
                                DstArray = Array.CreateInstance(element, obj.Array.Count)
                            };
                        }
                        else
                        {
                            context = new Context
                            {
                                Mode = ConvertMode.List,
                                Type = type.GetGenericArguments()[0],
                                DstArray = ReflectiveOperation.GetObjectCreator(type)()
                            };
                        }
                        context.JsonArrayEnumerator = obj.Array.GetEnumerator();
                        goto ArrayNext;
                    case JsonType.Object:
                        if (type.IsArray)
                            throw InvalidCastException(obj, type);
                        stack.Push(context);
                        context = new Context
                        {
                            Mode = ConvertMode.Object,
                            DstObject = ReflectiveOperation.GetObjectCreator(type)(),
                            SetterEnumerator = new SetterEnumerator(type, obj.Dictionary)
                        };
                        goto ObjectNext;
                    default:
                        result = type == typeof(double)
                            ? obj.Number
                            : type == typeof(int)
                                ? (int)obj.Number
                                : type == typeof(float)
                                    ? (float)obj.Number
                                    : System.Convert.ChangeType(ToValue(obj), type, CultureInfo.InvariantCulture);
                        break;
                }

                Return:
                if (stack.Count == 0)
                    return result;
                switch (context.Mode)
                {
                    case ConvertMode.Array:
                        // ReSharper disable once RedundantCast
                        context.DstArray[context.JsonArrayEnumerator.Position] = (dynamic)result;
                        break;
                    case ConvertMode.List:
                        context.DstArray.Add((dynamic)result);
                        break;
                    case ConvertMode.Object:
                        context.SetterEnumerator.Current.Invoke(context.DstObject, result);
                        goto ObjectNext;
                }

                ArrayNext:
                if (!context.JsonArrayEnumerator.MoveNext())
                {
                    result = context.DstArray;
                    context = stack.Pop();
                    goto Return;
                }
                type = context.Type;
                obj = context.JsonArrayEnumerator.Current;
                goto Convert;

                ObjectNext:
                if (!context.SetterEnumerator.MoveNext())
                {
                    result = context.DstObject;
                    context = stack.Pop();
                    goto Return;
                }
                var current = context.SetterEnumerator.Current;
                type = current.Type;
                obj = current.Value;
                goto Convert;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private static bool IsGenericList(Type type)
            {
                return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>);
            }

            private class SetterEnumerator
            {
                private readonly ReflectiveOperation.Setter[] _setters;
                private readonly JsonDictionary _dict;
                private int _position = -1;

                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public SetterEnumerator(Type type, JsonDictionary dict)
                {
                    _setters = ReflectiveOperation.GetSetterList(type);
                    _dict = dict;
                }

                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public bool MoveNext()
                {
                    while (true)
                    {
                        _position++;
                        if (_position == _setters.Length)
                            return false;
                        Current = _setters[_position];
                        if (_dict.TryGetValue(Current.Name, out Current.Value))
                            return true;
                    }
                }

                public ReflectiveOperation.Setter Current { get; private set; }
            }
        }

        private static object ConvertToIEnumerable(InternalObject obj)
        {
            return obj.IsArray
                ? (object)obj.Array.GetEnumerator().GetEnumerable().Select(ToValue)
                : obj.Dictionary.GetEnumerator().GetEnumerable().ToDictionary(x => x.Key, x => ToValue(x.Value));
        }
    }
}