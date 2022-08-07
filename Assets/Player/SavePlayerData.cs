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
        string path = Application.persistentDataPath + "/PlayerData.vismaya";
        FileStream fs = File.Create(path);
        BinaryFormatter formatter = new BinaryFormatter();

        PlayerData playerData = new PlayerData(player);

        formatter.Serialize(fs, playerData);
        fs.Close();
        string json = JsonUtility.ToJson(playerData);
        Debug.Log(json);


        FirebaseStorage storage = FirebaseStorage.DefaultInstance;
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
        reference.Child("users").Child("nfvaehubyfbcua").SetRawJsonValueAsync(json);
        // Create a root reference
        StorageReference storageRef = storage.RootReference;
        // Create a reference to the file you want to upload
        StorageReference riversRef = storageRef.Child("playerdata.vsmy");

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
        string path = Application.persistentDataPath + "/PlayerData.vismaya";
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
