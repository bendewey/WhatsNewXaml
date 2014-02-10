using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Storage;
using Windows.Storage.Search;
using NewXaml;
using QSF.Infrastructure.Model;

namespace QSF.Infrastructure.Model
{
    class CodeFileInfoFactory
    {
        public static CodeFileInfo GetCodeFile(Stream stream, string filename)
        {
            if (stream == null) return null;

            using (var streamReader = new StreamReader(stream))
            {
                var codeText = streamReader.ReadToEnd();
                return new CodeFileInfo
                {
                    FileName = filename.Replace(".txt", ""),
                    CodeContent = codeText
                };
            }
        }

        public static CodeFileInfo GetCodeFile(string resourceName)
        {
            if (string.IsNullOrWhiteSpace(resourceName))
                return null;

            var controlAssembly = typeof(CodeFileInfoFactory).GetTypeInfo().Assembly;

            using (var stream = controlAssembly.GetManifestResourceStream(resourceName))
            {
                return GetCodeFile(stream, resourceName);
            }
        }

        public static async Task<CodeFileInfo> GetCodeFile(StorageFile file)
        {
            using (var stream = await file.OpenReadAsync())
            {
                return GetCodeFile(stream.AsStream(), file.Name);
            }
        }

        public static async Task<IEnumerable<CodeFileInfo>> GetCodeFileForType(Type type)
        {
            var name = type.Name;
            var fullName = type.FullName;
            var codeFiles = new List<CodeFileInfo>();

            // return storage files
            var files = await Package.Current.InstalledLocation.GetFilesAsync();
            var matchingfiles = files.Where(f => f.Name.StartsWith(name)).Where(f => Path.GetExtension(f.Name) != ".xbf");
            foreach (var f in matchingfiles)
            {
                codeFiles.Add(await GetCodeFile(f));
            }

            // return resources
            var appAssembly = typeof(App).GetTypeInfo().Assembly;
            var names = appAssembly.GetManifestResourceNames();
            var matchingNames = names.Where(n => n.StartsWith(fullName));
            foreach (var n in matchingNames)
            {
                codeFiles.Add(GetCodeFile(n));
            }
            return codeFiles;
        }
    }
}
