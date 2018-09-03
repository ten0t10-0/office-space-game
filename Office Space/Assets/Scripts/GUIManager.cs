﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIManager : MonoBehaviour
{
    public string goMonitor, goUIScreen, goCompCanvas, goApps;
    public string goOrdersPanel;

	public string HudCanvas;

    [HideInInspector]
    public OrderUI OrdersPanelScript;

	[HideInInspector]
	public HudMain hudScript;

	[HideInInspector]
	public Notifications note ;

    private void Awake()
    {
		OrdersPanelScript = GameObject.Find(goMonitor).transform.Find(goUIScreen).Find(goCompCanvas).Find(goApps).Find(goOrdersPanel).gameObject.GetComponent<OrderUI>();
		hudScript = GameObject.Find("HudCanvas").GetComponent<HudMain>();
		note = GameObject.Find ("HudCanvas").transform.Find ("Phone").gameObject.GetComponent<Notifications> ();

    }
}
