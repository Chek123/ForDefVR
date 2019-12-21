using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Spusti nahodnu zabavnu animaciu na vojakovi
 * dovody: 
 *      Desynchronizacia Idle animacie
 *      Lepśi pocit z vojakov - su zivsi
 */
public class RandomFancyAnimationSwitch : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Kazdy vojak ma v sebe model na ktorom je Animator")]
    public Animator soldierAnimator;

    [SerializeField]
    [Tooltip("Nazvy trigger parametrov ktore v VojakController animatore spustia fancy animaciu")]
    private List<string> fancyAnimationTriggers;

    [SerializeField]
    [Tooltip("Pravdepodobnost s ktorou sa fancy animacia vytvori (x zo 100)")]
    private float chance = 10;

    [SerializeField]
    [Tooltip("Zdrzanie v sekundach po prehrati animacie (aby v kuse nejaky vojak netancoval)")]
    private float delayAfterAnimation = 20;

    [SerializeField]
    [Tooltip("Prestavka medzi pokusom o zaciatie animacie (podla parametru chance sa spusti alebo nespusti animacia)")]
    private float delayWithoutAnimation = 2;


    private bool isRunning = false;


    /**
     * Vykonanie jedneho cyklu spustania animacie, hodi sa kockou podla toho sa spusti nahodna fancy animacia alebo nespusti
     */ 
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

    //Inicializacia
    void OnEnable()
    {
        if (!isRunning)
        {
            Invoke("ResolveAnimation",((float)Random.Range(0, delayWithoutAnimation*1000))/1000);
            isRunning = true;
        }
    }
}
