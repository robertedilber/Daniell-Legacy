using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Daniell.Runtime.IO
{
    public static class BinaryDataFileUtils
    {
        public static void CreateFile<T>(string fileName, T data)
        {
            string path = GetFilePath(fileName);

            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Create);
            formatter.Serialize(stream, data);
            stream.Close();
        }

        public static T ReadFile<T>(string fileName)
        {
            string path = GetFilePath(fileName);

            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            var data = formatter.Deserialize(stream);
            stream.Close();

            return (T)data;
        }

        public static string GetFilePath(string fileName)
        {
            return $"{Application.persistentDataPath}/{fileName}";
        }
    }
}