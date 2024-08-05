using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Profiling;

public class DialogueManager : MonoBehaviour
{
    Dictionary<int, string[]> dialogData;
    public TextMeshProUGUI dialogText;
    public GameObject canvas;
    public int talkIndex = 0;
    public float dialogInterval = 0.05f;

    private void Awake() {
        dialogData = new Dictionary<int, string[]>();
        initDialog();
    }

    public void resetCanvas() {
        dialogText.text = "";
    }

    private void initDialog() {
        dialogData.Add(100, new string[] {"이 메시지는 테스트 메시지입니다.\n 2줄 테스트" , "여러 개 메시지 테스트", "ㅁㄴㅇㄹ"} );
        dialogData.Add(0, new string[] {"이 메시지는 2번째 메시지입니다.", "2번째 메시지의 2번째 메시지"});
    }

    private string getDialog(int id, int index) {
        if (!dialogData.ContainsKey(id) || index >= dialogData[id].Length) return null;
        return dialogData[id][index];
    }

    public void talk(int id) {
        string dialog = getDialog(id, talkIndex);
        if (dialog == null) {
            GameManager.Instance.playerSc.switchAction(false, "");
            return;
        }

        GameManager.Instance.playerSc.switchAction(true, "talk");
        
        // talk method...
        GameManager.Instance.playerSc.talkingInProgress = true;
        StartCoroutine(talkAnim(dialog));
        talkIndex++;
        
    }

    IEnumerator talkAnim(string m) {
        string dialogMessage = "";
        foreach (char a in m) {
            dialogMessage += a;
            dialogText.text = dialogMessage;
            yield return new WaitForSeconds(dialogInterval);
        }
        GameManager.Instance.playerSc.talkingInProgress = false;
    }

}
