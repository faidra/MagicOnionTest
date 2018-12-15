using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class IndexController : MonoBehaviour
{
    [SerializeField]
    Button LoginButton;
    [SerializeField]
    InputField NameInputField;
    [SerializeField]
    InputField HostInputField;
    [SerializeField]
    InputField PortInputField;
    [SerializeField]
    SimpleChatLoginContext LoginContext;
    [SerializeField]
    Button ExitButton;

    void Start()
    {
        NameInputField.text = LoginContext.UserName;
        HostInputField.text = LoginContext.Host;
        PortInputField.text = LoginContext.Port.ToString();

        LoginButton.OnClickAsObservable().Subscribe(_ => Login()).AddTo(this);
        ExitButton.OnClickAsObservable().Subscribe(_ => Exit()).AddTo(this);
    }

    void Login()
    {
        LoginContext.UserName = NameInputField.text;
        LoginContext.Host = HostInputField.text;
        LoginContext.Port = int.Parse(PortInputField.text);

        UnityEngine.SceneManagement.SceneManager.LoadScene("SimpleChatRoom");
    }

    void Exit()
    {
        Application.Quit();
    }
}
