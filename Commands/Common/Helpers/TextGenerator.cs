using System;
using System.IO;

namespace AutoCodeGenerationVSIX.Commands.Common.Helpers
{
    public class TextGenerator
    {
        private string GenerateTemplate()
        {
            return $@"using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Helpers.TextMethots
{{
    public class TextsCheckMethots
    {{
        // Girilen her kelimeyi büyük harfle başlayacak şekilde düzenler. Örnek: ""ahmet"" -> ""Ahmet"" veya ""al yazma"" -> ""Al Yazma""
        // You can add more text normalization methods here.
        public string NormalizeText(string text)
        {{
            var words = text.Split(' ');
            for (int i = 0; i < words.Length; i++)
            {{
                if (words[i].Length > 0)
                {{
                    var firstLetter = words[i].Substring(0, 1).ToUpperInvariant();
                    var restOfWord = words[i].Substring(1).ToLowerInvariant();
                    words[i] = firstLetter + restOfWord;
                }}
            }}
            return string.Join(' ', words);
        }}
    }}
}}
";
        }

        public void CreateOrUpdateTextsCheckMethots(string targetDirectory, string targetFileName)
        {
            string targetFilePath = Path.Combine(targetDirectory, targetFileName);

            if (File.Exists(targetFilePath))
            {
                // TextsCheckMethots sınıfı zaten mevcut, güncelleme yap
                string existingContent = File.ReadAllText(targetFilePath);
                string newMethod = GenerateTemplate();

                if (!existingContent.Contains("NormalizeText"))
                {
                    int insertIndex = existingContent.LastIndexOf("}");
                    if (insertIndex != -1)
                    {
                        existingContent = existingContent.Insert(insertIndex, newMethod);
                        File.WriteAllText(targetFilePath, existingContent);
                        System.Diagnostics.Debug.WriteLine($"TextsCheckMethots güncellendi: {targetFilePath}");
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("Kapanış süslü parantezi bulunamadı.");
                    }
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("NormalizeText metodu zaten mevcut.");
                }
            }
            else
            {
                // TextsCheckMethots sınıfı mevcut değil, yeni oluştur
                string content = GenerateTemplate();

                if (!Directory.Exists(targetDirectory))
                {
                    Directory.CreateDirectory(targetDirectory);
                }

                File.WriteAllText(targetFilePath, content);
                System.Diagnostics.Debug.WriteLine($"TextsCheckMethots oluşturuldu: {targetFilePath}");
            }
        }
    }
}
