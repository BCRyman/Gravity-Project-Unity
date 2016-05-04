using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Player : MonoBehaviour {

	[SerializeField]
	bool grounded = false;

	Rigidbody thisRigidbody;

	[SerializeField]
	float moveSpeed = 5, rotSpeed = 5, jumpForce;

	[SerializeField]
	Transform currentlyOnPlanet, lastPlanetOn;
	[SerializeField]
	List<Transform> localPlanetList;
	void Start()
	{
		localPlanetList = new List<Transform> ();
		thisRigidbody = this.GetComponent<Rigidbody> ();
		thisRigidbody.centerOfMass = new Vector3 (0, -0.9f, 0);
	}

	public void Update()
	{
		ApplyForceFromPlanet ();
		if (Input.GetKey (KeyCode.W)) {
			Move (Vector3.forward);
		}
		if (Input.GetKey (KeyCode.A)) {
			Rotate (-rotSpeed);
		}
		if (Input.GetKey (KeyCode.D)) {
			Rotate (rotSpeed);
		}
		if (Input.GetKey (KeyCode.S)) {
			Move (Vector3.back);
		}
		if (Input.GetKey (KeyCode.Space)) {
			Jump ();
		}
	}



	void Rotate(float rSpeed)
	{
		this.transform.Rotate (Vector3.up * Time.deltaTime * rSpeed);	
	}

	void Move(Vector3 dir)
	{
		this.transform.Translate (dir * Time.deltaTime * moveSpeed, Space.Self);
	}
	void Jump()
	{
		if(currentlyOnPlanet)
			thisRigidbody.AddForce ((transform.position - currentlyOnPlanet.position).normalized * jumpForce, ForceMode.Impulse);
	}
	void ApplyForceFromPlanet()
	{
		for (int i = 0; i < localPlanetList.Count; i++) {
			if (!grounded)
				thisRigidbody.AddForce ((localPlanetList[i].position - transform.position).normalized * localPlanetList[i].GetComponent<Planet> ().gravityValue);
			else
				thisRigidbody.AddForce ((localPlanetList[i].position - transform.position).normalized * (localPlanetList[i].GetComponent<Planet> ().gravityValue / 10));
		}
		if (localPlanetList.Count == 0) {
			thisRigidbody.AddForce ((lastPlanetOn.position - transform.position).normalized * (lastPlanetOn.GetComponent<Planet> ().gravityValue / 10));
		}
	}




	void OnCollisionEnter(Collision col)
	{
		if (col.collider.tag == "Planet") {
			grounded = true;
			thisRigidbody.velocity = Vector3.zero;
			thisRigidbody.angularVelocity = Vector3.zero;
			thisRigidbody.angularDrag = 0f;
			thisRigidbody.drag = 0f;
			currentlyOnPlanet = col.transform;
			lastPlanetOn = col.transform;
		}
	}

	void OnCollisionExit(Collision col)
	{
		if (col.collider.tag == "Planet") {
			grounded = false;
			currentlyOnPlanet = null;
		}
	}

	void OnTriggerEnter(Collider col)
	{
		Debug.Log (col.name);
		localPlanetList.Add (col.transform);
	}

	void OnTriggerExit(Collider col)
	{
		localPlanetList.Remove (col.transform);
	}
}
