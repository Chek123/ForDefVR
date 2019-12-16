using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Spusti nahodnu zabavnu animaciu na vojakovi
 * dovody: 
 *      Desynchronizacia Idle animacie
 *      Lepśi pocit z vojakov - su zivsi
 */
[RequireComponent(typeof(Animator))]
public class RandomFancyAnimationSwitch : MonoBehaviour
{

    [SerializeField]
    private List<string> fancyAnimationTriggers;

    [SerializeField]
    private float chance = 10;

    [SerializeField]
    private float delayAfterAnimation = 20;

    [SerializeField]
    private float delayWithoutAnimation = 2;

    private Animator anim;
    private bool isRunning = false;

    // Start is called before the first frame update
    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void ResolveAnimation()
    {
        if (!enabled)
        {
            isRunning = false;
            return;
        }
        Debug.Log("ResolveAnimation");

        if (Random.Range(0, 100) < chance)
        {
            Debug.Log("StartFancyAnimation, delay " + delayAfterAnimation + " seconds");

            var state = Random.Range(0, fancyAnimationTriggers.Count);
            anim.SetTrigger(fancyAnimationTriggers[state]);

            Invoke("ResolveAnimation", delayAfterAnimation);

        }
        else
        {
            Invoke("ResolveAnimation", delayWithoutAnimation);
        }

    }

    void OnEnable()
    {
        if (!isRunning)
        {
            ResolveAnimation();
            isRunning = true;
        }
    }
}
