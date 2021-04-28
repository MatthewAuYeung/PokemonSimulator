using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndSceneManager : DialogText
{
    private int currentActionIndex;
    public enum EndSelection { Restart, Exit }

    private void Start()
    {
        if(Player.instance.win)
        {
            StartCoroutine(TypeDialog("You Win!!!"));
        }
        else
        {
            StartCoroutine(TypeDialog("You Loose..."));
        }
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

            switch ((EndSelection)currentActionIndex)
            {
                case EndSelection.Restart:
                    if (Player.instance.win)
                    {
                        AudioManager.instance.StopSound("Win");
                        AudioManager.instance.PlaySound("MainTheme");
                    }
                    SceneManager.LoadScene("Selections");
                    break;
                case EndSelection.Exit:
                    Application.Quit();
                    break;
                default:
                    break;
            }
        }
    }
}
