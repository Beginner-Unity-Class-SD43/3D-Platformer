using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
    }

    public void AddCollected()
    {
        collected++;
    }
}
