using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuAnimation : MonoBehaviour
{

    public Animator Animator;

    public void Transition()
    {
        Animator.SetTrigger("StartSettings");
    }
}
