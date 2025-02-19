using UnityEngine;
using System;
using System.IO;

public class GameDataFileHandler {
    private string dataDirPath;
    private string dataFileName;

    public GameDataFileHandler(string dataDirPath, string dataFileName) {
        this.dataDirPath = dataDirPath;
        this.dataFileName = dataFileName;
    }

    public GameData Load() {
        string fullPath = Path.Combine(dataDirPath, dataFileName);

        GameData loadedData = null;

        if (File.Exists(fullPath)) {
            try {
                string dataToLoad = "";

                using (FileStream stream = new FileStream(fullPath, FileMode.Open)) {
                    using (StreamReader reader = new StreamReader(stream)) {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
            }
            catch (Exception e) {
                Debug.LogError($"An error occured when loading game data: {e.Message}");
            }
        }
        return loadedData;
    }

    public void Save(GameData data) {
        string fullPath = Path.Combine(dataDirPath, dataFileName);

        try {
            string dataToStore = JsonUtility.ToJson(data, true);

            using (FileStream stream = new FileStream(fullPath, FileMode.Create)) {
                using (StreamWriter writer = new StreamWriter(stream)) {
                    writer.Write(dataToStore);
                }
            }
        }
        catch (Exception e) {
            Debug.LogError($"An error occured when saving game data: {e.Message}");
        }
    }

    public void Clear() {
        string fullPath = Path.Combine(dataDirPath, dataFileName);

        if (File.Exists(fullPath)) {
            try {
                File.Delete(fullPath);
            }
            catch (Exception e) {
                Debug.LogError($"Failed to delete file {fullPath}: {e.Message}");
            }
        }
    }
}

