using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    Collectible[] collectibles;

    int collected;

    [SerializeField] TextMeshProUGUI textMesh;

    // Start is called before the first frame update
    void Start()
    {
        collectibles = FindObjectsOfType<Collectible>();
    }

    // Update is called once per frame
    void Update()
    {
        textMesh.text = collected + "/" + collectibles.Length;

        if(collected >= collectibles.Length)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    public void AddCollected()
    {
        collected++;
    }
}
