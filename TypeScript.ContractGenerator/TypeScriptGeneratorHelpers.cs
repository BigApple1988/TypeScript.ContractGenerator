using System;
using System.Linq;
using System.Reflection;

using JetBrains.Annotations;

namespace SkbKontur.TypeScript.ContractGenerator
{
    public static class TypeScriptGeneratorHelpers
    {
        public static (bool, Type) ProcessNullable(ICustomAttributeProvider attributeContainer, Type type, NullabilityMode nullabilityMode)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                var underlyingType = type.GetGenericArguments()[0];
                return (true, underlyingType);
            }

            if (attributeContainer == null || !type.IsClass)
                return (false, type);

            return (CanBeNull(attributeContainer, nullabilityMode), type);
        }

        private static bool CanBeNull([NotNull] ICustomAttributeProvider attributeContainer, NullabilityMode nullabilityMode)
        {
            var attributes = attributeContainer.GetCustomAttributes(inherit : true);
            return nullabilityMode == NullabilityMode.Pessimistic
                       ? attributes.All(x => x.GetType().Name != "NotNullAttribute")
                       : attributes.Any(x => x.GetType().Name == "CanBeNullAttribute");
        }
    }
}