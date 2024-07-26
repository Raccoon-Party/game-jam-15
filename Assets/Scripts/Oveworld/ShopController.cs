using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShopController : MonoBehaviour, Interactables
{
    [SerializeField] int shopCounter;

    public void Interact()
    {
        SceneManager.LoadScene("Shop" + shopCounter);
    }
}
