using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace SolStandard.Utility.System
{
    public class WindowsFileIO : IFileIO
    {
        public const string GameFolder = "SolStandard";
        private static readonly string SaveFolder = Path.Combine(Path.GetTempPath(), GameFolder);

        public void Save(string fileName, object content)
        {
            Directory.CreateDirectory(SaveFolder);

            string fileToSaveTo = Path.Combine(SaveFolder, fileName);
            using (Stream stream = File.OpenWrite(fileToSaveTo))
            {
                new BinaryFormatter().Serialize(stream, content);
            }
        }

        public T Load<T>(string fileName)
        {
            string fileToLoadFrom = Path.Combine(SaveFolder, fileName);

            if (!Directory.Exists(SaveFolder)) return default(T);

            using (Stream stream = File.OpenRead(fileToLoadFrom))
            {
                return (T) new BinaryFormatter().Deserialize(stream);
            }
        }
    }
}