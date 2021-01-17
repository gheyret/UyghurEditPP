using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace UyghurEditPP
{
    internal class JsonParser
    {
        public const int StringInitialCapacity = 128;
        public const int ReaderBufferSize = 2048;

        private static readonly JsonParser Instance = new JsonParser();
        private TextReader _reader;
        private readonly char[] _buffer = new char[ReaderBufferSize];
        private char[] _charBuffer = new char[StringInitialCapacity];

        private int _available;
        private int _bufferIndex;
        private char _nextChar;
        private int _position;
        private bool _isEnd;
        private static readonly bool[] WhiteSpace = new bool[' ' + 1];

        [StructLayout(LayoutKind.Explicit)]
        private struct Context
        {
            [FieldOffset(0)] public JsonArray Array;
            [FieldOffset(0)] public JsonDictionary Dictionary;
            [FieldOffset(8)] public string Key;
        }

        static JsonParser()
        {
            WhiteSpace['\r'] = WhiteSpace['\n'] = WhiteSpace['\t'] = WhiteSpace[' '] = true;
        }

        private void Setup(TextReader reader)
        {
            _bufferIndex = _position = 0;
            _reader = reader;
            _buffer[0] = '\0';
            _available = _reader.ReadBlock(_buffer, 0, _buffer.Length);
            _isEnd = _available == 0;
            _nextChar = _buffer[0];
        }

        public static object Parse(TextReader reader, int maxDepth)
        {
            return Instance.ParseInternal(reader, maxDepth);
        }

        private object ParseInternal(TextReader reader, int maxDepth)
        {
            Setup(reader);
            var stack = new Context[maxDepth];
            var depth = 0;
            var context = new Context();

            SkipWhiteSpaces();
            while (true)
            {
                var value = new InternalObject();
                switch (_nextChar)
                {
                    case '0':
                    case '1':
                    case '2':
                    case '3':
                    case '4':
                    case '5':
                    case '6':
                    case '7':
                    case '8':
                    case '9':
                    case '-':
                        value.Number = GetNumber();
                        break;
                    case 'n':
                        CheckToken("ull");
                        value.Type = JsonType.Null;
                        break;
                    case 't':
                        CheckToken("rue");
                        value.Type = JsonType.True;
                        break;
                    case 'f':
                        // ReSharper disable once StringLiteralTypo
                        CheckToken("alse");
                        value.Type = JsonType.False;
                        break;
                    case '"':
                        value.Type = JsonType.String;
                        value.String = GetString();
                        break;
                    case '[':
                        if (depth == maxDepth)
                            throw JsonParserException.TooDeepNesting(depth, _position);
                        Consume();
                        stack[depth++] = context;
                        context = new Context
                        {
                            Array = new JsonArray()
                        };
                        SkipWhiteSpaces();
                        continue;
                    case ']':
                        if (context.Array == null)
                            throw JsonParserException.UnexpectedError(_nextChar, _position);
                        Consume();
                        value.Type = JsonType.Array;
                        value.Array = context.Array;
                        context = stack[--depth];
                        break;
                    case '{':
                        if (depth == maxDepth)
                            throw JsonParserException.TooDeepNesting(depth, _position);
                        Consume();
                        stack[depth++] = context;
                        context = new Context
                        {
                            Dictionary = new JsonDictionary()
                        };
                        goto GetKey;
                    case '}':
                        if (context.Dictionary == null)
                            throw JsonParserException.UnexpectedError(_nextChar, _position);
                        Consume();
                        value.Type = JsonType.Object;
                        value.Dictionary = context.Dictionary;
                        context = stack[--depth];
                        break;
                    default:
                        if (_isEnd)
                            throw JsonParserException.UnexpectedEnd(_position);
                        throw JsonParserException.UnexpectedError(_nextChar, _position);
                }

                SkipWhiteSpaces();
                // Start
                if (depth == 0)
                {
                    if (_isEnd)
                        return JsonObject.ToValue(value);
                    throw JsonParserException.UnexpectedError(_nextChar, _position);
                }
                // Array
                if (context.Key == null)
                {
                    context.Array.Add(value);
                    if (_nextChar == ']')
                        continue;
                    if (_nextChar != ',')
                        throw JsonParserException.ExpectingError("',' or ']'", _position);
                    Consume();
                    SkipWhiteSpaces();
                    continue;
                }
                // Object
                context.Dictionary.Add(context.Key, value);
                if (_nextChar == '}')
                    continue;
                if (_nextChar != ',')
                    throw JsonParserException.ExpectingError("',' or '}'", _position);
                Consume();

                GetKey:
                SkipWhiteSpaces();
                if (_nextChar == '}')
                    continue;
                if (_nextChar != '"')
                    throw JsonParserException.ExpectingError("string", _position);
                context.Key = GetString();
                SkipWhiteSpaces();
                if (_nextChar != ':')
                    throw JsonParserException.ExpectingError("':'", _position);
                Consume();
                SkipWhiteSpaces();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void SkipWhiteSpaces()
        {
            while (true)
            {
                var ch = _nextChar;
                if (ch > ' ' || !WhiteSpace[ch])
                    return;
                Consume();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void Consume()
        {
            _bufferIndex++;
            _position++;
            if (_available == _bufferIndex)
            {
                _bufferIndex = 0;
                _available = _reader.ReadBlock(_buffer, 0, _buffer.Length);
                if (_available == 0)
                {
                    _isEnd = true;
                    _nextChar = '\0';
                    return;
                }
            }
            _nextChar = _buffer[_bufferIndex];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void CheckToken(string s)
        {
            Consume();
            foreach (var ch in s)
            {
                if (ch != _nextChar)
                	throw JsonParserException.ExpectingError(string.Format("'{0}'",ch), _position);
                Consume();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private double GetNumber()
        {
            var result = 0d;
            var sign = 1;
            if (_nextChar == '-')
            {
                sign = -1;
                Consume();
                if (!IsNumber())
                    throw JsonParserException.ExpectingError("digit", _position);
            }
            do
            {
                result = result * 10.0 + (_nextChar - '0');
                Consume();
            } while (IsNumber());
            if (_nextChar == '.')
            {
                Consume();
                if (!IsNumber())
                    throw JsonParserException.ExpectingError("digit", _position);
                var exp = 0.1;
                do
                {
                    result += (_nextChar - '0') * exp;
                    exp *= 0.1;
                    Consume();
                } while (IsNumber());
            }
            if (_nextChar == 'e' || _nextChar == 'E')
            {
                Consume();
                var expSign = 1;
                var exp = 0;
                if (_nextChar == '-' || _nextChar == '+')
                {
                    if (_nextChar == '-')
                        expSign = -1;
                    Consume();
                }
                if (!IsNumber())
                    throw JsonParserException.ExpectingError("digit", _position);
                do
                {
                    exp = exp * 10 + (_nextChar - '0');
                    Consume();
                } while (IsNumber());
                result = result * Math.Pow(10, expSign * exp);
            }
            return sign * result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool IsNumber()
        {
            return '0' <= _nextChar && _nextChar <= '9';
        }

        private string GetString()
        {
            Consume();
            var start = _position;
            var len = 0;
            while (true)
            {
                if (_isEnd)
                    throw JsonParserException.UnexpectedEnd(start + len);
                var ch = _nextChar;
                if (ch == '"')
                {
                    Consume();
                    return new string(_charBuffer, 0, len);
                }
                if (ch == '\\')
                {
                    ch = UnEscape();
                }
                else if (ch < ' ')
                {
                    throw JsonParserException.UnexpectedError(ch, _position);
                }
                if (len >= _charBuffer.Length)
                    Array.Resize(ref _charBuffer, _charBuffer.Length * 2);
                _charBuffer[len++] = ch;
                Consume();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private char UnEscape()
        {
            Consume();
            var ch = _nextChar;
            switch (ch)
            {
                case '\\':
                case '/':
                case '"':
                    break;
                case 'b':
                    ch = '\b';
                    break;
                case 'f':
                    ch = '\f';
                    break;
                case 'n':
                    ch = '\n';
                    break;
                case 'r':
                    ch = '\r';
                    break;
                case 't':
                    ch = '\t';
                    break;
                case 'u':
                    ch = UnEscapeUnicode();
                    break;
                default:
                    throw JsonParserException.InvalidError(string.Format("escape character '{0}'",ch), _position);
            }
            return ch;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private char UnEscapeUnicode()
        {
            var code = 0;
            for (var i = 0; i < 4; i++)
            {
                Consume();
                var ch = _nextChar;
                code <<= 4;
                if ('0' <= ch && ch <= '9')
                {
                    code += ch - '0';
                }
                else if ('a' <= ch && ch <= 'f')
                {
                    code += 10 + ch - 'a';
                }
                else if ('A' <= ch && ch <= 'F')
                {
                    code += 10 + ch - 'A';
                }
                else
                {
                	throw JsonParserException.InvalidError(string.Format("unicode escape '{0}'",ch), _position);
                }
            }
            return (char)code;
        }
    }

    public class JsonParserException : Exception
    {
        private JsonParserException(string message, string item, int position) :
    		base(string.Format("{0} {1} at {2}", message,item,position))
        {
        }

        public static JsonParserException ExpectingError(string expecting, int position)
        {
            return new JsonParserException("Expecting", expecting, position);
        }

        public static JsonParserException UnexpectedError(char ch, int position)
        {
        	return new JsonParserException("Unexpected" , string.Format("character '{0}'",ch), position);
        }

        public static JsonParserException InvalidError(string item, int position)
        {
            return new JsonParserException("Invalid", item, position);
        }

        public static JsonParserException UnexpectedEnd(int position)
        {
            return new JsonParserException("Unexpected", "end", position);
        }

        public static JsonParserException TooDeepNesting(int depth, int position)
        {
            return new JsonParserException("Too deep nesting", (depth + 1).ToString(), position);
        }
    }
}