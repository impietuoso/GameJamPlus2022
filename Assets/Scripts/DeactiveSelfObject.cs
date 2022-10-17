using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactiveSelfObject : MonoBehaviour
{
    public int timeToDeactivate;

    public void Start() {
        StartCoroutine(DieTutorial());
    }

    IEnumerator DieTutorial() {
        yield return new WaitForSeconds(timeToDeactivate);
        GameManager.instance.HideCanvasGroup(this.GetComponent<CanvasGroup>());
    }
}
