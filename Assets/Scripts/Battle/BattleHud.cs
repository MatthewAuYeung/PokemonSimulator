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

    public void SetData(Pokemon pokemon)
    {
        nameText.text = pokemon.Base.Name;
        levelText.text = "Lv " + pokemon.level;
        healthBar.SetHP((float)pokemon.hp / pokemon.MaxHP);
    }
}
