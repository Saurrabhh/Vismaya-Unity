using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Storage;
using System;
using UnityEngine.SceneManagement;
using System.Linq;

public class FirebaseManager : MonoBehaviour
{
    public static FirebaseAuth auth;
    public static FirebaseUser user;
    public static DatabaseReference database;
    public static StorageReference storage;

    

    public DependencyStatus dependancyStatus;

    private void Awake()
    {

        //FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        //{
        //    dependancyStatus = task.Result;
        //    if (dependancyStatus == DependencyStatus.Available)
        //    {
                InitializeFirebase();
        //    }
        //    else
        //    {
        //        Debug.LogError("Could Not resolve dependenciesss" + dependancyStatus.ToString());
        //    }
        //});

        storage = FirebaseStorage.DefaultInstance.RootReference;
        database = FirebaseDatabase.DefaultInstance.RootReference;


    }


    void InitializeFirebase()
    {
        auth = FirebaseAuth.DefaultInstance;
        auth.StateChanged += AuthStateChanged;
        AuthStateChanged(this, null);
        Debug.Log("Firebase Initialized");
    }

    public static void AuthStateChanged(object sender, EventArgs eventArgs)
    {
        if (auth.CurrentUser != user)
        {
            bool signedIn = user != auth.CurrentUser && auth.CurrentUser != null;
            if (!signedIn && user != null)
            {
                Debug.Log("Signed out " + user.UserId);
                SceneManager.LoadScene((int)Scenes.Auth);
            }
            user = auth.CurrentUser;
            if (signedIn)
            {
                Debug.Log("Signed in " + user.UserId);
            }
        }
    }

    
}
