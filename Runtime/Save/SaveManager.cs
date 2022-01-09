using Daniell.Runtime.DataStructures;
using Daniell.Runtime.GUIDSystem;
using Daniell.Runtime.Save;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace Daniell.Runtime.Save
{
    public static class SaveManager
    {
        [System.Serializable]
        private struct DataSaverContent
        {
            public string guid;
            public SaveDataContainer[] saveDataContainers;
        }

        private static List<DataSaver> _registeredDataSavers = new List<DataSaver>();

        public static void Register(DataSaver dataSaver)
        {
            _registeredDataSavers.Add(dataSaver);
        }

        public static void Unregister(DataSaver dataSaver)
        {
            _registeredDataSavers.Remove(dataSaver);
        }

#if UNITY_EDITOR
        [MenuItem("Daniell/Save System/Load")]
#endif
        public static void Load()
        {
            BinaryFormatter formatter = new BinaryFormatter();
            string path = $"{Application.persistentDataPath}/UserData.dat";

            if (File.Exists(path))
            {
                FileStream stream = new FileStream(path, FileMode.Open);
                var gameData = ((ValueWrapper<List<DataSaverContent>>)formatter.Deserialize(stream)).value;
                stream.Close();

                Dictionary<string, SaveDataContainer[]> dataSaverContents = new Dictionary<string, SaveDataContainer[]>();

                for (int i = 0; i < gameData.Count; i++)
                {
                    DataSaverContent v = gameData[i];
                    dataSaverContents.Add(v.guid, v.saveDataContainers);
                }

                for (int i = 0; i < _registeredDataSavers.Count; i++)
                {
                    var dataSaver = _registeredDataSavers[i];

                    // Save data
                    dataSaver.Load(dataSaverContents[dataSaver.GUID]);
                }
            }
        }

#if UNITY_EDITOR
        [MenuItem("Daniell/Save System/Save")]
#endif
        public static void Save()
        {
            List<DataSaverContent> dataSaverContent = new List<DataSaverContent>();

            for (int i = 0; i < _registeredDataSavers.Count; i++)
            {
                var dataSaver = _registeredDataSavers[i];

                var listOfContainers = dataSaver.Save();
                dataSaverContent.Add(new DataSaverContent() { guid = dataSaver.GUID, saveDataContainers = listOfContainers });
            }

            var gameData = new ValueWrapper<List<DataSaverContent>>(dataSaverContent);

            // Format in binary
            BinaryFormatter formatter = new BinaryFormatter();
            string path = $"{Application.persistentDataPath}/UserData.dat";

            FileStream stream = new FileStream(path, FileMode.OpenOrCreate);
            formatter.Serialize(stream, gameData);
            stream.Close();
        }
    }
}