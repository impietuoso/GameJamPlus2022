using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AreaTriggerEvent : MonoBehaviour {
    public bool oneTimeOnly = true;
    public UnityEvent newEvent;

    private void OnTriggerEnter2D(Collider2D collision) {
        newEvent.Invoke();
        if (oneTimeOnly) gameObject.SetActive(false);
    }
}
