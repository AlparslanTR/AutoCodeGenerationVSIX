using System;
using System.IO;

namespace AutoCodeGenerationVSIX.Commands.DataLayer.Context
{
    public class AppDbContextGenerator
    {
        private string GenerateTemplate(string modelName)
        {
            return @$"using System;
using EntityLayer.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Context
{{
    public sealed class AppDbContext : DbContext
    {{
        // Her auto code generation yapıldığında alttaki veritabanı bağlantı adresi değiştirilmelidir.
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {{
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer(""Add Here MsSql Connection Address"");
        }}

        public DbSet<{modelName}> {modelName}s {{ get; set; }}
    }}
}}";
        }

        private string GenerateDbSet(string modelName)
        {
            return $"public DbSet<{modelName}> {modelName}s {{ get; set; }}\n";
        }

        public void CreateOrUpdateAppDbContext(string targetDirectory, string targetFileName, string modelName)
        {
            string targetFilePath = Path.Combine(targetDirectory, targetFileName);
            string newDbSet = GenerateDbSet(modelName);

            if (File.Exists(targetFilePath))
            {
                // AppDbContext zaten mevcut, güncelleme yap
                string existingContent = File.ReadAllText(targetFilePath);

                if (!existingContent.Contains(newDbSet))
                {
                    int insertIndex = existingContent.LastIndexOf("}", existingContent.LastIndexOf("}") - 1);
                    if (insertIndex != -1)
                    {
                        existingContent = existingContent.Insert(insertIndex, newDbSet);
                        File.WriteAllText(targetFilePath, existingContent);
                        System.Diagnostics.Debug.WriteLine($"AppDbContext güncellendi: {targetFilePath}");
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("Kapanış süslü parantezi bulunamadı.");
                    }
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("DbSet zaten mevcut.");
                }
            }
            else
            {
                // AppDbContext mevcut değil, yeni oluştur
                string content = GenerateTemplate(modelName);

                if (!Directory.Exists(targetDirectory))
                {
                    Directory.CreateDirectory(targetDirectory);
                }

                File.WriteAllText(targetFilePath, content);
                System.Diagnostics.Debug.WriteLine($"AppDbContext oluşturuldu: {targetFilePath}");
            }
        }
    }
}
