using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum BattleState { Start, PlayerAction, PlayerMove, EnemyMove, Battle }
public enum PlayerActionState { Fight, Run }

public class BattleSystem : MonoBehaviour
{
    [SerializeField]
    private BattleUnit playerUnit;
    [SerializeField]
    private BattleHud playerHud;
    [SerializeField]
    private EnemyTrainer enemy;
    //[SerializeField]
    //private BattleUnit enemyUnit;
    [SerializeField]
    private BattleHud enemyHud;
    [SerializeField]
    private BattleDialogBox dialogBox;

    private BattleState battleState;
    private int currentActionIndex;
    private int currentMoveIndex;
    private bool isPlayerfirst;
    private int randomint;

    private void Start()
    {
        StartCoroutine(SetupBattle());
    }

    public IEnumerator SetupBattle()
    {
        if(Player.instance.pokemonBase == null || Enemy.instance.pokemonBase == null)
        {
            playerUnit.Setup();
            enemy.enemyUnit.Setup();
        }
        else
        {
            playerUnit.Setup(Player.instance.pokemonBase);
            enemy.enemyUnit.Setup(Enemy.instance.pokemonBase);
        }
        playerHud.SetData(playerUnit.Pokemon);
        enemyHud.SetData(enemy.enemyUnit.Pokemon);

        dialogBox.SetMoveNames(playerUnit.Pokemon.Moves);

        yield return dialogBox.TypeDialog($"??? sents out {enemy.enemyUnit.Pokemon.Base.Name}.");
        PlayerAction();
    }

    private void CompareSpeed()
    {
        if(playerUnit.Pokemon.Speed > enemy.enemyUnit.Pokemon.Speed)
        {
            isPlayerfirst = true;
        }
        else if(playerUnit.Pokemon.Speed == enemy.enemyUnit.Pokemon.Speed)
        {
            randomint = UnityEngine.Random.Range(0, 100);
            if(randomint > 50)
            {
                isPlayerfirst = true;
            }
            else
            {
                isPlayerfirst = false;
            }
        }
        else
        {
            isPlayerfirst = false;
        }
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

        playerUnit.AttackAnimation();
        yield return new WaitForSeconds(1.0f);

        enemy.enemyUnit.HitAnimation();

        var dmgInfo = enemy.enemyUnit.Pokemon.TakeDamage(move, playerUnit.Pokemon);
        yield return enemyHud.UpdateHp();

        yield return ShowDamageInfo(dmgInfo, playerUnit.Pokemon, playerHud);

        if (dmgInfo.Faint)
        {
            yield return dialogBox.TypeDialog($"{enemy.enemyUnit.Pokemon.Base.Name} fainted.");
            enemy.enemyUnit.FaintAnimation();
            Player.instance.win = true;
            AudioManager.instance.StopSound("BattleTheme");
            AudioManager.instance.PlaySound("Win");
            yield return new WaitForSeconds(1.0f);
            SceneManager.LoadScene("EndScene");
        }
        else
        {
            if(isPlayerfirst)
            {
                StartCoroutine(PerformEnemyMove());
            }
            else
            {
                PlayerAction();
            }
        }
    }

    private IEnumerator PerformEnemyMove()
    {
        battleState = BattleState.EnemyMove;

        //var move = enemy.enemyUnit.Pokemon.GetRandomMove();
        var move = enemy.EnemyAtk(playerUnit.Pokemon);
        yield return dialogBox.TypeDialog($"The opposing {enemy.enemyUnit.Pokemon.Base.Name} used {move.Base.Name}!");

        enemy.enemyUnit.AttackAnimation();
        yield return new WaitForSeconds(1.0f);

        playerUnit.HitAnimation();

        var dmgInfo = playerUnit.Pokemon.TakeDamage(move, enemy.enemyUnit.Pokemon);
        yield return playerHud.UpdateHp();

        yield return ShowDamageInfo(dmgInfo, enemy.enemyUnit.Pokemon, enemyHud);

        if (dmgInfo.Faint)
        {
            yield return dialogBox.TypeDialog($"{playerUnit.Pokemon.Base.Name} fainted.");
            playerUnit.FaintAnimation();
            Player.instance.win = false;
            AudioManager.instance.StopSound("BattleTheme");
            AudioManager.instance.PlaySound("MainTheme");
            yield return new WaitForSeconds(1.0f);
            SceneManager.LoadScene("EndScene");
        }
        else
        {
            if(isPlayerfirst)
            {
                PlayerAction();
            }
            else
            {
                StartCoroutine(PerformPlayerMove());
            }
        }
    }

    private IEnumerator ShowDamageInfo(Pokemon.DamageInfo info, Pokemon attacker, BattleHud hud)
    {
        if (info.TypeEffectiveness > 1.0f)
            yield return dialogBox.TypeDialog("It's super effective!");
        else if (info.TypeEffectiveness < 1.0f)
            yield return dialogBox.TypeDialog("It's not very effective...");

        if (info.Critical > 1.0f)
            yield return dialogBox.TypeDialog("A critical hit!");

        if(info.RecoilDmg > 0.0f)
        {
            attacker.hp -= info.RecoilDmg;
            yield return hud.UpdateHp();
            yield return dialogBox.TypeDialog($"{attacker.Base.Name} is damaged by recoil!");
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
            AudioManager.instance.PlaySound("Button");

            if (currentActionIndex < 1)
                ++currentActionIndex;
        }
        else if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            AudioManager.instance.PlaySound("Button");

            if (currentActionIndex > 0)
                --currentActionIndex;
        }
        dialogBox.UpdateActionSelection(currentActionIndex);

        if(Input.GetKeyDown(KeyCode.Z))
        {
            AudioManager.instance.PlaySound("Button");

            switch ((PlayerActionState)currentActionIndex)
            {
                case PlayerActionState.Fight:
                    PlayerMove();
                    break;
                case PlayerActionState.Run:
                    AudioManager.instance.StopSound("BattleTheme");
                    AudioManager.instance.PlaySound("MainTheme");
                    SceneManager.LoadScene("Menu");
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
            AudioManager.instance.PlaySound("Button");

            if (currentMoveIndex < playerUnit.Pokemon.Moves.Count - 1)
                ++currentMoveIndex;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            AudioManager.instance.PlaySound("Button");

            if (currentMoveIndex > 0)
                --currentMoveIndex;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            AudioManager.instance.PlaySound("Button");

            if (currentMoveIndex < playerUnit.Pokemon.Moves.Count - 2)
                currentMoveIndex += 2;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            AudioManager.instance.PlaySound("Button");

            if (currentMoveIndex > 1)
                currentMoveIndex -= 2;
        }

        dialogBox.UpdateMoveSelection(currentMoveIndex, playerUnit.Pokemon.Moves[currentMoveIndex]);

        if(Input.GetKeyDown(KeyCode.Z))
        {
            AudioManager.instance.PlaySound("Button");

            dialogBox.EnableMoveSelector(false);
            dialogBox.EnableDialogText(true);
            CompareSpeed();
            if(isPlayerfirst)
            {
                StartCoroutine(PerformPlayerMove());
            }
            else
            {
                StartCoroutine(PerformEnemyMove());
            }
        }
    }
}
