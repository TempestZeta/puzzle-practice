using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ComboText : MonoBehaviour
{
    public Text comboText;
    RectTransform rectTransform;
    RectTransform moveRect;
    public Outline outline;

    WaitForSeconds forSeconds;
    Color color;
    Color outColor;
    private void Awake()
    {

        forSeconds = new WaitForSeconds(0.1f);
        comboText.text = "";
        color = comboText.color;
        outColor = outline.effectColor;
        rectTransform = GetComponent<RectTransform>(); 
        moveRect = GameObject.FindGameObjectWithTag("ComboText").GetComponent<RectTransform>();
    }
 
    public void SetComboText(int count)
    {
        if (count == 0) {
            StartCoroutine(Colorchange());
            return;
        }
        comboText.text = count + "COMBO";
        //rectTransform.position = moveRect.position;
        //StartCoroutine(MoveToText());
        iTween.ValueTo(gameObject, iTween.Hash("from", moveRect.anchoredPosition, "to", rectTransform.anchoredPosition, "time", 0.1f, "onupdatetarget", this.gameObject,
        "onupdate", "MoveText"));      
    }

    void MoveText(Vector2 pos)
    {
        rectTransform.anchoredPosition = pos;
    }

    IEnumerator Colorchange()
    {
        float alpha=1.0f;
        while (comboText.color.a > 0.0f)
        {
            alpha -= 0.1f;
            color.a = alpha;
            outColor.a = alpha;
            comboText.color = color;
            outline.effectColor = outColor;
            yield return new WaitForSeconds(0.01f);
        }
        comboText.text = "";
        alpha =1.0f;
        color.a = alpha;
        outColor.a = alpha;
        comboText.color = color;
        outline.effectColor = outColor;
        yield break;
    }
}
