using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Firebase.Storage;
using Firebase;
using Firebase.Database;
using System.Threading.Tasks;

public static class SavePlayerData 
{
    public static void SavePlayer(Player player)
    {
        PlayerData playerData = new PlayerData(player);
        string path = Application.persistentDataPath + $"/{playerData.uid}.vismaya";
        Debug.Log(path);
        FileStream fs = File.Create(path);
        BinaryFormatter formatter = new BinaryFormatter();

        formatter.Serialize(fs, playerData);
        fs.Close();
        string json = JsonUtility.ToJson(playerData);


        StorageReference storage = FirebaseManager.storage;
        DatabaseReference database = FirebaseManager.database;
        
        StorageReference riversRef = storage.Child($"{playerData.uid}.vismaya");
        database.Child("Users").Child(playerData.uid).SetRawJsonValueAsync(json);

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

    public static PlayerData LoadPlayer()
    {
        string path = Application.persistentDataPath + $"/{Player.uid}.vismaya";
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
            Debug.LogError("Not Found" + path);
            return null;    
        }
    }
}
