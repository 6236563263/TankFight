using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class OpeningManager : MonoBehaviour
{
    public GameObject startButton;
    public GameObject rulesButton;
    public GameObject exitButton;
    public GameObject leftArrowButton;
    public GameObject rightArrowButton;
    public GameObject backButton;
    public GameObject goButton;
    public GameObject pageNumber;
    public GameObject[] texts;
    public TMP_InputField inputfield;
    private int page = 0;

    public static OpeningManager Instance;
    public int gameCount = 3;



    private void Awake()
    {
        Instance = this;
    }
    public void SREButtonEnable(bool boo)
    {
        startButton.SetActive(boo);
        rulesButton.SetActive(boo);  
        exitButton.SetActive(boo);
    }
    public void IntroductionButtonEnable(bool boo)
    {
        leftArrowButton.SetActive(boo);
        rightArrowButton.SetActive(boo);
        backButton.SetActive(boo);
    }
    public void PageChange(int i)
    {
        page += i;
        if (page < 0)
        {
            page = 0;
        }
        if (page > texts.Length - 1)
        {
            page = texts.Length - 1;
        }
        HideText();
        texts[page].SetActive(true);
        pageNumber.GetComponent<TextMeshPro>().text = (page + 1) + "\\" + texts.Length;
        Debug.Log(page);
    }
    public void HideText()
    {
        foreach (GameObject text in texts)
        {
            text.SetActive(false);
        }
    }
    public void PageReset()
    {
        page = 0;
    }
    public void ShowPageNumber(bool boo)
    {
        pageNumber.SetActive(boo);
    }
    public void GoEnable(bool boo)
    {
        inputfield.gameObject.SetActive(boo);
        goButton.gameObject.SetActive(boo);
    }
    public void SetGameCount()
    {
        gameCount = int.Parse(inputfield.text);
    }
}
