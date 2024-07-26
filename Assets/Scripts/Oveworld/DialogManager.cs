using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    [SerializeField] GameObject dialogBox;

    [SerializeField] Text dialogText;

    [SerializeField] int lettersPerSecond;

    public static DialogManager Instance { get; private set; }

    public event Action OnShowDialog;

    public event Action OnHideDialog;

    int currentLine = 0;

    bool isTyping;

    private void Awake()
    {
        Instance = this;
    }

    public void HandleUpdate(Dialog dialog)
    {
        if (!isTyping)
        {
            if (currentLine == 0)
            {
                StartCoroutine(ShowDialog(dialog));
                currentLine++;
            }
            else if (currentLine < dialog.Lines.Count)
            {
                StartCoroutine(TypeDialog(dialog.Lines[currentLine]));
                currentLine++;
            }
            else
            {
                dialogBox.SetActive(false);
                currentLine = 0;
                OnHideDialog?.Invoke();
            }
        }
    }

    public IEnumerator ShowDialog(Dialog dialog)
    {
        yield return new WaitForEndOfFrame();

        OnShowDialog?.Invoke();

        dialogBox.SetActive(true);
        StartCoroutine(TypeDialog(dialog.Lines[0]));
    }

    public IEnumerator TypeDialog(string line)
    {
        isTyping = true;
        dialogText.text = "";
        foreach (char letter in line.ToCharArray())
        {
            dialogText.text += letter;
            yield return new WaitForSeconds(1f / lettersPerSecond);
        }
        isTyping = false;
    }
}