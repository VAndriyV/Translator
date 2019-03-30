using Domain.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services
{
    public class FileService : IFileService
    {
        public async Task SaveFile(string path,string data)
        {
            using (StreamWriter sw = new StreamWriter(path, false, Encoding.Default))
            {
                await sw.WriteAsync(data);
            }
           
        }

        public async Task<string> ReadTextFromFile(string path)
        {
            if (File.Exists(path))
            {

                var fileData = await File.ReadAllTextAsync(path);

                return fileData;
            }
            return "";
        }

    }
}
