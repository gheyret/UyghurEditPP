using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace UyghurEditPP
{
    public partial class JsonObject
    {
        private static class ConvertFrom
        {
            private class Context
            {
                public ConvertMode Mode;
                public IEnumerator ArrayEnumerator;
                public GetterEnumerator GetterEnumerator;
                public JsonArray DstArray;
                public JsonDictionary DstDictionary;
            }

            public static InternalObject Convert(object value)
            {
                var stack = new Stack<Context>();
                var context = new Context();
                var result = new InternalObject();

                Convert:
                if (value == null)
                {
                    result.Type = JsonType.Null;
                    goto Return;
                }
                var type = value.GetType();
                switch (Type.GetTypeCode(type))
                {
                    case TypeCode.Empty:
                    case TypeCode.DBNull:
                        result.Type = JsonType.Null;
                        break;
                    case TypeCode.Boolean:
                        result.Type = (bool)value ? JsonType.True : JsonType.False;
                        break;
                    case TypeCode.Byte:
                    case TypeCode.SByte:
                    case TypeCode.Int16:
                    case TypeCode.UInt16:
                    case TypeCode.UInt32:
                    case TypeCode.Int64:
                    case TypeCode.UInt64:
                    case TypeCode.Decimal:
                        result.Number = ConvertToDouble(value);
                        break;
                    case TypeCode.Int32:
                        result.Number = (int)value;
                        break;
                    case TypeCode.Single:
                        result.Number = (float)value;
                        break;
                    case TypeCode.Double:
                        result.Number = (double)value;
                        break;
                    case TypeCode.Char:
                    case TypeCode.DateTime:
                        result.Type = JsonType.String;
                        result.String = ConvertToString(value);
                        break;
                    case TypeCode.String:
                        result.Type = JsonType.String;
                        result.String = (string)value;
                        break;
                    case TypeCode.Object:
                        if (typeof(IEnumerable).IsAssignableFrom(type)) // Can convert to array
                        {
                            stack.Push(context);
                            context = new Context
                            {
                                Mode = ConvertMode.Array,
                                ArrayEnumerator = ((IEnumerable)value).GetEnumerator(),
                                DstArray = new JsonArray()
                            };
                            goto ArrayNext;
                        }
                        JsonObject obj = value as JsonObject;
                        if (obj!=null)
                        {
                            result = obj._data;
                            break;
                        }
                        stack.Push(context);
                        var v1 = value;
                        context = new Context
                        {
                            Mode = ConvertMode.Object,
                            GetterEnumerator = new GetterEnumerator(v1),
                            DstDictionary = new JsonDictionary()
                        };
                        goto ObjectNext;
                }

                Return:
                if (stack.Count == 0)
                    return result;
                if (context.Mode == ConvertMode.Array)
                {
                    context.DstArray.Add(result);
                    goto ArrayNext;
                }
                context.DstDictionary[context.GetterEnumerator.Current.Name] = result;

                ObjectNext:
                if (context.GetterEnumerator.MoveNext())
                {
                    value = context.GetterEnumerator.Current.Value;
                    goto Convert;
                }
                result.Type = JsonType.Object;
                result.Dictionary = context.DstDictionary;
                context = stack.Pop();
                goto Return;

                ArrayNext:
                if (context.ArrayEnumerator.MoveNext())
                {
                    value = context.ArrayEnumerator.Current;
                    goto Convert;
                }
                result.Type = JsonType.Array;
                result.Array = context.DstArray;
                context = stack.Pop();
                goto Return;
            }

            private static double ConvertToDouble(object value)
            {
                return (double)System.Convert.ChangeType(value, typeof(double), CultureInfo.InvariantCulture);
            }

            private static string ConvertToString(object value)
            {
                return (string)System.Convert.ChangeType(value, typeof(string), CultureInfo.InvariantCulture);
            }

            private class GetterEnumerator
            {
                private readonly ReflectiveOperation.Getter[] _getters;
                private readonly object _target;
                private int _position = -1;

                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public GetterEnumerator(object target)
                {
                    _target = target;
                    _getters = ReflectiveOperation.GetGetterList(_target.GetType());
                }

                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public bool MoveNext()
                {
                    while (true)
                    {
                        _position++;
                        if (_position == _getters.Length)
                            return false;
                        Current = _getters[_position];
                        Current.Value = Current.Invoke(_target);
                        return true;
                    }
                }

                public ReflectiveOperation.Getter Current { get; private set; }
            }
        }
    }
}