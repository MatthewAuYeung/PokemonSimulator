using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pokemon
{
    public PokemonBase Base;
    public int level;
    public int hp;

    public List<Move> Moves { get; set; }

    public Pokemon(PokemonBase pBase, int pLevel)
    {
        Base = pBase;
        level = pLevel;
        hp = MaxHP;

        // Add moves
        Moves = new List<Move>();
        foreach(var move in Base.LearnableMoves)
        {
            if (move.Level <= level)
                Moves.Add(new Move(move.Base));

            if (Moves.Count >= 4)
                break;
        }
    }

    public int MaxHP { get { return Mathf.FloorToInt((2 * Base.MaxHP * level / 100.0f) + level + 10); } }
    public int Attack { get { return CalculateStats(Base.Attack, level, 5); } }
    public int Defence { get { return CalculateStats(Base.Defence, level, 5); } }
    public int SpAtk { get { return CalculateStats(Base.SpAtk, level, 5); } }
    public int SpDef { get { return CalculateStats(Base.SpDef, level, 5); } }
    public int Speed { get { return CalculateStats(Base.Speed, level, 5); } }

    private int CalculateStats(int stat, int lv, int offset)
    {
        return (Mathf.FloorToInt(2 * stat * lv / 100.0f + offset));
    }

    public DamageInfo TakeDamage(Move move, Pokemon attacker)
    {
        AudioManager.instance.PlaySound("Hit");

        float atk = 1.0f;
        float def = 1.0f;
        if(move.Base.AtkType == AttackType.Physical)
        {
            atk = attacker.Attack;
            def = Defence;
        }
        else if(move.Base.AtkType == AttackType.Special)
        {
            atk = attacker.SpAtk;
            def = SpDef;
        }

        float crit = 1.0f;
        if (Random.value * 100.0f <= 4.167f)
        {
            crit = 2.0f;
        }

        float type = TypeChart.GetEffectiveness(move.Base.Type, this.Base.Type1) * TypeChart.GetEffectiveness(move.Base.Type, this.Base.Type2);

        var dmgInfo = new DamageInfo()
        {
            Critical = crit,
            TypeEffectiveness = type,
            Faint = false
        };

        float modifiers = Random.Range(0.85f, 1.0f) * type * crit;
        float a = (attacker.level * 0.4f + 2);
        float d = (a * move.Base.Power * (atk / def) * 0.02f) + 2;
        if (move.Base.Power == 0)
            d = 0.0f;
        int dmg = Mathf.FloorToInt(d * modifiers);

        hp -= dmg;

        if(move.Base.Recoil)
        {
            dmgInfo.RecoilDmg = Mathf.FloorToInt(dmg * (1.0f / move.Base.Denominator));
        }
        else
        {
            dmgInfo.RecoilDmg = 0;
        }

        if (hp <= 0)
        {
            hp = 0;
            dmgInfo.Faint = true;
        }

        return dmgInfo;
    }

    public Move GetRandomMove()
    {
        int r = Random.Range(0, Moves.Count);
        return Moves[r];
    }

    public class DamageInfo
    {
        public bool Faint { get; set; }
        public float Critical { get; set; }
        public float TypeEffectiveness { get; set; }
        public int RecoilDmg { get; set; }
    }
}
