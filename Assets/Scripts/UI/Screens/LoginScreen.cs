using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoginScreen : UIScreen
{
    [SerializeField] private ScreenTransition _screenTransition;
    [SerializeField] private TMP_InputField mailInput;
    [SerializeField] private TMP_InputField passwordInput;
    [SerializeField] private Button loginButton;
    [SerializeField] private Button registerButton;

    public void Init(LoginUIController uiController)
    {
        loginButton.onClick.AddListener(() => OnLoginWithMail());
        registerButton.onClick.AddListener(uiController.OpenRegisterScreen);
    }

    public async UniTaskVoid OnLoginWithMail()
    {
        if (InputValid())
        {
            // get credentials
            string mail = mailInput.text;
            string password = passwordInput.text;
        }
        else
        {
        }
    }

    private bool InputValid()
    {
        bool mailValid = !string.IsNullOrEmpty(mailInput.text);
        bool passwordValid = !string.IsNullOrEmpty(passwordInput.text);
        return mailValid && passwordValid;
    }

    public override void Open()
    {
        base.Open();
        _screenTransition.OpenAnim();
    }

    public override void Close()
    {
        _screenTransition.CloseAnim().OnComplete(() => { base.Close(); });
    }
}