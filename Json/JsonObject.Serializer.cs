using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace UyghurEditPP
{
    public partial class JsonObject
    {
        internal unsafe class Serializer
        {
            private static readonly Serializer Instance = new Serializer();
            private readonly char[] _buffer = new char[4096];
            private char* _bufferStart;
            private char* _bufferEnd;
            private char* _pointer;
            private TextWriter _writer;

            [Flags]
            private enum Mode
            {
                Array = 1,
                Delimiter = 2
            }

            [StructLayout(LayoutKind.Explicit)]
            private struct Context
            {
                [FieldOffset(8)] public Mode Mode;
                [FieldOffset(0)] public JsonArray.Enumerator ArrayEnumerator;
                [FieldOffset(0)] public JsonDictionary.Enumerator DictionaryEnumerator;
            }

            public static void Serialize(object obj, TextWriter writer, int maxDepth)
            {
                Instance.SerializeInternal(ConvertFrom.Convert(obj), writer, maxDepth);
            }

            public static void Serialize(InternalObject obj, TextWriter writer, int maxDepth)
            {
                Instance.SerializeInternal(obj, writer, maxDepth);
            }

            private void SerializeInternal(InternalObject obj, TextWriter writer, int maxDepth)
            {
                var stack = new Context[maxDepth];
                var context = new Context();
                var depth = 0;
                _writer = writer;
                fixed (char* bufferStart = _buffer)
                {
                    _bufferStart = bufferStart;
                    _bufferEnd = bufferStart + _buffer.Length;
                    _pointer = bufferStart;
                    Convert:
                    switch (obj.Type)
                    {
                        case JsonType.Null:
                            EnsureBuffer(4);
                            *_pointer++ = 'n';
                            *_pointer++ = 'u';
                            *_pointer++ = 'l';
                            *_pointer++ = 'l';
                            break;
                        case JsonType.True:
                            EnsureBuffer(4);
                            *_pointer++ = 't';
                            *_pointer++ = 'r';
                            *_pointer++ = 'u';
                            *_pointer++ = 'e';
                            break;
                        case JsonType.False:
                            EnsureBuffer(5);
                            *_pointer++ = 'f';
                            *_pointer++ = 'a';
                            *_pointer++ = 'l';
                            *_pointer++ = 's';
                            *_pointer++ = 'e';
                            break;
                        case JsonType.String:
                            WriteString(obj.String);
                            break;
                        case JsonType.Array:
                            if (obj.Array.Count == 0)
                            {
                                EnsureBuffer(2);
                                *_pointer++ = '[';
                                *_pointer++ = ']';
                                break;
                            }
                            EnsureBuffer(1);
                            *_pointer++ = '[';
                            stack[depth++] = context;
                            context = new Context
                            {
                                Mode = Mode.Array,
                                ArrayEnumerator = obj.Array.GetEnumerator()
                            };
                            break;
                        case JsonType.Object:
                            if (obj.Dictionary.Count == 0)
                            {
                                EnsureBuffer(2);
                                *_pointer++ = '{';
                                *_pointer++ = '}';
                                break;
                            }
                            EnsureBuffer(1);
                            *_pointer++ = '{';
                            if (depth == maxDepth)
                                throw new JsonSerializerException("Too deep nesting");
                            stack[depth++] = context;
                            context = new Context
                            {
                                DictionaryEnumerator = obj.Dictionary.GetEnumerator()
                            };
                            break;
                        default:
                            EnsureBuffer(24);
                            _pointer += FastDtoa.Convert(obj.Number, _pointer);
                            break;
                    }

                    Return:
                    if (depth == 0)
                    {
                        var len = (int)(_pointer - _bufferStart);
                        if (len > 0)
                            _writer.Write(_buffer, 0, len);
                        return;
                    }

                    if ((context.Mode & Mode.Array) != 0)
                    {
                        if (context.ArrayEnumerator.MoveNext())
                        {
                            if ((context.Mode & Mode.Delimiter) != 0)
                            {
                                EnsureBuffer(1);
                                *_pointer++ = ',';
                            }
                            context.Mode |= Mode.Delimiter;
                            obj = context.ArrayEnumerator.Current;
                            goto Convert;
                        }
                        EnsureBuffer(1);
                        *_pointer++ = ']';
                    }
                    else
                    {
                        if (context.DictionaryEnumerator.MoveNext())
                        {
                            var current = context.DictionaryEnumerator.Current;

                            if ((context.Mode & Mode.Delimiter) != 0)
                            {
                                EnsureBuffer(1);
                                *_pointer++ = ',';
                            }
                            WriteString(current.Key);
                            EnsureBuffer(1);
                            *_pointer++ = ':';
                            context.Mode |= Mode.Delimiter;
                            obj = current.Value;
                            goto Convert;
                        }
                        EnsureBuffer(1);
                        *_pointer++ = '}';
                    }
                    context = stack[--depth];
                    goto Return;
                }
            }

            private readonly char[] _hex =
                {'0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f'};

            private void WriteString(string s)
            {
                EnsureBuffer(1);
                *_pointer++ = '\"';
                foreach (var ch in s)
                {
                    if (ch >= 0x20 && ch != '"' && ch != '/' && ch != '\\')
                    {
                        EnsureBuffer(1);
                        *_pointer++ = ch;
                        continue;
                    }
                    char escape;
                    switch (ch)
                    {
                        case '"':
                            escape = '"';
                            break;
                        case '/':
                            escape = '/';
                            break;
                        case '\\':
                            escape = '\\';
                            break;
                        case '\b':
                            escape = 'b';
                            break;
                        case '\t':
                            escape = 't';
                            break;
                        case '\n':
                            escape = 'n';
                            break;
                        case '\f':
                            escape = 'f';
                            break;
                        case '\r':
                            escape = 'r';
                            break;
                        default:
                            EnsureBuffer(6);
                            *_pointer++ = '\\';
                            *_pointer++ = 'u';
                            *_pointer++ = _hex[ch >> 12];
                            *_pointer++ = _hex[(ch >> 8) & 0xf];
                            *_pointer++ = _hex[(ch >> 4) & 0xf];
                            *_pointer++ = _hex[ch & 0xf];
                            continue;
                    }
                    EnsureBuffer(2);
                    *_pointer++ = '\\';
                    *_pointer++ = escape;
                }
                EnsureBuffer(1);
                *_pointer++ = '"';
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private void EnsureBuffer(int size)
            {
                if (_pointer + size <= _bufferEnd)
                    return;
                _writer.Write(_buffer, 0, (int)(_pointer - _bufferStart));
                _pointer = _bufferStart;
            }


            // ReSharper disable once MemberCanBePrivate.Global
            public class JsonSerializerException : Exception
            {
                public JsonSerializerException(string message) : base(message)
                {
                }
            }
        }
    }
}