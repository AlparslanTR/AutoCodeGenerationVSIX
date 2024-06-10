using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCodeGenerationVSIX.Commands.BusinessLayer.ClientMessages
{
    public class MessageGenerator
    {
        private string GenerateTemplate(string modelName)
        {
            return $@"using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.ClientMessages
{{
    public static class {modelName}Messages
    {{
        public const string {modelName}NotFound = ""{modelName} Bulunamadı.!"";
        public const string {modelName}NotCreated = ""{modelName} Oluşturulamadı.!"";
        public const string {modelName}Created = ""{modelName} Oluşturuldu.!"";
        public const string {modelName}NotUpdated = ""{modelName} Güncellenemedi.!"";
        public const string {modelName}Updated = ""{modelName} Güncellendi.!"";
        public const string {modelName}NotDeleted = ""{modelName} Silinemedi.!"";
        public const string {modelName}Deleted = ""{modelName} Silindi.!"";
        public const string {modelName}NotListed = ""{modelName} Listelenemedi.!"";
        public const string {modelName}Listed = ""{modelName} Listelendi.!"";
        public const string {modelName}IsNotNull = ""{modelName} Boş Olamaz.!"";
        public const string {modelName}NameLength = ""Minimum 3 Karakter Olmalı ve En Fazla 100 Karakter Olmalıdır.!"";
        public const string {modelName}NameExists = ""{modelName} Adı Kullanılmaktadır.!"";
    }}
}}
";
        }
        public void GenerateMessageFiles(string modelName, string outputPath)
        {
            var messageContent = GenerateTemplate(modelName);
            var messagePath = Path.Combine(outputPath, $"{modelName}Messages.cs");
            File.WriteAllText(messagePath, messageContent);
        }
    }
}
