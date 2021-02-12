﻿using System.Collections;
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

    public bool TakeDamage(Move move, Pokemon attacker)
    {
        float modifiers = Random.Range(0.85f, 1.0f);
        float a = (attacker.level * 0.4f + 2);
        float d = (a * move.Base.Power * ((float)attacker.Attack / Defence) * 0.02f) + 2;
        int dmg = Mathf.FloorToInt(d * modifiers);

        hp -= dmg;

        if (hp <= 0)
        {
            hp = 0;
            return true;
        }
        else
            return false;
    }

    public Move GetRandomMove()
    {
        int r = Random.Range(0, Moves.Count);
        return Moves[r];
    }
}
