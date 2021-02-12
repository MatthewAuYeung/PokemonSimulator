using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleHud : MonoBehaviour
{
    [SerializeField]
    private Text nameText;
    [SerializeField]
    private Text levelText;
    [SerializeField]
    private HealthBar healthBar;

    Pokemon _pokemon;

    public void SetData(Pokemon pokemon)
    {
        _pokemon = pokemon;
        nameText.text = pokemon.Base.Name;
        levelText.text = "Lv " + pokemon.level;
        healthBar.SetHP((float)pokemon.hp / pokemon.MaxHP);
    }

    public IEnumerator UpdateHp()
    {
        yield return healthBar.SetHpSmooth((float)_pokemon.hp / _pokemon.MaxHP);
    }
}
