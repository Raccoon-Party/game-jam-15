using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NpcBehaviour : MonoBehaviour, Interactables
{
    [SerializeField] Dialog dialog;

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
