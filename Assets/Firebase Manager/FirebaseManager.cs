using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using TMPro;
using System;

public class FirebaseManager : MonoBehaviour
{
    public FirebaseAuth auth;
    public FirebaseUser user;
    public DependencyStatus dependancyStatus;

    public TMP_InputField emailField;
    public TMP_InputField passwordField;

    LevelLoader levelLoader;


    private void Awake()
    {


        levelLoader = FindObjectOfType<LevelLoader>();
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            dependancyStatus = task.Result;
            if (dependancyStatus == DependencyStatus.Available)
            {
                InitializeFirebase();
            }
            else
            {
                Debug.LogError("Could Not resolve dependenciesss" + dependancyStatus.ToString());
            }
        });


    }

    private void OnEnable()
    {
        Cursor.lockState = CursorLockMode.None;
    }

    void InitializeFirebase()
    {
        auth = FirebaseAuth.DefaultInstance;
        auth.StateChanged += AuthStateChanged;
        AuthStateChanged(this, null);
        Debug.Log("Firebase Initialized");


    }

    void AuthStateChanged(object sender, EventArgs eventArgs)
    {
        if (auth.CurrentUser != user)
        {
            bool signedIn = user != auth.CurrentUser && auth.CurrentUser != null;
            if (!signedIn && user != null)
            {
                Debug.Log("Signed out " + user.UserId);
            }
            user = auth.CurrentUser;
            if (signedIn)
            {
                Debug.Log("Signed in " + user.UserId);
            }
        }
    }

    public void LoginBtn()
    {
        StartCoroutine(Login(emailField.text, passwordField.text));
    }

    public void RegisterBtn()
    {
        StartCoroutine(Register(emailField.text, passwordField.text));
    }

    public void SignOutBtn()
    {
        auth.SignOut();
        auth.StateChanged += AuthStateChanged;
        AuthStateChanged(this, null);

    }


    public IEnumerator Login(string _email, string _password)
    {
        //Call the Firebase auth signin function passing the email and password
        var LoginTask = auth.SignInWithEmailAndPasswordAsync(_email, _password);

        //Wait until the task completes
        yield return new WaitUntil(predicate: () => LoginTask.IsCompleted);

        if (LoginTask.Exception != null)
        {
            //If there are errors handle them
            Debug.LogWarning(message: $"Failed to register task with {LoginTask.Exception}");
            FirebaseException firebaseEx = LoginTask.Exception.GetBaseException() as FirebaseException;
            AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

            string message = "Login Failed!";
            switch (errorCode)
            {
                case AuthError.MissingEmail:
                    message = "Missing Email";
                    break;
                case AuthError.MissingPassword:
                    message = "Missing Password";
                    break;
                case AuthError.WrongPassword:
                    message = "Wrong Password";
                    break;
                case AuthError.InvalidEmail:
                    message = "Invalid Email";
                    break;
                case AuthError.UserNotFound:
                    message = "Account does not exist";
                    break;
            }

        }
        else
        {
            //User is now logged in
            //Now get the result
            user = LoginTask.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})", user.DisplayName, user.Email);
            levelLoader.LoadNextLevel();


        }

    }


    private IEnumerator Register(string _email, string _password)
    {

        //Call the Firebase auth signin function passing the email and password
        var RegisterTask = auth.CreateUserWithEmailAndPasswordAsync(_email, _password);
        //Wait until the task completes
        yield return new WaitUntil(predicate: () => RegisterTask.IsCompleted);

        if (RegisterTask.Exception != null)
        {
            //If there are errors handle them
            Debug.LogWarning(message: $"Failed to register task with {RegisterTask.Exception}");
            FirebaseException firebaseEx = RegisterTask.Exception.GetBaseException() as FirebaseException;
            AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

            string message = "Register Failed!";
            switch (errorCode)
            {
                case AuthError.MissingEmail:
                    message = "Missing Email";
                    break;
                case AuthError.MissingPassword:
                    message = "Missing Password";
                    break;
                case AuthError.WeakPassword:
                    message = "Weak Password";
                    break;
                case AuthError.EmailAlreadyInUse:
                    message = "Email Already In Use";
                    break;
            }
        }
        else
        {
            //User has now been created
            //Now get the result
            user = RegisterTask.Result;

            if (user != null)
            {
                //Create a user profile and set the username
                UserProfile profile = new UserProfile { DisplayName = user.DisplayName };

                //Call the Firebase auth update user profile function passing the profile with the username
                var ProfileTask = user.UpdateUserProfileAsync(profile);
                //Wait until the task completes
                yield return new WaitUntil(predicate: () => ProfileTask.IsCompleted);

                if (ProfileTask.Exception != null)
                {
                    //If there are errors handle them
                    Debug.LogWarning(message: $"Failed to register task with {ProfileTask.Exception}");
                    FirebaseException firebaseEx = ProfileTask.Exception.GetBaseException() as FirebaseException;
                    AuthError errorCode = (AuthError)firebaseEx.ErrorCode;
                    //warningRegisterText.text = "Username Set Failed!";
                }
                else
                {
                    Debug.Log("Done eee");
                }
            }
        }

    }
}
