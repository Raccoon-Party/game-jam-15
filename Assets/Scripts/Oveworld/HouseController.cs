using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HouseController : MonoBehaviour, Interactables
{
    public void Interact()
    {
        SceneManager.LoadScene("House");
    }
}
