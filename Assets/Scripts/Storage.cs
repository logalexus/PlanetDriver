using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class Storage 
{
    private string _filePath;
    private BinaryFormatter _formatter;

    public Storage()
    {
        var directory = Application.persistentDataPath + "/seves";
        if (!Directory.Exists(directory))
            Directory.CreateDirectory(directory);
        _filePath = directory + "/GameSave.save";
        _formatter = new BinaryFormatter();
    }

    public object Load(object saveDataByDefault)
    {
        if (!File.Exists(_filePath))
        {
            if (saveDataByDefault != null)
                Save(saveDataByDefault);
            return saveDataByDefault;
        }
        var file = File.Open(_filePath, FileMode.Open);
        var saveData = _formatter.Deserialize(file);
        file.Close();
        return saveData;
    }

    public void Save(object saveData)
    {
        using (var file = File.Create(_filePath))
        {
            _formatter.Serialize(file, saveData);
        }
    }
}
