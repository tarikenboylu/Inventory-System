using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class SaveSystem
{
    public static void Save<T>(T _obj, string saveFileName) where T : class
    {
        BinaryFormatter formatter = new();
        FileStream stream = new(saveFileName, FileMode.Create);

        formatter.Serialize(stream, _obj);
        stream.Close();
        Debug.Log("Saved Succesfully as " + saveFileName);
    }

    public static T Load<T>(string saveFileName) where T : class
    {
        if (!File.Exists(saveFileName))
        {
            Debug.LogError("Save file not exist" + saveFileName);
            return null;
        }

        BinaryFormatter formatter = new();
        FileStream stream = new(saveFileName, FileMode.Open);

        if (stream.Length == 0)
        {
            Debug.LogError("Save file is empty");
            return null;
        }
        
        T data = formatter.Deserialize(stream) as T;
        stream.Close();

        Debug.Log("Load Succesfully");
        return data;
    }
}