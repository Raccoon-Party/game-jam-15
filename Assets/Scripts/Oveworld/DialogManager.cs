using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    [SerializeField] GameObject dialogBox;

    [SerializeField] TextMeshProUGUI dialogText;

    [SerializeField] TextMeshProUGUI speakerNameText;

    [SerializeField] Image speakerSpriteImage;

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

    public bool HandleUpdate(Dialog dialog)
    {
        if (!isTyping)
        {
            if (currentLine == 0)
            {
                StartCoroutine(ShowDialog(dialog));
                currentLine++;
                return false;
            }
            else if (currentLine < dialog.Lines.Count)
            {
                StartCoroutine(TypeDialog(dialog.Lines[currentLine]));
                currentLine++;
                return false;
            }
            else
            {
                dialogBox.SetActive(false);
                currentLine = 0;
                OnHideDialog?.Invoke();
                return true;
            }
        }
        return false;
    }

    public IEnumerator ShowDialog(Dialog dialog)
    {
        yield return new WaitForEndOfFrame();

        OnShowDialog?.Invoke();

        dialogBox.SetActive(true);

        speakerNameText.text = dialog.SpeakerName;
        speakerSpriteImage.sprite = dialog.SpeakerSprite;

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