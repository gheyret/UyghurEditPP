using System.Dynamic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Microsoft.CSharp.RuntimeBinder;
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

namespace UyghurEditPP
{
    public partial class JsonObject : DynamicObject
    {
        private readonly InternalObject _data;

        public bool IsArray{
        	get{return _data.IsArray;}
        }

        public bool IsObject{
        	get{return !IsArray;}
        }

        public int Length{
        	get{return CountImpl();}
        }

        public int Count{
        	get{return CountImpl();}
        }

        private int CountImpl([CallerMemberName] string caller = null)
        {
        	if( IsArray){
        		return _data.Array.Count;
        	}
        	else{
            	throw new RuntimeBinderException(string.Format("'DynaJson.JsonObject' does not contain a definition for '{0}'",caller));
        	}
        }

        public bool IsDefined(string key)
        {
            return IsObject && _data.Dictionary.ContainsKey(key);
        }

        public bool IsDefined(int index)
        {
            return IsArray && index < _data.Array.Count;
        }

        public bool Delete(string key)
        {
            if (!IsObject)
                return false;
            return _data.Dictionary.Remove(key);
        }

        public bool Delete(int index)
        {
            if (!IsArray || index >= _data.Array.Count)
                return false;
            _data.Array.RemoveAt(index);
            return true;
        }

        public T Deserialize<T>()
        {
            return (T)(dynamic)this;
        }

        public void Serialize(TextWriter writer)
        {
            Serialize(writer, DynaJson.MaxDepth);
        }

        public void Serialize(TextWriter writer, int maxDepth)
        {
            Serializer.Serialize(_data, writer, maxDepth);
        }

        public override string ToString()
        {
            return ToString(DynaJson.MaxDepth);
        }

        public string ToString(int maxDepth)
        {
            var writer = new StringWriter();
            Serialize(writer, maxDepth);
            return writer.ToString();
        }

        internal JsonObject()
        {
            _data.Type = JsonType.Object;
            _data.Dictionary = new JsonDictionary();
        }

        internal JsonObject(InternalObject obj)
        {
            _data = obj;
        }

        internal JsonObject(object obj)
        {
            _data = ConvertFrom.Convert(obj);
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = null;
            if (IsArray)
                return false;
            InternalObject value;
            if (!_data.Dictionary.TryGetValue(binder.Name, out value))
                return false;
            result = ToValue(value);
            return true;
        }

        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            if (args.Length > 0)
            {
                result = null;
                return false; // fallback to TryInvoke
            }
            result = IsObject && _data.Dictionary.ContainsKey(binder.Name);
            return true;
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            if (!IsObject)
                return false;
            _data.Dictionary[binder.Name] = ConvertFrom.Convert(value);
            return true;
        }

        public override bool TryInvoke(InvokeBinder binder, object[] args, out object result)
        {
            result = null;
            if (IsArray)
            {
                result = Delete((int)args[0]);
            }
            else if (IsObject)
            {
                result = Delete((string)args[0]);
            }
            return result != null;
        }

        public override bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object result)
        {
            result = ToValue(IsArray ? _data.Array[(int)indexes[0]] : _data.Dictionary[(string)indexes[0]]);
            return true;
        }

        public override bool TrySetIndex(SetIndexBinder binder, object[] indexes, object value)
        {
            var internalObject = ConvertFrom.Convert(value);
            if (IsObject)
            {
                _data.Dictionary[(string)indexes[0]] = internalObject;
                return true;
            }
            var index = (int)indexes[0];
            if (index < _data.Array.Count)
            {
                _data.Array[index] = internalObject;
            }
            else
            {
                _data.Array.Add(internalObject);
            }
            return true;
        }

        public override bool TryConvert(ConvertBinder binder, out object result)
        {
            result = ConvertTo.Convert(_data, binder.Type);
            return true;
        }

        internal static object ToValue(InternalObject obj)
        {
            switch (obj.Type)
            {
                case JsonType.Null:
                    return null;
                case JsonType.True:
                    return true;
                case JsonType.False:
                    return false;
                case JsonType.String:
                    return obj.String;
                case JsonType.Array:
                case JsonType.Object:
                    return new JsonObject(obj);
                default:
                    return obj.Number;
            }
        }
    }

    internal enum JsonType : uint
    {
        Null = 0xfff00001, // Use NaN boxing
        True = 0xfff00002,
        False = 0xfff00003,
        String = 0xfff00004,
        Array = 0xfff00005,
        Object = 0xfff00006
    }

    [StructLayout(LayoutKind.Explicit)]
    internal struct InternalObject
    {
        [FieldOffset(4)] public JsonType Type;
        [FieldOffset(0)] public double Number;
        [FieldOffset(8)] public string String;
        [FieldOffset(8)] public JsonArray Array;
        [FieldOffset(8)] public JsonDictionary Dictionary;

        public bool IsArray{
        	get {
        		return Type == JsonType.Array;
        	}
        }
    }
}