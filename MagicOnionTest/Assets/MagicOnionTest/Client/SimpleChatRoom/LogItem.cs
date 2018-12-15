using UnityEngine;
using UnityEngine.UI;

class LogItem : MonoBehaviour
{
    [SerializeField]
    Text Text;

    public void SetText(string text) => Text.text = text;
}
