using System.Collections.Generic;
using System.IO;
using System.Linq;
using AutoCodeGenerationVSIX.Commands;
using AutoCodeGenerationVSIX.Commands.BusinessLayer.BusinessServiceAbstract;
using AutoCodeGenerationVSIX.Commands.DataLayer.Context;
using AutoCodeGenerationVSIX.Commands.DataLayer.DalAbstract;
using AutoCodeGenerationVSIX.Commands.DataLayer.EntityFramework;
using AutoCodeGenerationVSIX.Commands.EntityLayer.Dtos;
using AutoCodeGenerationVSIX.Commands.BusinessLayer.Services;
using AutoCodeGenerationVSIX.Commands.BusinessLayer.ClientMessages;
using Microsoft.WindowsAPICodePack.Dialogs;
using AutoCodeGenerationVSIX.Commands.Common.Helpers;
using AutoCodeGenerationVSIX.Commands.Common.Response;
using AutoCodeGenerationVSIX.Commands.DataLayer.Repository;
using AutoCodeGenerationVSIX.Commands.DataLayer.GenericBaseDal;
using AutoCodeGenerationVSIX.Commands.BusinessLayer.GenericBaseAbstract;

namespace AutoCodeGenerationVSIX
{
    [Command(PackageIds.MyCommand)]
    internal sealed class MyCommand : BaseCommand<MyCommand>
    {
        private readonly DtoGenerator _dtoGenerator = new();
        private readonly AppDbContextGenerator _appDbContextGenerator = new();
        private readonly BaseGenericDalGenerator _baseGenericDalGenerator = new();
        private readonly AbstractGenerator _abstractGenerator = new();
        private readonly EntityFrameworkGenerator _entityFrameworkGenerator = new();
        private readonly RepositoryGenerator _repositoryGenerator = new();
        private readonly BaseGenericAbstract _baseGenericAbstract = new();
        private readonly ServiceAbstractGenerator _serviceAbstractGenerator = new();
        private readonly ServiceGenerator _serviceGenerator = new();
        private readonly MessageGenerator _messageGenerator = new();
        private readonly TextGenerator _textGenerator = new();
        private readonly ErrorDtoGenerator _errorDtoGenerator = new();
        private readonly ResultDtoGenerator _resultDtoGenerator = new();
        private readonly ProjectSetupHelper _projectSetupHelper = new();

        protected override async Task ExecuteAsync(OleMenuCmdEventArgs e)
        {
            var docView = await VS.Documents.GetActiveDocumentViewAsync();
            string filePath = docView?.FilePath;
            if (filePath is null)
            {
                await VS.MessageBox.ShowWarningAsync("AutoCodeGenerationVSIX", "Aktif döküman bulunamadı.");
                return;
            }

            string solutionPath = PathHelper.GetProjectPath(filePath);
            if (string.IsNullOrEmpty(solutionPath))
            {
                await VS.MessageBox.ShowWarningAsync("AutoCodeGenerationVSIX", "Çözüm dizini bulunamadı.");
                return;
            }

            _projectSetupHelper.SetupProjectStructure(solutionPath);

            string projectPath = PathHelper.GetProjectPath(filePath);
            if (string.IsNullOrEmpty(projectPath))
            {
                await VS.MessageBox.ShowWarningAsync("AutoCodeGenerationVSIX", "Proje dizini bulunamadı.");
                return;
            }

            string modelName = PathHelper.GetModelName(filePath);

            var createdFiles = new List<string>();
            var errors = new List<string>();

            // DTO Oluşturma
            CreateDtosOrUpdates(projectPath, modelName, createdFiles, errors);

            // AppDbContext Güncelleme
            UpdateAppDbContextOrCreate(projectPath, modelName, createdFiles, errors);

            // GenericDal Oluşturma
            CreateGenericDalOrUpdates(projectPath, createdFiles, errors);

            // DalAbstract Oluşturma
            CreateAbstractOrUpdates(projectPath, modelName, createdFiles, errors);

            // EntityFramework Oluşturma
            CreateEntityFrameworkOrUpdates(projectPath, modelName, createdFiles, errors);

            // Repository Oluşturma
            CreateRepositoryOrUpdate(projectPath, createdFiles, errors);

            // GenericService Oluşturma
            CreateGenericServiceOrUpdates(projectPath, createdFiles, errors);

            // ServiceAbstract Oluşturma
            CreateServiceAbstractOrUpdates(projectPath, modelName, createdFiles, errors);

            // Service Manager Oluşturma
            CreateServiceOrUpdates(projectPath, modelName, createdFiles, errors);

            // ClientMessages Oluşturma
            CreateMessagesOrUpdates(projectPath, modelName, createdFiles, errors);

            // TextsCheckMethots Oluşturma veya güncelleme
            CreateOrUpdateTextsCheckMethots(projectPath, createdFiles, errors);

            // ErrorDto Oluşturma veya güncelleme
            CreateOrUpdateErrorDtoOrUpdates(projectPath, createdFiles, errors);

            // ResultDto Oluşturma veya güncelleme
            CreateOrUpdateResultDtoOrUpdates(projectPath, createdFiles, errors);

            // Özel ayarlanabilir mesaj kutusu
            await ShowCustomMessageBoxAsync(modelName, createdFiles, errors);
        }

