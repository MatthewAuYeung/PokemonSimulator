using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectionManager : MonoBehaviour
{
    public enum SelectionState { None, PlayerPokemonSelection, ChooseEnemySelection, ManualEnemySelection, RandomEnemySelecion }

    public enum PokemonSelection { charizard, decidueye, dragpult, electivire, empoleon, garchomp, gardevoir, greninja, hydreigon, jirachi, lapras, lucario, lycanroc, metagross, salamence, snorlax, sylveon, toxicroak, vikavolt }
    public List<Icon> selectionIcons;

    [SerializeField]
    private SelectionDialogBox dialogBox;

    private SelectionState selectionState = SelectionState.None;
    private int currentSelectIndex;
    private int enemySelectionIndex;
    private float timer = 0.0f;
    private bool startRandomize = false;

    private void Start()
    {
        StartCoroutine(dialogBox.TypeDialog("Select your Pokemon..."));
        selectionState = SelectionState.PlayerPokemonSelection;
    }

    private void Update()
    {
        UpdateIconSelection(currentSelectIndex);

        switch (selectionState)
        {
            case SelectionState.None:
                break;
            case SelectionState.PlayerPokemonSelection:
                PlayerPokemonSelection();
                break;
            case SelectionState.ChooseEnemySelection:
                ChooseEnemySeleciton();
                break;
            case SelectionState.ManualEnemySelection:
                ManaulEnemySelection();
                break;
            case SelectionState.RandomEnemySelecion:
                RandomEnemySelection();
                break;
            default:
                break;
        }
    }

    private void PlayerPokemonSelection()
    {
        SelectionInput(true);
    }

    private void ChooseEnemySeleciton()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(dialogBox.TypeDialog("Randomizing enemy Pokemon..."));
            selectionState = SelectionState.RandomEnemySelecion;
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            StartCoroutine(dialogBox.TypeDialog("Select enemy Pokemon..."));
            selectionState = SelectionState.ManualEnemySelection;
        }
    }

    private void ManaulEnemySelection()
    {
        SelectionInput(false);
    }

    private void RandomEnemySelection()
    {
        timer += Time.deltaTime;
        RandomSelection();
    }

    private void SelectionInput(bool isPlayer)
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            AudioManager.instance.PlaySound("Button");

            if (currentSelectIndex < selectionIcons.Count - 1)
                ++currentSelectIndex;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            AudioManager.instance.PlaySound("Button");

            if (currentSelectIndex > 0)
                --currentSelectIndex;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            AudioManager.instance.PlaySound("Button");

            if (currentSelectIndex < selectionIcons.Count - 4)
                currentSelectIndex += 4;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            AudioManager.instance.PlaySound("Button");

            if (currentSelectIndex > 3)
                currentSelectIndex -= 4;
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            AudioManager.instance.PlaySound("Button");
            if (isPlayer)
            {
                Player.instance.pokemonBase = selectionIcons[currentSelectIndex].pokemonBase;
                StartCoroutine(dialogBox.TypeDialog("Press 'R' to randomize or 'M' to choose your enemy..."));
                selectionState = SelectionState.ChooseEnemySelection;
            }
            else
            {
                Enemy.instance.pokemonBase = selectionIcons[currentSelectIndex].pokemonBase;
                ToBattleScene();
            }
        }
    }

    private void UpdateIconSelection(int index)
    {
        for (int i = 0; i < selectionIcons.Count; ++i)
        {
            if (i == index)
                selectionIcons[index].OnSelection();
            else
                selectionIcons[i].OffSelection();
        }
    }

    private void RandomSelection()
    {
        if(timer < 3.0f)
        {
            currentSelectIndex = Random.Range(0, selectionIcons.Count);
        }
        else if(timer > 4.0f)
        {
            Enemy.instance.pokemonBase = selectionIcons[currentSelectIndex].pokemonBase;
            ToBattleScene();
        }
    }
    private void ToBattleScene()
    {
        AudioManager.instance.StopSound("MainTheme");
        AudioManager.instance.PlaySound("BattleTheme");
        SceneManager.LoadScene("BattleScene");
    }
}
