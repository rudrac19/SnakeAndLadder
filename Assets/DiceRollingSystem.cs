using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceRollingSystem : MonoBehaviour
{
    public GameObject[] diceFaces;

    private void Start()
    {
        foreach (GameObject diceFace in diceFaces)
        {
            diceFace.SetActive(false);
        }
        diceFaces[5].SetActive(true);
    }

    public void Roll()
    {
        int randomInteger = Random.Range(0, 6);
        Debug.Log(randomInteger);
        for (int i = 0; i < 6; i++)
        {
            if (i == randomInteger)
            {
                diceFaces[i].SetActive(true);
            } else {
                diceFaces[i].SetActive(false);
            }
        }

    }
}
