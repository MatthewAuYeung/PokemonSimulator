using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTrainer : MonoBehaviour
{
    public BattleUnit enemyUnit;
    
    public Move EnemyAtk(Pokemon playerPokemon)
    {
        PokemonType type1 = playerPokemon.Base.Type1;
        PokemonType type2 = playerPokemon.Base.Type2;

        List<MoveInfo> moveInfos = new List<MoveInfo>();

        foreach (var move in enemyUnit.Pokemon.Moves)
        {
            if(move.Base.AtkType != AttackType.StatChange)
            {
                MoveInfo info = new MoveInfo();
                info.move = move;
                info.typeEffectiveness = TypeChart.GetEffectiveness(move.Base.Type, type1) * TypeChart.GetEffectiveness(move.Base.Type, type2);
                moveInfos.Add(info);
            }
        }

        MoveInfo finalMove = new MoveInfo();
        finalMove = moveInfos[0];
        foreach (var moveInfo in moveInfos)
        {
            if(moveInfo.typeEffectiveness > finalMove.typeEffectiveness)
            {
                finalMove = moveInfo;
            }
            else if(moveInfo.typeEffectiveness == finalMove.typeEffectiveness)
            {
                if(moveInfo.move.Base.Power > finalMove.move.Base.Power)
                {
                    finalMove = moveInfo;
                }
            }
        }

        return finalMove.move;
    }
}

public class MoveInfo
{
    public Move move;
    public float typeEffectiveness;
}
