using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDManager : MonoBehaviour {
    
    public SimpleHealthBar targetHealthBar;
    float targetCurrent, targetMax;

    void ActivateTargetHealthBar(float current, float max)
    {
        targetHealthBar.transform.parent.gameObject.SetActive(true);
        targetHealthBar.UpdateBar(current, max);
    }

    public void UpdateTargetHealthBar(float current, float max)
    {
        targetHealthBar.transform.parent.gameObject.SetActive(true);
        targetHealthBar.UpdateBar(current, max);
        StopAllCoroutines();
        StartCoroutine(WaitThenDeactivateTargetHealthBar());
    }

    IEnumerator WaitThenDeactivateTargetHealthBar()
    {
        yield return new WaitForSecondsRealtime(10);
        targetHealthBar.transform.parent.gameObject.SetActive(false);
    }
}
