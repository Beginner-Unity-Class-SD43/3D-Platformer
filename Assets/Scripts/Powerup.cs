using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class Powerup : MonoBehaviour
{
    public UnityEvent collectEvent;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            collectEvent.Invoke();
            gameObject.SetActive(false);
        }
    }
}
