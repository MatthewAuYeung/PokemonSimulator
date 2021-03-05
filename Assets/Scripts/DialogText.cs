using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogText : MonoBehaviour
{
    [SerializeField]
    protected List<Text> actionTexts;

    [SerializeField]
    protected int charPerSec = 30;
    [SerializeField]
    protected Text dialogText;
    [SerializeField]
    protected Color highlight;

    protected float offset;

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
}
