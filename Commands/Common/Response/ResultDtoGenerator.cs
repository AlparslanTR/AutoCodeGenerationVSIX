using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCodeGenerationVSIX.Commands.Common.Response
{
    public class ResultDtoGenerator
    {
        private string GenerateTemplate()
        {
            return $@"using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Common.Response
{{
    public class ResultDto<T> where T : class
    {{
        public T? Data {{ get; set; }} 
        public int StatusCode {{ get; private set; }}
        public string Message {{ get; set; }} = string.Empty!;

        [JsonIgnore]
        public bool isSuccess {{ get; private set; }}
        public ErrorDto? Error {{ get; private set; }}


        public static ResultDto<T> Success(T data, string message, int statusCode)
        {{
            return new ResultDto<T> {{ Data = data, Message = message, StatusCode = statusCode, isSuccess = true }};
        }}

        public static ResultDto<T> Success(int statusCode)
        {{
            return new ResultDto<T> {{ Data = default, StatusCode = statusCode, isSuccess = true }};
        }}

        public static ResultDto<T> Fail(ErrorDto errorDto, int statusCode)
        {{
            return new ResultDto<T> {{ Error = errorDto, StatusCode = statusCode, isSuccess = false }};
        }}

        public static ResultDto<T> Fail(string errorMessage, int statusCode, bool isShow)
        {{
            var errorDto = new ErrorDto(errorMessage, isShow);
            return new ResultDto<T> {{ Error = errorDto, StatusCode = statusCode, isSuccess = false }};
        }}
    }}
}}
";
        }

        public void CreateOrUpdateResultDto(string targetDirectory, string targetFileName)
        {
            string targetFilePath = Path.Combine(targetDirectory, targetFileName);

            if (File.Exists(targetFilePath))
            {
                // ResultDto sınıfı zaten mevcut, güncelleme yap
                string existingContent = File.ReadAllText(targetFilePath);
                string newMethod = GenerateTemplate();

                if (!existingContent.Contains("ResultDto"))
                {
                    int insertIndex = existingContent.LastIndexOf("}");
                    if (insertIndex != -1)
                    {
                        existingContent = existingContent.Insert(insertIndex, newMethod);
                        File.WriteAllText(targetFilePath, existingContent);
                        System.Diagnostics.Debug.WriteLine($"ResultDto güncellendi: {targetFilePath}");
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine($"ResultDto güncellenemedi: {targetFilePath}");
                    }
                }
            }
            else
            {
                // ResultDto sınıfı yok, oluştur
                string newMethod = GenerateTemplate();
                File.WriteAllText(targetFilePath, newMethod);
                System.Diagnostics.Debug.WriteLine($"ResultDto oluşturuldu: {targetFilePath}");
            }
        }
    }
}
