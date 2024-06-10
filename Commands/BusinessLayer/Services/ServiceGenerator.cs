using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCodeGenerationVSIX.Commands.BusinessLayer.Services
{
    public class ServiceGenerator
    {
        private string GenerateTemplate(string modelName)
        {
            return $@"using BusinessLayer.Abstract;
using BusinessLayer.ClientMessages;
using Common.Helpers.TextMethots;
using Common.Response;
using DataLayer.Abstract;
using DataLayer.EntityFramework;
using EntityLayer.Dtos.{modelName}Dtos;
using EntityLayer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BusinessLayer.Concrete
{{
    public sealed class {modelName}Manager : I{modelName}Service
    {{
        private readonly I{modelName}Dal _{modelName}Dal;
        private readonly TextsCheckMethots _textsCheckMethos;

        public {modelName}Manager(I{modelName}Dal {modelName}Dal, TextsCheckMethots textsCheckMethos)
        {{
            _{modelName}Dal = {modelName}Dal;
            _textsCheckMethos = textsCheckMethos;
        }}



        public async Task<ResultDto<Create{modelName}Dtos>> CreateAsync(Create{modelName}Dtos entity, CancellationToken cancellationToken = default)
        {{
            throw new NotImplementedException();
        }}

        public async Task<ResultDto<List{modelName}Dtos>> DeleteAsync(List{modelName}Dtos entity, CancellationToken cancellationToken = default)
        {{
            throw new NotImplementedException();
        }}

        public async Task<ResultDto<List<List{modelName}Dtos>>> FindAsync(Expression<Func<{modelName}, bool>> predicate, CancellationToken cancellationToken = default)
        {{
            throw new NotImplementedException();
        }}

        public async Task<ResultDto<List<List{modelName}Dtos>>> GetAllAsync(CancellationToken cancellationToken = default)
        {{
            throw new NotImplementedException();
        }}

        public async Task<ResultDto<List{modelName}Dtos>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {{
            throw new NotImplementedException();
        }}

        public async Task<ResultDto<List<List{modelName}Dtos>>> GetPagedAsync(int pageNumber, int pageSize, Expression<Func<{modelName}, bool>>? filter = null, Expression<Func<{modelName}, object>>? orderBy = null, bool ascending = true, CancellationToken cancellationToken = default)
        {{
            throw new NotImplementedException();
        }}

        public async Task<ResultDto<Update{modelName}Dtos>> UpdateAsync(Update{modelName}Dtos entity, CancellationToken cancellationToken = default)
        {{
            throw new NotImplementedException();
        }}

        private async Task<ResultDto<List{modelName}Dtos>> Check{modelName}NameExistsAsync(string name, CancellationToken cancellationToken = default)
        {{
            throw new NotImplementedException();
        }}
    }}
}}

";
        }

        public void GenerateServiceFiles(string modelName, string outputPath)
        {
            var serviceContent = GenerateTemplate(modelName);
            var servicePath = Path.Combine(outputPath, $"{modelName}Manager.cs");
            File.WriteAllText(servicePath, serviceContent);
        }
    }
}
