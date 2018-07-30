using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour {

    private Animator animator;

    public GameObject OpenPanel = null;

    private bool isInsideTrigger = false;
	bool doorOpend=false;

	// Use this for initialization
	void Start () {
        animator = transform.Find("Door").GetComponent<Animator>();
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            isInsideTrigger = true;
            OpenPanel.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            isInsideTrigger = false;
            animator.SetBool("open", false);
            OpenPanel.SetActive(false);
			if (doorOpend == true)
			{
				StartCoroutine (DoorCloseSound());
				doorOpend = false;
			}

        }
    }

    private bool IsOpenPanelActive
    {
        get
        {
            return OpenPanel.activeInHierarchy;
        }
    }

	IEnumerator DoorCloseSound()
	{
		yield return new WaitForSeconds(1.5f);
		FindObjectOfType<SoundManager>().Play("DoorClosed");
	}

    // Update is called once per frame
    void Update () 
	{

        if(IsOpenPanelActive && isInsideTrigger)
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                OpenPanel.SetActive(false);
                animator.SetBool("open", true); 
				FindObjectOfType<SoundManager>().Play("DoorOpen");
				doorOpend = true;
            }
        }
	}
}
