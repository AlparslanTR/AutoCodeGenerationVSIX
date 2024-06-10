using System;
using System.IO;

namespace AutoCodeGenerationVSIX.Commands.Common.Helpers
{
    public class ProjectSetupHelper
    {
        private string GenerateCsprojContent(string libraryName, string[] references = null)
        {
            string referencesContent = string.Empty;
            if (references != null && references.Length > 0)
            {
                referencesContent = "<ItemGroup>\n";
                foreach (var reference in references)
                {
                    referencesContent += $"    <ProjectReference Include=\"..\\{reference}\\{reference}.csproj\" />\n";
                }
                referencesContent += "</ItemGroup>\n";
            }

            return $@"<Project Sdk=""Microsoft.NET.Sdk"">
  <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
  </PropertyGroup>
  <ItemGroup>
    <PackageReference Include=""Microsoft.EntityFrameworkCore"" Version=""8.0.5"" />
    <PackageReference Include=""Microsoft.EntityFrameworkCore.Design"" Version=""8.0.5"" />
    <PackageReference Include=""Microsoft.EntityFrameworkCore.SqlServer"" Version=""8.0.5"" />
    <PackageReference Include=""Microsoft.EntityFrameworkCore.Tools"" Version=""8.0.5"" />
  </ItemGroup>
  {referencesContent}
</Project>";
        }

        public void CreateClassLibrary(string solutionPath, string libraryName, string[] references = null)
        {
            string libraryPath = Path.Combine(solutionPath, libraryName);
            Directory.CreateDirectory(libraryPath);

            string csprojContent = GenerateCsprojContent(libraryName, references);
            File.WriteAllText(Path.Combine(libraryPath, $"{libraryName}.csproj"), csprojContent);
        }

        public void CreateFolders(string libraryPath, params string[] folders)
        {
            foreach (var folder in folders)
            {
                Directory.CreateDirectory(Path.Combine(libraryPath, folder));
            }
        }

        public void SetupProjectStructure(string solutionPath)
        {
            // EntityLayer
            CreateClassLibrary(solutionPath, "EntityLayer");
            string entityLayerPath = Path.Combine(solutionPath, "EntityLayer");
            CreateFolders(entityLayerPath, "Models");

            // DataLayer
            CreateClassLibrary(solutionPath, "DataLayer", ["EntityLayer"]);
            string dataLayerPath = Path.Combine(solutionPath, "DataLayer");
            CreateFolders(dataLayerPath, "Context", "Abstract", "EntityFramework", "Repositories");

            // BusinessLayer
            CreateClassLibrary(solutionPath, "BusinessLayer", ["EntityLayer", "DataLayer", "Common"]);
            string businessLayerPath = Path.Combine(solutionPath, "BusinessLayer");
            CreateFolders(businessLayerPath, "Abstract", "Concrete", "ClientMessages");

            // Common
            CreateClassLibrary(solutionPath, "Common");
            string commonPath = Path.Combine(solutionPath, "Common");
            CreateFolders(commonPath, "Helpers", "Response");
        }
    }
}
