using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System;

public class SimpleChatRoomView : MonoBehaviour
{
    [SerializeField]
    Text MyName;
    [SerializeField]
    InputField InputField;
    [SerializeField]
    Button LogoutButton;
    [SerializeField]
    LogItem LogItemTemplate;
    [SerializeField]
    Transform ContentsRoot;

    public IObservable<Unit> OnLogoutClickedAsObservable()
    {
        return LogoutButton.OnClickAsObservable();
    }

    public IObservable<string> OnInputAsObservable()
    {
        return InputField.OnEndEditAsObservable();
    }

    public void ClearInputField()
    {
        InputField.text = "";
    }

    public void SetMyName(string name) => MyName.text = name;

    public void OnJoin(ChatRoomMember member)
    {
        AddLog($"{member.Name}が入室しました");
    }

    public void OnLeave(ChatRoomMember member)
    {
        AddLog($"{member.Name}が退室しました");
    }

    public void OnSpeak(ChatRoomMember member, string message)
    {
        AddLog($"{member.Name}:{message}");
    }

    void AddLog(string text)
    {
        var item = Instantiate(LogItemTemplate);
        item.transform.SetParent(ContentsRoot);
        item.SetText(text);
    }
}
