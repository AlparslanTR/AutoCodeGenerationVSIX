using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCodeGenerationVSIX.Commands
{
    public static class PathHelper
    {
            // Proje adını dinamik olarak belirleyecek bir metot
            public static string GetProjectName(string filePath)
            {
                // Proje adını dosya yolundan çıkarmak için örnek bir yöntem
                // Bu yöntemi kendi projenizin yapısına göre özelleştirebilirsiniz
                var directoryInfo = new DirectoryInfo(Path.GetDirectoryName(filePath));
                while (directoryInfo.Parent != null && !directoryInfo.GetFiles("*.csproj").Any())
                {
                    directoryInfo = directoryInfo.Parent;
                }
                var projectFile = directoryInfo.GetFiles("*.csproj").FirstOrDefault();
                return projectFile != null ? Path.GetFileNameWithoutExtension(projectFile.Name) : null;
            }

            // Proje yolunu dinamik olarak belirleyecek bir metot
            public static string GetProjectPath(string filePath)
            {
                string projectName = GetProjectName(filePath);
                string projectPath = Path.GetDirectoryName(filePath);

                while (!string.IsNullOrEmpty(projectPath) && !Directory.Exists(Path.Combine(projectPath, projectName)))
                {
                    projectPath = Path.GetDirectoryName(projectPath);
                }

                return projectPath;
            }

            public static string GetModelName(string filePath)
            {
                return Path.GetFileNameWithoutExtension(filePath);
            }
    }
}
