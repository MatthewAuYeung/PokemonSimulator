using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Icon : MonoBehaviour
{
    public PokemonBase pokemonBase;
    private Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    public void OnSelection()
    {
        image.color = Color.white;
    }

    public void OffSelection()
    {
        image.color = Color.black;
    }
}
