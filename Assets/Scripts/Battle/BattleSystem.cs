using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BattleState { Start, PlayerAction, PlayerMove, EnemyMove, Battle }
public enum PlayerActionState { Fight, Run }

public class BattleSystem : MonoBehaviour
{
    [SerializeField]
    private BattleUnit playerUnit;
    [SerializeField]
    private BattleHud playerHud;
    [SerializeField]
    private BattleUnit enemyUnit;
    [SerializeField]
    private BattleHud enemyHud;
    [SerializeField]
    private BattleDialogBox dialogBox;

    private BattleState battleState;
    private int currentActionIndex;
    private int currentMoveIndex;

    private void Start()
    {
        StartCoroutine(SetupBattle());
    }

    public IEnumerator SetupBattle()
    {
        playerUnit.Setup();
        enemyUnit.Setup();
        playerHud.SetData(playerUnit.Pokemon);
        enemyHud.SetData(enemyUnit.Pokemon);

        dialogBox.SetMoveNames(playerUnit.Pokemon.Moves);

        yield return dialogBox.TypeDialog($"??? sents out {enemyUnit.Pokemon.Base.Name}.");
        yield return new WaitForSeconds(1.0f);
        PlayerAction();
    }

    private void PlayerAction()
    {
        battleState = BattleState.PlayerAction;
        StartCoroutine(dialogBox.TypeDialog("Choose an action."));
        dialogBox.EnableActionSelector(true);
        dialogBox.EnableMoveSelector(false);
    }

    private void PlayerMove()
    {
        battleState = BattleState.PlayerMove;
        dialogBox.EnableActionSelector(false);
        dialogBox.EnableDialogText(false);
        dialogBox.EnableMoveSelector(true);
    }

    private IEnumerator PerformPlayerMove()
    {
        battleState = BattleState.Battle;
        var move = playerUnit.Pokemon.Moves[currentMoveIndex];
        yield return dialogBox.TypeDialog($"{playerUnit.Pokemon.Base.Name} used {move.Base.Name}.");

        yield return new WaitForSeconds(1.0f);

        bool isFainted = enemyUnit.Pokemon.TakeDamage(move, playerUnit.Pokemon);
        yield return enemyHud.UpdateHp();

        if (isFainted)
        {
            yield return dialogBox.TypeDialog($"{enemyUnit.Pokemon.Base.Name} fainted.");
        }
        else
        {
            StartCoroutine(PerformEnemyMove());
        }
    }

    private IEnumerator PerformEnemyMove()
    {
        battleState = BattleState.EnemyMove;

        var move = enemyUnit.Pokemon.GetRandomMove();
        yield return dialogBox.TypeDialog($"{enemyUnit.Pokemon.Base.Name} used {move.Base.Name}.");

        yield return new WaitForSeconds(1.0f);

        var isFainted = playerUnit.Pokemon.TakeDamage(move, enemyUnit.Pokemon);
        yield return playerHud.UpdateHp();
        if (isFainted)
        {
            yield return dialogBox.TypeDialog($"{playerUnit.Pokemon.Base.Name} fainted.");
        }
        else
        {
            PlayerAction();
        }
    }

    private void Update()
    {
        switch (battleState)
        {
            case BattleState.Start:
                break;
            case BattleState.PlayerAction:
                PlayerActionInput();
                break;
            case BattleState.PlayerMove:
                PlayerMoveInput();
                break;
            case BattleState.EnemyMove:
                break;
            case BattleState.Battle:
                break;
            default:
                break;
        }
    }

    private void PlayerActionInput()
    {
        if(Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (currentActionIndex < 1)
                ++currentActionIndex;
        }
        else if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (currentActionIndex > 0)
                --currentActionIndex;
        }
        dialogBox.UpdateActionSelection(currentActionIndex);

        if(Input.GetKeyDown(KeyCode.Z))
        {
            switch ((PlayerActionState)currentActionIndex)
            {
                case PlayerActionState.Fight:
                    PlayerMove();
                    break;
                case PlayerActionState.Run:
                    break;
                default:
                    break;
            }
        }
    }

    private void PlayerMoveInput()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (currentMoveIndex < playerUnit.Pokemon.Moves.Count - 1)
                ++currentMoveIndex;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (currentMoveIndex > 0)
                --currentMoveIndex;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (currentMoveIndex < playerUnit.Pokemon.Moves.Count - 2)
                currentMoveIndex += 2;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (currentMoveIndex > 1)
                currentMoveIndex -= 2;
        }

        dialogBox.UpdateMoveSelection(currentMoveIndex, playerUnit.Pokemon.Moves[currentMoveIndex]);

        if(Input.GetKeyDown(KeyCode.Z))
        {
            dialogBox.EnableMoveSelector(false);
            dialogBox.EnableDialogText(true);
            StartCoroutine(PerformPlayerMove());
        }
    }
}
