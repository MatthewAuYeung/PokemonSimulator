using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Move", menuName = "Pokemon/Create new move")]

public class MoveBase : ScriptableObject
{
    [SerializeField]
    private string name;

    [TextArea]
    [SerializeField]
    private string description;

    [SerializeField]
    private PokemonType type;
    [SerializeField]
    private int power;
    [SerializeField]
    private int accuracy;
    [SerializeField]
    private int pp;
    [SerializeField]
    AttackType attackType;
    [SerializeField]
    bool recoil = false;
    [SerializeField]
    private int denominator;

    public string Name { get { return name; } }
    public string Description { get { return description; } }
    public PokemonType Type { get { return type; } }
    public int Power { get { return power; } }
    public int Accuracy { get { return accuracy; } }
    public int PP { get { return pp; } }
    public AttackType AtkType { get { return attackType; } }
    public bool Recoil { get { return recoil; } }
    public int Denominator { get { return denominator; } }
}

public enum AttackType
{
    None,
    Physical,
    Special,
    StatChange
}