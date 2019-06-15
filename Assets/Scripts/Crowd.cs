using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crowd : MonoBehaviour
{
    Animator anim;
    float delay;
    bool isCoroutineExecuting;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        anim.enabled = false;
        delay = Random.Range(.1f, .7f);
        StartCoroutine(ExecuteAfterTime(delay));
    }

    IEnumerator ExecuteAfterTime(float delay)
    {
        if (isCoroutineExecuting)
            yield break;

        isCoroutineExecuting = true;

        yield return new WaitForSeconds(delay);

        // Code to execute after the delay
        gameObject.SetActive(true);
        anim.enabled = true;
        isCoroutineExecuting = false;
    }
}