        private void CreateDtosOrUpdates(string projectPath, string modelName, List<string> createdFiles, List<string> errors)
        {
            try
            {
                string dtoOutputPath = Path.Combine(projectPath, "EntityLayer", "Dtos", $"{modelName}Dtos");

                if (!Directory.Exists(dtoOutputPath))
                {
                    Directory.CreateDirectory(dtoOutputPath);
                }

                _dtoGenerator.GenerateDtoFiles(modelName, dtoOutputPath);

                createdFiles.Add($"Create{modelName}Dto.cs");
                createdFiles.Add($"Update{modelName}Dto.cs");
                createdFiles.Add($"List{modelName}Dto.cs");
            }
            catch (Exception ex)
            {
                errors.Add(ex.Message);
            }
        }

        private void UpdateAppDbContextOrCreate(string projectPath, string modelName, List<string> createdFiles, List<string> errors)
        {
            try
            {
                string contextOutputPath = Path.Combine(projectPath, "DataLayer", "Context");
                string contextFileName = "AppDbContext.cs";
                _appDbContextGenerator.CreateOrUpdateAppDbContext(contextOutputPath, contextFileName, modelName);
                createdFiles.Add(contextFileName);
            }
            catch (Exception ex)
            {
                errors.Add(ex.Message);
            }
        }

        private void CreateGenericDalOrUpdates(string projectPath, List<string> createdFiles, List<string> errors)
        {
            try
            {
                string genericDalOutputPath = Path.Combine(projectPath, "DataLayer", "Abstract");

                if (!Directory.Exists(genericDalOutputPath))
                {
                    Directory.CreateDirectory(genericDalOutputPath);
                }

                _baseGenericDalGenerator.GenerateGenericDal(genericDalOutputPath, "IGenericDal.cs");
                createdFiles.Add("IGenericDal.cs");
            }
            catch (Exception ex)
            {
                errors.Add(ex.Message);
            }
        }

        private void CreateAbstractOrUpdates(string projectPath, string modelName, List<string> createdFiles, List<string> errors)
        {
            try
            {
                string abstractOutputPath = Path.Combine(projectPath, "DataLayer", "Abstract");

                if (!Directory.Exists(abstractOutputPath))
                {
                    Directory.CreateDirectory(abstractOutputPath);
                }

                _abstractGenerator.GenerateAbstractFiles(modelName, abstractOutputPath);
                createdFiles.Add($"I{modelName}Dal.cs");
            }
            catch (Exception ex)
            {
                errors.Add(ex.Message);
            }
        }

        private void CreateEntityFrameworkOrUpdates(string projectPath, string modelName, List<string> createdFiles, List<string> errors)
        {
            try
            {
                string entityFrameworkOutputPath = Path.Combine(projectPath, "DataLayer", "EntityFramework");

                if (!Directory.Exists(entityFrameworkOutputPath))
                {
                    Directory.CreateDirectory(entityFrameworkOutputPath);
                }

                _entityFrameworkGenerator.GenerateEntityFrameworkFiles(modelName, entityFrameworkOutputPath);
                createdFiles.Add($"EF{modelName}Dal.cs");
            }
            catch (Exception ex)
            {
                errors.Add(ex.Message);
            }
        }

        private void CreateGenericServiceOrUpdates(string projectPath, List<string> createdFiles, List<string> errors)
        {
            try
            {
                string genericServiceOutputPath = Path.Combine(projectPath, "BusinessLayer", "Abstract");

                if (!Directory.Exists(genericServiceOutputPath))
                {
                    Directory.CreateDirectory(genericServiceOutputPath);
                }

                _baseGenericAbstract.GenerateGenericDal(genericServiceOutputPath, "IGenericService.cs");
                createdFiles.Add("IGenericService.cs");
            }
            catch (Exception ex)
            {
                errors.Add(ex.Message);
            }
        }

        private void CreateServiceAbstractOrUpdates(string projectPath, string modelName, List<string> createdFiles, List<string> errors)
        {
            try
            {
                string serviceAbstractOutputPath = Path.Combine(projectPath, "BusinessLayer", "Abstract");

                if (!Directory.Exists(serviceAbstractOutputPath))
                {
                    Directory.CreateDirectory(serviceAbstractOutputPath);
                }

                _serviceAbstractGenerator.GenerateServiceAbstractFiles(modelName, serviceAbstractOutputPath);
                createdFiles.Add($"I{modelName}Service.cs");
            }
            catch (Exception ex)
            {
                errors.Add(ex.Message);
            }
        }

