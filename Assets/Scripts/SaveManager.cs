using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveManager
{
    private static string saveFilePath = Path.Combine(Application.persistentDataPath, "save.dat");

    public static void SaveGame(SaveData data)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream file = File.Create(saveFilePath);
        formatter.Serialize(file, data);
        file.Close();
        Debug.Log("Игра сохранена: " + saveFilePath);
    }

    public static SaveData LoadGame()
    {
        if (File.Exists(saveFilePath))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream file = File.Open(saveFilePath, FileMode.Open);
            SaveData data = formatter.Deserialize(file) as SaveData;
            file.Close();
            Debug.Log("Игра загружена: " + saveFilePath);
            return data;
        }
        else
        {
            Debug.Log("Сохраненный файл не найден, создается новый SaveData");
            return new SaveData { cellStates = new List<CellState>(), bestScore = 0 };
        }
    }
}