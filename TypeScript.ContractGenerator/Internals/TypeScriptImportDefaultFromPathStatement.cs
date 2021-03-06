using SkbKontur.TypeScript.ContractGenerator.CodeDom;

namespace SkbKontur.TypeScript.ContractGenerator.Internals
{
    internal class TypeScriptImportDefaultFromPathStatement : TypeScriptImportStatement
    {
        public override string GenerateCode(ICodeGenerationContext context)
        {
            return $"import {TypeName} from '{context.GetReferenceFromUnitToAnother(CurrentUnit.Path, PathToUnit)}';";
        }

        public string TypeName { get; set; }
        public TypeScriptUnit CurrentUnit { get; set; }
        public string PathToUnit { get; set; }
    }
}