        private void CreateRepositoryOrUpdate(string projectPath, List<string> createdFiles, List<string> errors)
        {
            try
            {
                string repositoryOutputPath = Path.Combine(projectPath, "DataLayer", "Repositories");
                string repositoryFileName = "GenericRepository.cs";

                _repositoryGenerator.GenerateRepository(repositoryOutputPath, repositoryFileName);
                createdFiles.Add(repositoryFileName);
            }
            catch (Exception ex)
            {
                errors.Add(ex.Message);
            }
        }

        private void CreateServiceOrUpdates(string projectPath, string modelName, List<string> createdFiles, List<string> errors)
        {
            try
            {
                string serviceOutputPath = Path.Combine(projectPath, "BusinessLayer", "Concrete");

                if (!Directory.Exists(serviceOutputPath))
                {
                    Directory.CreateDirectory(serviceOutputPath);
                }

                _serviceGenerator.GenerateServiceFiles(modelName, serviceOutputPath);
                createdFiles.Add($"{modelName}Manager.cs");
            }
            catch (Exception ex)
            {
                errors.Add(ex.Message);
            }
        }

        private void CreateMessagesOrUpdates(string projectPath, string modelName, List<string> createdFiles, List<string> errors)
        {
            try
            {
                string messageOutputPath = Path.Combine(projectPath, "BusinessLayer", "ClientMessages");

                if (!Directory.Exists(messageOutputPath))
                {
                    Directory.CreateDirectory(messageOutputPath);
                }

                _messageGenerator.GenerateMessageFiles(modelName, messageOutputPath);
                createdFiles.Add($"{modelName}Messages.cs");
            }
            catch (Exception ex)
            {
                errors.Add(ex.Message);
            }
        }

        private void CreateOrUpdateTextsCheckMethots(string projectPath, List<string> createdFiles, List<string> errors)
        {
            try
            {
                string commonHelpersOutputPath = Path.Combine(projectPath, "Common", "Helpers", "TextMethots");
                string commonHelpersFileName = "TextsCheckMethots.cs";
             
                _textGenerator.CreateOrUpdateTextsCheckMethots(commonHelpersOutputPath, commonHelpersFileName);
                createdFiles.Add(commonHelpersFileName);
            }
            catch (Exception ex)
            {
                errors.Add(ex.Message);
            }
        }

        private void CreateOrUpdateErrorDtoOrUpdates(string projectPath, List<string> createdFiles, List<string> errors)
        {
            try
            {
                string dtoOutputPath = Path.Combine(projectPath, "Common", "Response");
                string dtoFileName = "ErrorDto.cs";

                if (!Directory.Exists(dtoOutputPath))
                {
                    Directory.CreateDirectory(dtoOutputPath);
                }

                _errorDtoGenerator.CreateOrUpdateErrorDto(dtoOutputPath, dtoFileName);
                createdFiles.Add(dtoFileName);
            }
            catch (Exception ex)
            {
                errors.Add(ex.Message);
            }
        }

        private void CreateOrUpdateResultDtoOrUpdates(string projectPath, List<string> createdFiles, List<string> errors)
        {
            try
            {
                string dtoOutputPath = Path.Combine(projectPath, "Common", "Response");
                string dtoFileName = "ResultDto.cs";

                if (!Directory.Exists(dtoOutputPath))
                {
                    Directory.CreateDirectory(dtoOutputPath);
                }

                _resultDtoGenerator.CreateOrUpdateResultDto(dtoOutputPath, dtoFileName);
                createdFiles.Add(dtoFileName);
            }
            catch (Exception ex)
            {
                errors.Add(ex.Message);
            }
        }

        private async Task ShowCustomMessageBoxAsync(string modelName, List<string> createdFiles, List<string> errors)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            if (errors.Count > 0)
            {
                string errorList = string.Join("\n", errors);
                var errorDialog = new TaskDialog
                {
                    Caption = "AutoCodeGenerationVSIX",
                    InstructionText = $"{modelName} için gerekli dosyalar oluşturulamadı.",
                    Text = $"İşlem başarısız oldu. Hatalar:\n{errorList}",
                    Icon = TaskDialogStandardIcon.Error,
                    StandardButtons = TaskDialogStandardButtons.Ok
                };
                errorDialog.Show();
                return;
            }

            var fileNames = createdFiles.Select(Path.GetFileName);
            string fileList = string.Join("\n", fileNames);

            var dialog = new TaskDialog
            {
                Caption = "AutoCodeGenerationVSIX",
                InstructionText = $"{modelName} için gerekli dosyalar oluşturuldu. Oluşturulan veya güncellenen dosyalar:\n{fileList}",
                Text = "İşlem başarıyla tamamlandı.",
                Icon = TaskDialogStandardIcon.Information,
                StandardButtons = TaskDialogStandardButtons.Ok
            };
            dialog.Show();
        }
    }
}
