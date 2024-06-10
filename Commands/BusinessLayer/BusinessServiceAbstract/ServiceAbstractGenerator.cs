using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCodeGenerationVSIX.Commands.BusinessLayer.BusinessServiceAbstract
{
    public class ServiceAbstractGenerator
    {
        private string GenerateTemplate(string modelName)
        {
            return $@"using EntityLayer.Dtos.{modelName}Dtos;
using EntityLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BusinessLayer.Abstract
{{
    public interface I{modelName}Service : IGenericService<{modelName}, Create{modelName}Dtos, Update{modelName}Dtos,List{modelName}Dtos>
    {{
        // You can add Custom Operations
    }}
}}
";
        }

        public void GenerateServiceAbstractFiles(string modelName, string outputPath)
        {
            var serviceAbstractContent = GenerateTemplate(modelName);
            var serviceAbstractPath = Path.Combine(outputPath, $"I{modelName}Service.cs");
            File.WriteAllText(serviceAbstractPath, serviceAbstractContent);
        }
    }
}
