using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Wordle : MonoBehaviour
{
    [SerializeField]
    private GameObject wordLinePrefab;
    [SerializeField]
    private TMP_Text playerInput;
    [SerializeField]
    private TextAsset possibleAnswer, allowedWord;

    private string answer;
    private string playerAnswer, botAnswer; // For Checking Result only
    private GameObject[] wordLines;
    private int wordLineOffsetY = 60, currentLineAndChance = 0;

    /*private void Start()
    {
        SpawnWordLines();
        GetRandomAnswer();
    }
    public void CheckAnswer()
    {
        playerAnswer = playerInput.text;
        botAnswer = answer;
        SetGrey();
        //SetGreen();
        SetYellow();
        currentLineAndChance++;
    }

    private void SetGreen()
    {
        for (int i = 0; i < 6; i++)
        {
            if (playerAnswer[i].Equals(botAnswer[i]))
            {
                wordLines[currentLineAndChance].transform.GetChild(i).GetComponent<Image>().color = Color.green;
                playerAnswer = playerAnswer.Substring(0, i) + "0";
                if (i + 1 < 6)
                    playerAnswer += playerAnswer.Substring(i + 1, 6);
            }
        }
    }
    private void SetGrey()
    {

    }
    private void SetYellow()
    { 
        
    }
    private void GetRandomAnswer()
    {
        string[] posAnswers = possibleAnswer.ToString().Split();
        //answer = posAnswers[(int)UnityEngine.Random.Range(0, posAnswers.Length - 1)];
    }
    private void SpawnWordLines()
    {
        wordLines = new GameObject[6];
        for (int i = 0; i < 6; i++)
        {
            GameObject line = Instantiate(wordLinePrefab) as GameObject;
            line.transform.SetParent(transform);
            line.GetComponent<RectTransform>().SetLocalPositionAndRotation(Vector3.down * wordLineOffsetY * i, transform.rotation);
            wordLines[i] = line;
        }
    }*/
}
