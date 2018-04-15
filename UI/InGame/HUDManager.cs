using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour {
    
    public SimpleHealthBar targetHealthBar;
    float targetCurrent, targetMax;
    public Text detectionText;

    void ActivateTargetHealthBar(float current, float max, string str = null)
    {
        targetHealthBar.transform.parent.gameObject.SetActive(true);
        if (str != null)
        {
            targetHealthBar.additionalText = str;
        }
        targetHealthBar.UpdateBar(current, max);
    }

    public void UpdateTargetHealthBar(float current, float max)
    {
        targetHealthBar.transform.parent.gameObject.SetActive(true);
        targetHealthBar.UpdateBar(current, max);
        StopCoroutine(WaitThenDeactivateTargetHealthBar());
        StartCoroutine(WaitThenDeactivateTargetHealthBar());
    }

    IEnumerator WaitThenDeactivateTargetHealthBar()
    {
        yield return new WaitForSecondsRealtime(6);
        targetHealthBar.transform.parent.gameObject.SetActive(false);
    }

    public void UpdateDetectionText(string newText)
    {
        detectionText.gameObject.SetActive(true);
        detectionText.text = newText;
    }

    IEnumerator WaitThenDeactivateDetectionText()
    {
        yield return new WaitForSecondsRealtime(3);
        detectionText.gameObject.SetActive(false);
    }
}
