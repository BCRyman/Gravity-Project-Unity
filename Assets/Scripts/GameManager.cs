using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {


	public Player player;
	public static GameManager INSTANCE
	{
		get{
			return instance;	
		}
	}
	private static GameManager instance;
	// Use this for initialization
	void Start () {
		instance = this;
	}

	[SerializeField]
	List<Planet> planetList;

	// Update is called once per frame
	void Update () {
		//player.PlayerUpdate (planetList[0].transform);
	}


}
