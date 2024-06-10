using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCodeGenerationVSIX.Commands.DataLayer.DalAbstract
{
    public class AbstractGenerator
    {
            private string GenerateTemplate(string modelName)
            {
                return $@"using EntityLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Abstract
{{
    public interface I{modelName}Dal : IGenericDal<{modelName}>
    {{
        // Add custom operations here
    }}
}}";
            }

            public void GenerateAbstractFiles(string modelName, string outputPath)
            {
                var abstractContent = GenerateTemplate(modelName);
                var abstractPath = Path.Combine(outputPath, $"I{modelName}Dal.cs");
                File.WriteAllText(abstractPath, abstractContent);
            }
    }
}
