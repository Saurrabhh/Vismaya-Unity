using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Firebase.Storage;
using Firebase;
using Firebase.Database;
using System.Threading.Tasks;
using Firebase.Extensions;

[System.Serializable]
public static class SavePlayerData 
{
    
    public static void SavePlayer(Player player, string format)
    {
        StorageReference storage = FirebaseManager.storage;
        DatabaseReference database = FirebaseManager.database;

        PlayerData playerData = new PlayerData(player);
        string dir = Application.persistentDataPath + $"/{playerData.uid}";
        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }
        string path = dir + $"{playerData.uid}.{format}";
        Debug.Log(path);
        FileStream fs = File.Create(path);
        BinaryFormatter formatter = new BinaryFormatter();

        formatter.Serialize(fs, playerData);
        fs.Close();
        string json = JsonUtility.ToJson(playerData);


        
        
        StorageReference finalPath = storage.Child(playerData.uid).Child($"{playerData.uid}.{format}");
        database.Child("Users").Child(playerData.uid).SetRawJsonValueAsync(json);

        // Upload the file to the path "images/rivers.jpg"
        finalPath.PutFileAsync(path)
            .ContinueWith((Task<StorageMetadata> task) => {       
                if (task.IsFaulted || task.IsCanceled)
                {
                    Debug.Log(task.Exception.ToString());
            // Uh-oh, an error occurred!
                }
                else
                {
            // Metadata contains file metadata such as size, content-type, and download URL.
                    StorageMetadata metadata = task.Result;
                    string md5Hash = metadata.Md5Hash;
                    Debug.Log("Finished uploading...");
                    Debug.Log("md5 hash = " + md5Hash);
                }
            });
    }

    public static PlayerData LoadPlayer(string uid, string format)
    {
        string path = Application.persistentDataPath + $"/{uid}/{uid}.{format}";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream fs = File.OpenRead(path);

            PlayerData playerData = formatter.Deserialize(fs) as PlayerData;
            fs.Close();
            return playerData;
        }
        else
        {
            bool downloaded = false;
            StorageReference dataFile = FirebaseManager.storage.Child(uid).Child($"{uid}.{format}");
            dataFile.GetFileAsync(path).ContinueWithOnMainThread(task => {
                if (!task.IsFaulted && !task.IsCanceled)
                {
                    Debug.Log("File downloaded.");
                    downloaded = true;
                }
            });
            if (downloaded)
            {
                LoadPlayer(uid, format);
            }
            return null;    
        }
    }

    public static void SaveTools(string uid, string format, Tool tool)
    {
        StorageReference storage = FirebaseManager.storage;
        DatabaseReference database = FirebaseManager.database;
        string path = Application.persistentDataPath + $"/{uid}.{format}";
        FileStream fs = File.Create(path);
        BinaryFormatter formatter = new BinaryFormatter();
        ToolData toolData = new(tool);

        formatter.Serialize(fs, toolData);
        fs.Close();
        //string json = JsonUtility.ToJson(Player.totalTools);


        StorageReference riversRef = storage.Child($"{uid}.{format}");
        //database.Child("Users").Child(playerData.uid).SetRawJsonValueAsync(json);

        // Upload the file to the path "images/rivers.jpg"
        riversRef.PutFileAsync(path)
            .ContinueWith((Task<StorageMetadata> task) => {
                if (task.IsFaulted || task.IsCanceled)
                {
                    Debug.Log(task.Exception.ToString());
                    // Uh-oh, an error occurred!
                }
                else
                {
                    // Metadata contains file metadata such as size, content-type, and download URL.
                    StorageMetadata metadata = task.Result;
                    string md5Hash = metadata.Md5Hash;
                    Debug.Log("Finished uploading...");
                    Debug.Log("md5 hash = " + md5Hash);
                }
            });
    }
}
