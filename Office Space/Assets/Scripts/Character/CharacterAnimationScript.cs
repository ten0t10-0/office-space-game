using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimationScript : MonoBehaviour
{
    public List<string> IdlingActionBoolNames;
    public List<string> IdlingActionAnimationNames;
    public int MinSecondsBetweenIdleAction = 3;
    public int MaxSecondsBetweenIdleAction = 10;

    private float intervalIdleAction;
    private float tIdleAction;
    private float animLengthCurrentIdleAction;
    private int indexCurrentIdleAction;
    private bool isIdleAction;

    [HideInInspector]
    public Animator Animator;
    private RuntimeAnimatorController ac;

    private void Awake()
    {
        Animator = GetComponent<Animator>();
        ac = Animator.runtimeAnimatorController;

        indexCurrentIdleAction = -1;
        isIdleAction = false;
    }

    private void Start()
    {
        RandomizeNextIdleActTime();
    }

    private void Update()
    {
        if (Animator.GetBool("IsIdling"))
        {
            if (!isIdleAction)
            {
                if (Time.time >= tIdleAction + intervalIdleAction)
                {
                    indexCurrentIdleAction = Random.Range(0, IdlingActionBoolNames.Count);

                    for (int i = 0; i < ac.animationClips.Length; i++)
                    {
                        if (ac.animationClips[i].name == IdlingActionAnimationNames[indexCurrentIdleAction])
                        {
                            animLengthCurrentIdleAction = ac.animationClips[i].length;
                            break;
                        }
                    }

                    isIdleAction = true;
                    Animator.SetBool(IdlingActionBoolNames[indexCurrentIdleAction], true);
                    tIdleAction = Time.time;
                }
            }
            else
            {
                if (Time.time >= tIdleAction + animLengthCurrentIdleAction)
                {
                    StopIdleAct();
                }
            }
        }
        else
        {
            StopIdleAct();
        }
    }

    public void StopIdleAct()
    {
        if (indexCurrentIdleAction != -1)
        {
            Animator.SetBool(IdlingActionBoolNames[indexCurrentIdleAction], false);

            RandomizeNextIdleActTime();

            indexCurrentIdleAction = -1;
        }
        else
        {
            tIdleAction = Time.time;
        }

        isIdleAction = false;
    }

    private void RandomizeNextIdleActTime()
    {
        intervalIdleAction = Random.Range(MinSecondsBetweenIdleAction, MaxSecondsBetweenIdleAction + 1);

        tIdleAction = Time.time;
    }

    public void Interact()
    {
        if (Animator.GetBool("IsIdling") && !Animator.GetCurrentAnimatorStateInfo(0).IsName("Interact01"))
        {
            StopIdleAct();

            RandomizeNextIdleActTime();

            Animator.SetTrigger("Interact 01");
        }
    }

    public void Greet()
    {
        if (Animator.GetBool("IsIdling") && !Animator.GetCurrentAnimatorStateInfo(0).IsName("Greet01"))
        {
            StopIdleAct();

            RandomizeNextIdleActTime();

            Animator.SetTrigger("Greet 01");
        }
    }

    public void Idle()
    {
        Animator.SetBool("IsWalking", false);
        Animator.SetBool("IsRunning", false);
        Animator.SetBool("IsIdling", true);
    }

    public void MoveWalk()
    {
        Animator.SetBool("IsWalking", true);
        Animator.SetBool("IsRunning", false);

        Animator.SetBool("IsIdling", false);
    }

    public void MoveRun()
    {
        Animator.SetBool("IsWalking", false);
        Animator.SetBool("IsRunning", true);

        Animator.SetBool("IsIdling", false);
    }

    public bool IsInteractAnim
    {
        get { return Animator.GetCurrentAnimatorStateInfo(0).IsName("Interact01"); }
    }
}
