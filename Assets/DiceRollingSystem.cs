using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceRollingSystem : MonoBehaviour
{
    public GameObject[] diceFaces;
    public int randomInteger = 0;
    public PlayerSystem playerSystem;

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
        int randomIntegerIndex = Random.Range(0, 6);
        randomInteger = randomIntegerIndex += 1;
        Debug.Log(randomInteger);
        for (int i = 0; i < 6; i++)
        {
            if (i == randomInteger - 1)
            {
                diceFaces[i].SetActive(true);
            }
            else
            {
                diceFaces[i].SetActive(false);
            }
        }
        playerSystem.OnDiceRolled(randomInteger);

    }
}
