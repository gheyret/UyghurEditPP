using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace UyghurEditPP
{
    internal static class ReflectiveOperation
    {
        private class ReflectionCache<T>
        {
            private readonly TypeDictionary<T> _cache = new TypeDictionary<T>();
            private readonly Func<Type, T> _operation;

            public ReflectionCache(Func<Type, T> operation)
            {
                _operation = operation;
            }

            public T Get(Type type)
            {
            	T value;
                return _cache.TryGetValue(type, out value) ? value : _cache.Insert(type, _operation(type));
            }
        }

        public class Getter
        {
            public string Name;
            public Func<object, object> Invoke;
            public object Value;
        }

        public class Setter
        {
            public string Name;
            public Type Type;
            public Action<object, object> Invoke;
            public InternalObject Value;
        }

        private static readonly ReflectionCache<Getter[]> GetterListCache = new ReflectionCache<Getter[]>(CreateGetterList);

        public static Getter[] GetGetterList(Type type)
        {
            return GetterListCache.Get(type);
        }

        private static Getter[] CreateGetterList(Type type)
        {
            return (from prop in type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                select new Getter
                {
                    Name = prop.Name,
                    Invoke = MakeGetter(prop)
                }).ToArray();
        }

        private static Func<object, object> MakeGetter(PropertyInfo prop)
        {
            var paramObj = Expression.Parameter(typeof(object));
            return Expression.Lambda<Func<object, object>>(
                Expression.Convert(
                    // ReSharper disable once AssignNullToNotNullAttribute
                    Expression.Property(Expression.Convert(paramObj, prop.DeclaringType), prop),
                    typeof(object)), paramObj).Compile();
        }

        private static readonly ReflectionCache<Setter[]> SetterListCache = new ReflectionCache<Setter[]>(CreateSetterList);

        public static Setter[] GetSetterList(Type type)
        {
            return SetterListCache.Get(type);
        }

        private static Setter[] CreateSetterList(Type type)
        {
            return (from prop in type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                where prop.CanWrite
                select new Setter
                {
                    Name = prop.Name,
                    Type = prop.PropertyType,
                    Invoke = MakeSetter(prop)
                }).ToArray();
        }

        private static Action<object, object> MakeSetter(PropertyInfo prop)
        {
            var paramThis = Expression.Parameter(typeof(object));
            var paramObj = Expression.Parameter(typeof(object));
            return Expression.Lambda<Action<object, object>>(
                Expression.Assign(
                    // ReSharper disable once AssignNullToNotNullAttribute
                    Expression.Property(Expression.Convert(paramThis, prop.DeclaringType), prop),
                    Expression.Convert(paramObj, prop.PropertyType)),
                paramThis, paramObj).Compile();
        }

        private static readonly ReflectionCache<Func<object>> ObjectCreatorCache = new ReflectionCache<Func<object>>(CreateObjectCreator);

        public static Func<object> GetObjectCreator(Type type)
        {
            return ObjectCreatorCache.Get(type);
        }

        private static Func<object> CreateObjectCreator(Type type)
        {
            return Expression.Lambda<Func<object>>(Expression.New(type)).Compile();
        }
    }
}