using System.Linq;
using System.Reflection;

using SkbKontur.TypeScript.ContractGenerator.Abstractions;

namespace SkbKontur.TypeScript.ContractGenerator.Internals
{
    public class MethodWrapper : IMethodInfo
    {
        public MethodWrapper(MethodInfo method)
        {
            Method = method;
        }

        public MethodInfo Method { get; }

        public string Name => Method.Name;
        public ITypeInfo ReturnType => new TypeWrapper(Method.ReturnType);

        public IParameterInfo[] GetParameters()
        {
            return Method.GetParameters().Select(x => (IParameterInfo)new ParameterWrapper(x)).ToArray();
        }

        public object[] GetCustomAttributes(bool inherit)
        {
            return Method.GetCustomAttributes(inherit);
        }
    }
}