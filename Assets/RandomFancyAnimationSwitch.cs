using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Spusti nahodnu zabavnu animaciu na vojakovi
 * dovody: 
 *      Desynchronizacia Idle animacie
 *      Lepśi pocit z vojakov - su zivsi
 */
public class RandomFancyAnimationSwitch : MonoBehaviour
{
    [SerializeField]
    public Animator soldierAnimator;

    [SerializeField]
    private List<string> fancyAnimationTriggers;

    [SerializeField]
    private float chance = 10;

    [SerializeField]
    private float delayAfterAnimation = 20;

    [SerializeField]
    private float delayWithoutAnimation = 2;


    private bool isRunning = false;

    private void ResolveAnimation()
    {
        if (!enabled)
        {
            isRunning = false;
            return;
        }

        if (Random.Range(0, 100) < chance)
        {

            var state = Random.Range(0, fancyAnimationTriggers.Count);
            soldierAnimator.SetTrigger(fancyAnimationTriggers[state]);

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
            Invoke("ResolveAnimation",((float)Random.Range(0, delayWithoutAnimation*1000))/1000);
            isRunning = true;
        }
    }
}
