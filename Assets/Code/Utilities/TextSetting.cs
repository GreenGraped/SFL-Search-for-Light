using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextSetting : MonoBehaviour
{
    public TextMeshProUGUI tmpText;

    void Start()
    {
        // 텍스트 오브젝트의 해상도 확인 및 설정
        Canvas canvas = GetComponentInParent<Canvas>();
        if (canvas.renderMode == RenderMode.WorldSpace)
        {
            tmpText.fontSize = 0.1f; // 월드 스페이스에 적절한 폰트 사이즈 설정
        }
        else
        {
            tmpText.fontSize = 24; // 스크린 스페이스에 적절한 폰트 사이즈 설정
        }

        // 텍스트의 해상도와 관련된 설정을 확인
        tmpText.enableAutoSizing = false;
        tmpText.fontSizeMin = 10;
        tmpText.fontSizeMax = 30;
        tmpText.overflowMode = TextOverflowModes.Overflow;
        tmpText.transform.localScale = Vector3.one;
    }
}
