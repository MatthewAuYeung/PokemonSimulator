using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleDialogBox : DialogText
{
    [SerializeField]
    private GameObject actionSelector;
    [SerializeField]
    private GameObject moveSelector;
    [SerializeField]
    private GameObject moveInfo;

    [SerializeField]
    private List<Text> moveTexts;
    [SerializeField]
    private Text ppText;
    [SerializeField]
    private Text TypeText;

    public void EnableActionSelector(bool state)
    {
        actionSelector.SetActive(state);
    }

    public void EnableMoveSelector(bool state)
    {
        moveSelector.SetActive(state);
        moveInfo.SetActive(state);
    }

    public void UpdateMoveSelection(int moveIndex, Move move)
    {
        for (int i = 0; i < moveTexts.Count; ++i)
        {
            if (i == moveIndex)
                moveTexts[i].color = highlight;
            else
                moveTexts[i].color = Color.black;
        }

        ppText.text = $"PP {move.PP}/{move.Base.PP}";
        TypeText.text = move.Base.Type.ToString();
    }

    public void SetMoveNames(List<Move> moves)
    {
        for (int i = 0; i < moveTexts.Count; ++i)
        {
            if (i < moves.Count)
                moveTexts[i].text = moves[i].Base.Name;
            else
                moveTexts[i].text = "-";
        }
    }
}
