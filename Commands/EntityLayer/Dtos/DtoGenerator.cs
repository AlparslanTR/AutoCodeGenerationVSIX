using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCodeGenerationVSIX.Commands.EntityLayer.Dtos
{
    public class DtoGenerator
    {
        private string GenerateTemplate(string dtoType, string modelName)
        {
        return $@"using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Dtos.{modelName}Dtos
{{
    public sealed record {dtoType}{modelName}Dtos
    (
        // Add properties here
    );
}}";
        }

        public void GenerateDtoFiles(string modelName, string outputPath)
        {
            var createDtoContent = GenerateTemplate("Create", modelName);
            var updateDtoContent = GenerateTemplate("Update", modelName);
            var listDtoContent = GenerateTemplate("List", modelName);

            var createDtoPath = Path.Combine(outputPath, $"Create{modelName}Dtos.cs");
            var updateDtoPath = Path.Combine(outputPath, $"Update{modelName}Dtos.cs");
            var listDtoPath = Path.Combine(outputPath, $"List{modelName}Dtos.cs");

            File.WriteAllText(createDtoPath, createDtoContent);
            File.WriteAllText(updateDtoPath, updateDtoContent);
            File.WriteAllText(listDtoPath, listDtoContent);
        }
    }
}
