using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCodeGenerationVSIX.Commands.DataLayer.EntityFramework
{
    public class EntityFrameworkGenerator
    {
        private string GenerateTemplate(string modelName)
        {
            return $@"using DataLayer.Abstract;
using DataLayer.Context;
using DataLayer.Repositories;
using EntityLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.EntityFramework
{{
    public class EF{modelName}Dal : GenericRepository<{modelName}>, I{modelName}Dal
    {{
        public EF{modelName}Dal(AppDbContext context) : base(context)
        {{
            // add Ef options
        }}
    }}
}}";
        }

        public void GenerateEntityFrameworkFiles(string modelName, string outputPath)
        {
            var entityFrameworkContent = GenerateTemplate(modelName);
            var entityFrameworkPath = Path.Combine(outputPath, $"EF{modelName}Dal.cs");
            File.WriteAllText(entityFrameworkPath, entityFrameworkContent);
        }
    }
}
