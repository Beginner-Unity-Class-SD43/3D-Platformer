using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    InputManager inputManager;
    [SerializeField] Dialogue dialogue;
    [SerializeField] GameObject interactCanvas;

    bool canInteract;

    // Start is called before the first frame update
    void Start()
    {
        inputManager = FindObjectOfType<InputManager>();
        canInteract = false;
        interactCanvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(inputManager.interactInput && canInteract && !dialogue.gameObject.activeInHierarchy)
        {
            canInteract = false;
            dialogue.gameObject.SetActive(true);
        }
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !dialogue.gameObject.activeInHierarchy)
        {
            canInteract = true;
            interactCanvas.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            canInteract = false;
            interactCanvas.SetActive(false);
            if (dialogue.gameObject.activeInHierarchy)
            {
                dialogue.gameObject.SetActive(false);
            }
        }
    }
}
