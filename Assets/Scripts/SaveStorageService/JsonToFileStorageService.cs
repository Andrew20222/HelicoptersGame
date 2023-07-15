using System;
using System.IO;
using UnityEngine;

public class JsonToFileStorageService : IStorageService
{
    public void Save(string key, object data)
    {
        var path = BuildPath(key);
        var json = JsonUtility.ToJson(data);
        using (var fileStream = new StreamWriter(path))
        {
            fileStream.Write(json);
        }
    }

    public T Load<T>(string key)
    {
        var path = BuildPath(key);
        using (var fileReading = new StreamReader(path))
        {
            var json = fileReading.ReadToEnd();
            var data = JsonUtility.FromJson<T>(json);
            return data;
        }
    }

    private string BuildPath(string key)
    {
        return Path.Combine(Application.persistentDataPath, key);
    }
}