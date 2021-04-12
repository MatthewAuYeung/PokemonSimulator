using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectionManager : MonoBehaviour
{
    public enum PokemonSelection { charizard, decidueye, dragpult, electivire, empoleon, garchomp, gardevoir, greninja, hydreigon, jirachi, lapras, lucario, lycanroc, metagross, salamence, snorlax, sylveon, toxicroak, vikavolt }
    public List<Icon> selectionIcons;

    private int currentSelectIndex;
    private int enemySelectionIndex;
    private float timer = 0.0f;
    private bool startRandomize = false;

    private void Update()
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

            if (currentSelectIndex > 1)
                currentSelectIndex -= 4;
        }

        UpdateIconSelection(currentSelectIndex);

        if (Input.GetKeyDown(KeyCode.Z))
        {
            AudioManager.instance.PlaySound("Button");
            Player.instance.pokemonBase = selectionIcons[currentSelectIndex].pokemonBase;

            startRandomize = true;
        }

        if(startRandomize)
        {
            timer += Time.deltaTime;
            RandomSelection();
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
            AudioManager.instance.StopSound("MainTheme");
            AudioManager.instance.PlaySound("BattleTheme");
            SceneManager.LoadScene("BattleScene");
        }
    }
}
