﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : DialogText
{
    private int currentActionIndex;
    public enum MenuSelection { Start, Exit }

    private void Start()
    {
        StartCoroutine(TypeDialog("Press Z to select an action..."));
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            AudioManager.instance.PlaySound("Button");

            if (currentActionIndex < 1)
                ++currentActionIndex;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            AudioManager.instance.PlaySound("Button");

            if (currentActionIndex > 0)
                --currentActionIndex;
        }

        UpdateActionSelection(currentActionIndex);

        if (Input.GetKeyDown(KeyCode.Z))
        {
            AudioManager.instance.PlaySound("Button");

            switch ((MenuSelection)currentActionIndex)
            {
                case MenuSelection.Start:
                    //AudioManager.instance.StopSound("MainTheme");
                    //AudioManager.instance.PlaySound("BattleTheme");
                    SceneManager.LoadScene("Selections");
                    break;
                case MenuSelection.Exit:
                    Application.Quit();
                    break;
                default:
                    break;
            }
        }
    }
}
