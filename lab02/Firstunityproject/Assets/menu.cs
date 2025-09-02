using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using System;
using System.Collections;



public class menu : MonoBehaviour
{
    public Button level1Button;
    public Button level2Button;
    public Button level3Button;
    public TextMeshProUGUI titleText;
    public TMP_InputField nameField;
    void Start()
    {
        level1Button.interactable = false;
        Debug.Log("Start");
        titleText.text = " ";
    }

    void Update()
    {

    }
    public void Playgame()
    {
        SceneManager.LoadSceneAsync("level 1");
    }
    public void Playgame2()
    {
        SceneManager.LoadSceneAsync("SampleScene");
    }


    public void GoButton()
    {
        level1Button.interactable = true;
        level2Button.interactable = true;
        titleText.text = "Welcome " + nameField.text;

    }
}
