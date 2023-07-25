namespace UCABPagaloTodoWeb.Utilities
{
    public static class GenerateMultiPart
    {
        public static MultipartFormDataContent GenerateFile(IFormFile file)
        {
            var form = new MultipartFormDataContent();
            var fileContent = new StreamContent(file.OpenReadStream());
            form.Add(fileContent, "Archivo", file.FileName);
            return form;
        }
    }
}
