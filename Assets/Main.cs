using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Main : MonoBehaviour
{
	public GameObject Ball;
	// Use this for initialization
	void Start()
	{
		
	}

	// Update is called once per frame
	void Update()
	{
		
	}
	void OnTriggerEnter(Collider other)
	{
		Instantiate(Ball);
	}
}
