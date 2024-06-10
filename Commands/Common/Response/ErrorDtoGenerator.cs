using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCodeGenerationVSIX.Commands.Common.Response
{
    public class ErrorDtoGenerator
    {
        private string GenerateTemplate()
        {
            return $@"using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Response
{{
    public class ErrorDto
    {{
        public List<string> Errors {{ get; private set; }} = new List<string>();
        public bool IsShow {{ get; private set; }}

        public ErrorDto(string error, bool isShow)
        {{
            Errors.Add(error);
            IsShow = isShow;
        }}

        public ErrorDto(List<string> errors, bool isShow)
        {{
            Errors = errors;
            IsShow = isShow;
        }}
    }}
}}
";
        }

        public void CreateOrUpdateErrorDto(string targetDirectory, string targetFileName)
        {
            string targetFilePath = Path.Combine(targetDirectory, targetFileName);

            if (File.Exists(targetFilePath))
            {
                // ErrorDto sınıfı zaten mevcut, güncelleme yap
                string existingContent = File.ReadAllText(targetFilePath);
                string newMethod = GenerateTemplate();

                if (!existingContent.Contains("ErrorDto"))
                {
                    int insertIndex = existingContent.LastIndexOf("}");
                    if (insertIndex != -1)
                    {
                        existingContent = existingContent.Insert(insertIndex, newMethod);
                        File.WriteAllText(targetFilePath, existingContent);
                        System.Diagnostics.Debug.WriteLine($"ErrorDto güncellendi: {targetFilePath}");
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine($"ErrorDto güncellenemedi: {targetFilePath}");
                    }
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"ErrorDto zaten mevcut: {targetFilePath}");
                }
            }
            else
            {
                // ErrorDto sınıfı yok, oluştur
                string newMethod = GenerateTemplate();
                File.WriteAllText(targetFilePath, newMethod);
                System.Diagnostics.Debug.WriteLine($"ErrorDto oluşturuldu: {targetFilePath}");
            }
        }
    }
}
