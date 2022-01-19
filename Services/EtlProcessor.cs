using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using DataExchangeWorkerService.Helpers;
using DataExchangeWorkerService.Models;
using Microsoft.Extensions.Logging;

namespace DataExchangeWorkerService.Services
{
    public class EtlProcessor
    {
        private readonly ILogger<EtlProcessor> _logger;
        private readonly WorkerOptions _options;
        private readonly string _sourceFilePath;
        private readonly string _destinationFilePath;


        public EtlProcessor(ILogger<EtlProcessor> logger, WorkerOptions options)
        {
            _logger = logger;
            _options = options;
            _sourceFilePath = "Files\\TransformationFiles\\";
            _destinationFilePath = "Files\\Archived\\";
        }

        public async Task DoWork()
        {
            _logger.LogInformation("DoWork work");
            foreach (var optionsClient in _options.Clients)
            {
                _logger.LogInformation("Client Name: " + optionsClient);
            }

            await FileProcessor();
        }

        public async Task FileProcessor()
        {
            FileInfo[] files = GetAllFiles(_sourceFilePath);

            foreach (var file in files)
            {
                var clientName = GetClientNameForFile(file.Name);
                var clientType = GetClientType(clientName);
                switch (clientName)
                {
                    case "ClientA":
                        var fileData = await ExtractDataFromFile<ClientAModel>(file.FullName, 1);
                        var filePath = await LoadData<ClientAModel>(fileData, clientName, file.FullName);
                        break;
                    case "ClientB":
                        Console.WriteLine("ClientB: " + clientName);
                        break;
                    case "ClientC":
                        Console.WriteLine("ClientC: " + clientName);
                        break;
                    default:
                        Console.WriteLine("Invalid Client Name");
                        break;
                }
            }
        }

        public async Task<IList<T>> ExtractDataFromFile<T>(string filePath, int startRow)
        {
            Console.WriteLine("Extract Data");
            return default;
        }

        // public async Task<IList<T>> TransformData<T>(List<T> data)
        // {
        //     Console.WriteLine(filePath + " start row: " + startRow);
        //     return default;
        // }

        public async Task<string> LoadData<T>(IList<T> data, string clientName, string filePath)
        {
            Console.WriteLine("Load Data");

            MoveFile(clientName, filePath, _sourceFilePath, _destinationFilePath);
            return default;
        }

        #region Helper Method

        private FileInfo[] GetAllFiles(string directoryPath)
        {
            var pathBuilt = Path.Combine(Directory.GetCurrentDirectory(), directoryPath);
            DirectoryInfo directory = new DirectoryInfo(pathBuilt);
            return directory.GetFiles().Where(c => c.Extension == ".xlsx").OrderBy(c => c.CreationTimeUtc).ToArray();
        }

        private static void MoveFile(string clientName, string filePath, string sourcePath, string destinationPath)
        {
            sourcePath = Path.Combine(Directory.GetCurrentDirectory(), sourcePath);
            destinationPath = Path.Combine(Directory.GetCurrentDirectory(), destinationPath);
            if (!Directory.Exists(destinationPath))
            {
                Directory.CreateDirectory(destinationPath);
            }

            DirectoryInfo directory = new DirectoryInfo(sourcePath);
            var file = directory.GetFiles().FirstOrDefault(c => c.FullName == filePath);
            if (file is { Exists: true })
            {
                var path = Path.Combine(destinationPath,
                    clientName + DateTime.UtcNow.ToString("yyyy-MM-dd-hh-mm-ss") + file.Extension);
                File.Move(file.FullName, path);
            }
        }

        private string GetClientNameForFile(string fileName)
        {
            return _options.Clients.FirstOrDefault(fileName.Contains);
        }

        private Type GetClientType(string clientName)
        {
            var types = Assembly.GetExecutingAssembly().GetTypes();
            foreach (var type in types) 
            {
                if (type.Name.Contains(clientName))
                {
                    return type;
                }
            }

            return null;
        }

        #endregion
    }
}
