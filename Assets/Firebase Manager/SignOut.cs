using Firebase.Auth;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignOut : MonoBehaviour
{
    public void SignOutBtn()
    {
        FirebaseAuth auth = FirebaseManager.auth;
        auth.SignOut();
        auth.StateChanged += FirebaseManager.AuthStateChanged;
        FirebaseManager.AuthStateChanged(this, null);
    }
}
