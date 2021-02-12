using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleDialogBox : MonoBehaviour
{
    [SerializeField]
    private int charPerSec = 30;
    [SerializeField]
    private Text dialogText;
    [SerializeField]
    private Color highlight;

    [SerializeField]
    private GameObject actionSelector;
    [SerializeField]
    private GameObject moveSelector;
    [SerializeField]
    private GameObject moveInfo;

    [SerializeField]
    private List<Text> actionTexts;
    [SerializeField]
    private List<Text> moveTexts;
    [SerializeField]
    private Text ppText;
    [SerializeField]
    private Text TypeText;

    private float offset;

    private void Start()
    {
        offset = 1.0f / charPerSec;
    }
    public void SetDialog(string dialog)
    {
        dialogText.text = dialog;
    }

    public IEnumerator TypeDialog(string dialog)
    {
        dialogText.text = "";
        foreach (var letter in dialog.ToCharArray())
        {
            dialogText.text += letter;
            yield return new WaitForSeconds(offset);
        }
        yield return new WaitForSeconds(1.0f);
    }

    public void EnableDialogText(bool state)
    {
        dialogText.enabled = state;
    }

    public void EnableActionSelector(bool state)
    {
        actionSelector.SetActive(state);
    }

    public void EnableMoveSelector(bool state)
    {
        moveSelector.SetActive(state);
        moveInfo.SetActive(state);
    }

    public void UpdateActionSelection(int actionIndex)
    {
        for (int i = 0; i < actionTexts.Count; ++i)
        {
            if (i == actionIndex)
                actionTexts[i].color = highlight;
            else
                actionTexts[i].color = Color.black;
        }
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
