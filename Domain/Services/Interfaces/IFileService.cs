using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services.Interfaces
{
    public interface IFileService
    {
        Task SaveFile(string path, string data);

        Task<string> ReadTextFromFile(string path);
    }
}
