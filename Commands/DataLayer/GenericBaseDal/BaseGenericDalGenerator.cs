using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCodeGenerationVSIX.Commands.DataLayer.GenericBaseDal
{
    public class BaseGenericDalGenerator
    {
        private string GenerateTemplate()
        {
            return $@"using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DataLayer.Abstract
{{
    public interface IGenericDal<T> where T : class
    {{
        Task<List<T>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<T> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task CreateAsync(T entity, CancellationToken cancellationToken = default);
        Task UpdateAsync(T entity, CancellationToken cancellationToken = default);
        Task DeleteAsync(T entity, CancellationToken cancellationToken = default);
        Task<List<T>> FindAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
        Task<List<T>> GetPagedAsync(int pageNumber, int pageSize, Expression<Func<T, bool>>? filter = null, Expression<Func<T, object>>? orderBy = null, bool ascending = true, CancellationToken cancellationToken = default);
        Task<T?> GetByFilterAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
        Task<int> CountAsync(Expression<Func<T, bool>>? filter = null, CancellationToken cancellationToken = default);
    }}
}}
";
        }

        public void GenerateGenericDal(string targetDirectory, string targetFileName)
        {
            string targetFilePath = Path.Combine(targetDirectory, targetFileName);

            if (File.Exists(targetFilePath))
            {
                System.Diagnostics.Debug.WriteLine("IGenericDal zaten mevcut.");
                return;
            }

            string content = GenerateTemplate();

            if (!Directory.Exists(targetDirectory))
            {
                Directory.CreateDirectory(targetDirectory);
            }

            File.WriteAllText(targetFilePath, content);
            System.Diagnostics.Debug.WriteLine($"IGenericDal oluşturuldu: {targetFilePath}");
        }
    }
}
