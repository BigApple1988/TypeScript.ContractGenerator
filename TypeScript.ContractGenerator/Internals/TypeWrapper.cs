using System;
using System.Linq;

using SkbKontur.TypeScript.ContractGenerator.Abstractions;

namespace SkbKontur.TypeScript.ContractGenerator.Internals
{
    public class TypeWrapper : ITypeInfo
    {
        public TypeWrapper(Type type)
        {
            Type = type;
        }

        public Type Type { get; }

        public string Name => Type.Name;
        public string FullName => Type.FullName;
        public string Namespace => Type.Namespace;
        public bool IsEnum => Type.IsEnum;
        public bool IsValueType => Type.IsValueType;
        public bool IsArray => Type.IsArray;
        public bool IsAbstract => Type.IsAbstract;
        public bool IsGenericType => Type.IsGenericType;
        public bool IsGenericParameter => Type.IsGenericParameter;
        public bool IsGenericTypeDefinition => Type.IsGenericTypeDefinition;
        public ITypeInfo BaseType => new TypeWrapper(Type.BaseType);

        public IMethodInfo[] GetMethods()
        {
            return Type.GetMethods().Select(x => (IMethodInfo)new MethodWrapper(x)).ToArray();
        }

        public IPropertyInfo[] GetProperties()
        {
            return Type.GetProperties().Select(x => (IPropertyInfo)new PropertyWrapper(x)).ToArray();
        }

        public ITypeInfo[] GetGenericArguments()
        {
            return Type.GetGenericArguments().Select(x => (ITypeInfo)new TypeWrapper(x)).ToArray();
        }

        public ITypeInfo GetGenericTypeDefinition()
        {
            return new TypeWrapper(Type.GetGenericTypeDefinition());
        }

        public ITypeInfo GetElementType()
        {
            return new TypeWrapper(Type.GetElementType());
        }

        public object[] GetCustomAttributes(bool inherit)
        {
            return Type.GetCustomAttributes(inherit);
        }

        public bool Equals(ITypeInfo other)
        {
            return Type == other?.Type;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;

            if (ReferenceEquals(this, obj))
                return true;

            return obj is ITypeInfo typeInfo && Equals(typeInfo);
        }

        public override int GetHashCode()
        {
            return Type.GetHashCode();
        }

        public override string ToString()
        {
            return Type.ToString();
        }
    }
}