//using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Wordle : MonoBehaviour
{
    [SerializeField]
    private TextAsset asset_possibleAnswer, asset_allowedWord;
    [SerializeField]
    private GameObject gameObject_wordLinePrefab, gameObject_resultBoard;
    [SerializeField]
    private TMP_InputField input_playerAnswer;

    private GameObject[] array_wordLines_UI;
    private string[] array_allowAnswer, array_possibleAnswer;
    private char[] array_playerAnswer_check, array_botAnswer_check; // For Checking Result Only
    private int int_wordLineOffsetY = 50, int_currentLineAndChance = 0;
    private string string_botAnswer;

    private void Start()
    {
        SETUP_SpawnWordLines();
        SETUP_GetAnswerList();
        SETUP_GetRandomAnswer();
        input_playerAnswer.ActivateInputField();
        //print(string_botAnswer);
    }
    private void Update()
    {
        SubmitAnswer();
    }
    private void SubmitAnswer()     // Ignore
    {
        if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return))
        {
            Public_CheckAnswer();
            input_playerAnswer.ActivateInputField();
        }
    }
    public void Public_Restart()    // Ignore
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void Public_CheckAnswer()
    {
        if (int_currentLineAndChance < 6)
        {
            array_playerAnswer_check = input_playerAnswer.text.ToLower().Trim().ToArray();
            array_botAnswer_check = string_botAnswer.ToArray();

            if (IsValidAnswer())   
            {
                Handler_SetLineColor();
                input_playerAnswer.GetComponent<Image>().color = Color.white; // Input field box's color
                int_currentLineAndChance++;
            }
            else
                input_playerAnswer.GetComponent<Image>().color = Color.red; 

            if (IsSame())   
                int_currentLineAndChance = 6;
            if (int_currentLineAndChance == 6)  
                Handler_EndGame(IsSame() ? true : false);
        }
    }
    private void Handler_EndGame(bool isWin)
    {
        input_playerAnswer.enabled = false;
        gameObject_resultBoard.GetComponent<Image>().color = isWin ? Color.green : Color.red;
        gameObject_resultBoard.transform.SetParent(this.gameObject.transform);
        TMP_Text text_EndGameBoard = gameObject_resultBoard.transform.GetChild(0).GetComponent<TMP_Text>();
        text_EndGameBoard.text = isWin ? "Congratulation, you win!" : "Too bad, the mystery word is --" + string_botAnswer + "--";
    }
    private void Handler_SetLineColor()
    {
        for (int i = 0; i < array_playerAnswer_check.Length; i++)   // Assign letters for UI
        {
            GameObject currentLetterUI = array_wordLines_UI[int_currentLineAndChance].transform.GetChild(i).GetChild(0).gameObject;
            currentLetterUI.GetComponent<TMP_Text>().SetText(array_playerAnswer_check[i] + "");
        }
        Handler_SetGreen(); // Set UI colors in order: Green, Yellow, then Grey
        Handler_SetYellow();
        Handler_SetGrey();
    }
    private void Handler_SetGreen()
    {
        for (int i = 0; i < array_playerAnswer_check.Length; i++)
            if (array_playerAnswer_check[i].Equals(array_botAnswer_check[i]))
            {
                Helper_SetColor(Color.green, i);
                array_botAnswer_check[i] ='0';
            }
    }
    private void Handler_SetYellow()
    {
        for (int i = 0; i < array_playerAnswer_check.Length; i++)
            if (!Helper_GetColor(i).Equals(Color.green) && array_botAnswer_check.Contains(array_playerAnswer_check[i]))
            {
                Helper_SetColor(Color.yellow, i);
                array_botAnswer_check[Helper_IndexOf(array_playerAnswer_check[i])] = '0';
            }
    }
    private void Handler_SetGrey()
    {
        for (int i = 0; i < array_playerAnswer_check.Length; i++)
        {
            if(Helper_GetColor(i).Equals(Color.white))
                Helper_SetColor(Color.grey, i);
        }
    }
    private int Helper_IndexOf(char letter) // Find index of Character in array_botAnswer_check version
    {
        for (int i = 0; i < array_botAnswer_check.Length; i++)
        {
            if (array_botAnswer_check[i].Equals(letter))
                return i;
        }
        return -1;
    }
    private Color Helper_GetColor(int index)    // Get Color of the current Letter at the current Line UI
    {
        Transform currentLine = array_wordLines_UI[int_currentLineAndChance].transform;
        Image currentLetterUI = currentLine.GetChild(index).GetComponent<Image>();
        return currentLetterUI.color;
    }
    private void Helper_SetColor(Color color, int index)   // Set Color for current Letter at current Line UI
    {
        Transform currentLine = array_wordLines_UI[int_currentLineAndChance].transform;
        Image currentLetterUI = currentLine.GetChild(index).GetComponent<Image>();
        currentLetterUI.color = color;
    }
    private bool IsSame()
    {
        return input_playerAnswer.text.ToLower().Equals(string_botAnswer);
    }
    private bool IsValidAnswer()
    {
        string playerAnswer = input_playerAnswer.text.ToLower().Trim();
        return array_allowAnswer.Contains(playerAnswer) || array_possibleAnswer.Contains(playerAnswer);
    }
    private void SETUP_GetAnswerList()  // Split then Trim whitespaces from TextAsset
    {
        array_allowAnswer = asset_allowedWord.text.Split('\n');
        for (int i = 0; i < array_allowAnswer.Length; i++)
            array_allowAnswer[i] = array_allowAnswer[i].Trim(); 

        array_possibleAnswer = asset_possibleAnswer.text.Split('\n');
        for (int i = 0; i < array_possibleAnswer.Length; i++)
            array_possibleAnswer[i] = array_possibleAnswer[i].Trim(); 
    }
    private void SETUP_GetRandomAnswer()    // Get random answer in Possible Answers list
    {
        int randomLetterIndex = (int)Random.Range(0, array_possibleAnswer.Length - 1);
        string_botAnswer = array_possibleAnswer[randomLetterIndex];
    }
    private void SETUP_SpawnWordLines()     // Set up UI display for turns/lines
    {
        array_wordLines_UI = new GameObject[6];
        for (int i = 0; i < 6; i++)
        {
            GameObject line = Instantiate(gameObject_wordLinePrefab) as GameObject;
            line.transform.SetParent(transform);
            line.GetComponent<RectTransform>().SetLocalPositionAndRotation(Vector3.up * (150 - int_wordLineOffsetY * i), transform.rotation);
            array_wordLines_UI[i] = line;
        }
    }
}