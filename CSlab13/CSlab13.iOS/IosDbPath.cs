using System;
using Xamarin.Forms;
using System.IO;
using CSlab13.iOS;

[assembly: Dependency(typeof(IosDbPath))]
namespace CSlab13.iOS
{
    public class IosDbPath : IPath
    {
        public string GetDatabasePath(string sqliteFilename)
        {
            // определяем путь к бд
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "..", "Library", sqliteFilename);
        }
    }
}