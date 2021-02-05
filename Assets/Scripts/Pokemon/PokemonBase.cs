using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Pokemon", menuName ="Pokemon/Create new pokemon")]

public class PokemonBase : ScriptableObject
{
    [SerializeField]
    private string name;

    [TextArea]
    [SerializeField]
    private string description;

    [SerializeField]
    private Sprite frontSprite;
    [SerializeField]
    private Sprite backSprite;

    [SerializeField]
    private PokemonType type1;
    [SerializeField]
    private PokemonType type2;

    // Base Stats
    [SerializeField]
    private int maxHp;
    [SerializeField]
    private int attack;
    [SerializeField]
    private int defense;
    [SerializeField]
    private int spAtk;
    [SerializeField]
    private int spDef;
    [SerializeField]
    private int speed;
    //--------------------
    [SerializeField]
    private List<LearnableMove> learnableMoves;

    //public valuables
    public string Name { get { return name; } }
    public string Description { get { return description; } }
    public PokemonType Type1 { get { return type1; } }
    public PokemonType Type2 { get { return type2; } }
    public int MaxHP { get { return maxHp; } }
    public int Attack { get { return attack; } }
    public int Defence { get { return defense; } }
    public int SpAtk { get { return spAtk; } }
    public int SpDef { get { return spDef; } }
    public int Speed { get { return speed; } }
    public Sprite FrontSprite { get { return frontSprite; } }
    public Sprite BackSprite { get { return backSprite; } }
    public List<LearnableMove> LearnableMoves { get { return learnableMoves; } }
}

[System.Serializable]
public class LearnableMove
{
    [SerializeField]
    private MoveBase moveBase;
    [SerializeField]
    int level;

    public MoveBase Base { get { return moveBase; } }
    public int Level { get { return level; } }
}

public enum PokemonType
{
    None,
    Normal,
    Fire,
    Water,
    Grass,
    Electric,
    Ground,
    Rock,
    Fighting,
    Ice,
    Steel,
    Poision,
    Bug,
    Flying,
    Psychic,
    Dark,
    Ghost,
    Dragon,
    Fairy
}
