using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NpcBehaviour : MonoBehaviour, Interactables
{
    [SerializeField] Dialog dialog;

    // [SerializeField] TMP_Text speakerName;

    // [SerializeField] string[] speaker;

    // [SerializeField] Image portraitImage;

    // [SerializeField] Sprite[] portrait;

    public void Interact()
    {
        DialogManager.Instance.HandleUpdate(dialog);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
