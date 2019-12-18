using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class SetAnimatorState : MonoBehaviour
{
    [SerializeField]
    private string booleanName;

    [SerializeField]
    private bool booleanValue;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Animator>().SetBool(booleanName, booleanValue);
    }

}
