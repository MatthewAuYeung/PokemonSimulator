using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BattleUnit : MonoBehaviour
{
    [SerializeField] PokemonBase _base;
    [SerializeField] int level;
    [SerializeField] bool isPlayerUnit;

    public Pokemon Pokemon;

    private Image image;
    private Vector3 originalPos;
    private Color originalColor;

    private void Awake()
    {
        image = GetComponent<Image>();
        originalPos = image.transform.localPosition;
        originalColor = image.color;
    }

    public void Setup()
    {
        Pokemon = new Pokemon(_base, level);
        if (isPlayerUnit)
            image.sprite = Pokemon.Base.BackSprite;
        else
            image.sprite = Pokemon.Base.FrontSprite;

        EnterAnimation();
    }

    public void Setup(PokemonBase selection)
    {
        Pokemon = new Pokemon(selection, level);
        if (isPlayerUnit)
            image.sprite = Pokemon.Base.BackSprite;
        else
            image.sprite = Pokemon.Base.FrontSprite;

        EnterAnimation();
    }

    public void EnterAnimation()
    {
        if (isPlayerUnit)
            image.transform.localPosition = new Vector3(-505.0f, originalPos.y);
        else
            image.transform.localPosition = new Vector3(550.0f, originalPos.y);

        image.transform.DOLocalMoveX(originalPos.x, 1.0f);
    }

    public void AttackAnimation()
    {
        var sequence = DOTween.Sequence();
        if (isPlayerUnit)
            sequence.Append(image.transform.DOLocalMoveX(originalPos.x + 50.0f, 0.25f));
        else
            sequence.Append(image.transform.DOLocalMoveX(originalPos.x - 50.0f, 0.25f));

        sequence.Append(image.transform.DOLocalMoveX(originalPos.x, 0.25f));
    }

    public void HitAnimation()
    {
        var sequence = DOTween.Sequence();
        sequence.Append(image.DOColor(Color.gray, 0.05f));
        sequence.Append(image.DOColor(originalColor, 0.05f));
        sequence.Append(image.DOColor(Color.gray, 0.05f));
        sequence.Append(image.DOColor(originalColor, 0.05f));
        sequence.Append(image.DOColor(Color.gray, 0.1f));
        sequence.Append(image.DOColor(originalColor, 0.1f));
    }

    public void FaintAnimation()
    {
        var sequence = DOTween.Sequence();
        sequence.Append(image.transform.DOLocalMoveY(originalPos.y - 150.0f, 0.5f));
        sequence.Join(image.DOFade(0.0f, 0.5f));
    }
}
