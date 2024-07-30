using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComingSoonBehavior : MonoBehaviour
{
    bool canDisable = false;
    private void Start()
    {

    }
    void Update()
    {
        if (canDisable)
        {
            if (Input.anyKeyDown)
            {
                gameObject.SetActive(false);
            }
        }
    }

    private void OnEnable()
    {
        canDisable = false;
        StartCoroutine(Wait());
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(1);
        canDisable = true;
    }
}
