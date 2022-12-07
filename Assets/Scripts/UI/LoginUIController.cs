using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class LoginUIController : MonoBehaviour
{
    [SerializeField] private LoginScreen loginScreen;
    [SerializeField] private RegisterScreen registerScreen;
     
    private UIScreen _activeScreen;

    private void Start()
    {
        loginScreen.Init(this);
        registerScreen.Init(this);

        OpenLoginScreen();
    }

    public void OpenLoginScreen()
    {
        _activeScreen?.Close();
        loginScreen.Open();
        _activeScreen = loginScreen;
    }
    
    public void OpenRegisterScreen()
    {
        _activeScreen?.Close();
        registerScreen.Open();
        _activeScreen = registerScreen;
    }
}