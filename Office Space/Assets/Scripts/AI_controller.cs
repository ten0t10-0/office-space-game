using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI_controller : MonoBehaviour 
{

	[SerializeField] private NavMeshAgent m_NavmeshAgent;
	[SerializeField] private GameObject m_Target;


	void Start () 
	{
		
	}
	

	void Update () 
	{
		m_NavmeshAgent.SetDestination (m_Target.transform.position);


	}
}
