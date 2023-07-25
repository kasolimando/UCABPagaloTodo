using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using FirebaseAdmin;
using UCABPagaloTodoMS.Core.Services.Firebase;
using Newtonsoft.Json;
using UCABPagaloTodoMS.Infrastructure.Settings;
using Microsoft.AspNetCore.Http;

namespace UCABPagaloTodoMS.Infrastructure.Services.Firebase
{
    public class Firebase : IFirebase
    {
        private readonly StorageClient storageClient;
        public FirebaseApp app;
        public Firebase()
        {
            var jsonCredentials = JsonConvert.SerializeObject(new FirebaseCredentials());
            var credential = GoogleCredential.FromJson(jsonCredentials);
            app = FirebaseApp.Create(new AppOptions()
            {
                Credential = credential
            });
            storageClient = StorageClient.Create(credential);
        }

        //Se elimina la instancia
        public void FirebaseDelete()
        {
            app.Delete();
        }

        //Se lee los datos del archivo especificado
        public async Task<string> ReadFileContentsAsync(string objectName)
        {

            using (var stream = new MemoryStream())
            {
                try
                {
                    await storageClient.DownloadObjectAsync("ucabpagalotodo-56723.appspot.com", objectName, stream);
                    stream.Seek(0, SeekOrigin.Begin);
                    var reader = new StreamReader(stream);
                    return reader.ReadToEnd();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    throw new Exception("El nombre de archivo no existe");
                }
            }
        }

        public async Task<string> UploadFile(IFormFile file)
        {
            var objectName = file.FileName;
            var contentType = file.ContentType;

            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                memoryStream.Position = 0;
                await storageClient.UploadObjectAsync("ucabpagalotodo-56723.appspot.com", objectName, contentType, memoryStream);
            }
            return objectName;
        }
    }
}
