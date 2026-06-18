using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagmentSystem.BLL.Services.AttachmentService
{
    public interface IAttachmentService
    {
        Task<string?> UploadAsync(Stream fileStream, string fileName,string FolderName,CancellationToken ct=default);
        bool Delete(string FileName,string FolderName);
        (Stream stream,string ContentType)? GetFile(string FileName,string FolderName);

    }
}
