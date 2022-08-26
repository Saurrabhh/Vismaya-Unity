using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using UnityEngine.SceneManagement;

public class AuthManager : MonoBehaviour
{
    [SerializeField] TMP_InputField emailFieldLogin;
    [SerializeField] TMP_InputField passwordFieldLogin;


    [SerializeField] TMP_InputField emailFieldRegister;
    [SerializeField] TMP_InputField passwordFieldRegister;
    [SerializeField] TMP_InputField confirmPasswordFieldRegister;
    [SerializeField] TMP_InputField nameField;
    [SerializeField] TMP_InputField ageField;
    [SerializeField] TMP_Dropdown genderDropdown;

    [SerializeField] GameObject panel;
    public LevelLoader levelLoader;

    public FirebaseAuth auth;
    public FirebaseUser user;
    public DatabaseReference database;
    public Player player;
    string format;
    private void Start()
    {
        auth = FirebaseManager.auth;
        user = FirebaseManager.user;
        database = FirebaseManager.database;
        format = "vismaya";
        if(auth.CurrentUser != null)
        {
            Debug.Log("lalalalalala");
            panel.SetActive(true);
            player.LoadPlayer(auth.CurrentUser.UserId, format);
            levelLoader.LoadLevel(Scenes.Terrain);
        }
        
    }


    public void LoginBtn()
    {
        StartCoroutine(Login(emailFieldLogin.text, passwordFieldLogin.text));
    }

    public void RegisterBtn()
    {    
        StartCoroutine(Register(emailFieldRegister.text, passwordFieldRegister.text, confirmPasswordFieldRegister.text, nameField.text, int.Parse(ageField.text), genderDropdown.options[genderDropdown.value].text));
    }

    public void SignOutBtn()
    {
        auth.SignOut();
        auth.StateChanged += FirebaseManager.AuthStateChanged;
        FirebaseManager.AuthStateChanged(this, null);
    }


    public IEnumerator Login(string _email, string _password)
    {
        //Call the Firebase auth signin function passing the email and password
        var LoginTask = auth.SignInWithEmailAndPasswordAsync(_email, _password);
        panel.SetActive(true);
        

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
            player.LoadPlayer(user.UserId, format);
            Debug.LogFormat("User signed in successfully: {0} ({1})", user.DisplayName, user.Email);
            levelLoader.LoadLevel(Scenes.Terrain);
        }

    }


    private IEnumerator Register(string _email, string _password, string _confirmpassword, string _name, int _age, string _gender)
    {
        if(_password != _confirmpassword)
        {
            Debug.Log("nooo");
            yield break;
        }

        //Call the Firebase auth signin function passing the email and password
        var RegisterTask = auth.CreateUserWithEmailAndPasswordAsync(_email, _password);
        panel.SetActive(true);
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
                    Player.pName = _name;
                    Player.age = _age;
                    Player.gender = _gender;
                    Player.email = _email;
                    Player.uid = user.UserId;
                    Player.currentSceneIndex = (int)Scenes.Museum;
                    Player.money = 100;
                    
                    
                    SavePlayerData.SavePlayer(player, format);
                    levelLoader.LoadLevel(Scenes.Terrain);
                    
                }
            }
        }

    }
}
