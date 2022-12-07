using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RegisterScreen : UIScreen
{
    [SerializeField] private ScreenTransition _screenTransition;
    [SerializeField] private TMP_InputField mailInput;
    [SerializeField] private TMP_InputField passwordInput;
    [SerializeField] private TMP_InputField repeatedPasswordInput;
    [SerializeField] private Button registerButton;
    [SerializeField] private Button backButton;

    public void Init(LoginUIController uiController)
    {
        //registerButton.onClick.AddListener();
        backButton.onClick.AddListener(uiController.OpenLoginScreen);
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