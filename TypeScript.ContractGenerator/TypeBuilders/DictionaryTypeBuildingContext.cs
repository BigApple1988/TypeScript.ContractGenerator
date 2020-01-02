using System.Collections.Generic;

using JetBrains.Annotations;

using SkbKontur.TypeScript.ContractGenerator.Abstractions;
using SkbKontur.TypeScript.ContractGenerator.CodeDom;

namespace SkbKontur.TypeScript.ContractGenerator.TypeBuilders
{
    public class DictionaryTypeBuildingContext : ITypeBuildingContext
    {
        public DictionaryTypeBuildingContext([NotNull] ITypeInfo dictionaryType, [NotNull] TypeScriptGenerationOptions options)
        {
            keyType = dictionaryType.GetGenericArguments()[0];
            valueType = dictionaryType.GetGenericArguments()[1];
            this.options = options;
        }

        public static bool Accept(ITypeInfo type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition().Type == typeof(Dictionary<,>);
        }

        public bool IsDefinitionBuilt => true;

        public void Initialize(ITypeGenerator typeGenerator)
        {
        }

        public void BuildDefinition(ITypeGenerator typeGenerator)
        {
        }

        public TypeScriptType ReferenceFrom(TypeScriptUnit targetUnit, ITypeGenerator typeGenerator, IAttributeProvider attributeProvider)
        {
            return new TypeScriptTypeDefintion
                {
                    Members =
                        {
                            new TypeScriptTypePropertyGetterDeclaration
                                {
                                    Argument = new TypeScriptArgumentDeclaration
                                        {
                                            Name = "key",
                                            Type = GetKeyType(targetUnit, typeGenerator, attributeProvider),
                                        },
                                    ResultType = GetValueType(targetUnit, typeGenerator, attributeProvider),
                                    Optional = true,
                                }
                        }
                };
        }

        private TypeScriptType GetKeyType(TypeScriptUnit targetUnit, ITypeGenerator typeGenerator, IAttributeProvider attributeProvider)
        {
            var key = typeGenerator.ResolveType(keyType).ReferenceFrom(targetUnit, typeGenerator, null);
            return MaybeNull(keyType, key, attributeProvider, 1);
        }

        private TypeScriptType GetValueType(TypeScriptUnit targetUnit, ITypeGenerator typeGenerator, IAttributeProvider attributeProvider)
        {
            var value = typeGenerator.ResolveType(valueType).ReferenceFrom(targetUnit, typeGenerator, null);
            return MaybeNull(valueType, value, attributeProvider, 1 + TypeScriptGeneratorHelpers.GetGenericArgumentsToSkip(keyType));
        }

        private TypeScriptType MaybeNull(ITypeInfo trueType, TypeScriptType type, IAttributeProvider attributeProvider, int index)
        {
            if (options.NullabilityMode != NullabilityMode.NullableReference)
                return type;

            var isNullable = TypeScriptGeneratorHelpers.NullableReferenceCanBeNull(attributeProvider, trueType, index);
            return TypeScriptGeneratorHelpers.BuildTargetNullableTypeByOptions(type, isNullable, options);
        }

        private readonly ITypeInfo keyType;
        private readonly ITypeInfo valueType;
        private readonly TypeScriptGenerationOptions options;
    }
}