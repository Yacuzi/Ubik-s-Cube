using UnityEngine;
using System.Collections;

public class UnCheckIsKinematicOnTriggerEnter : MonoBehaviour {

	[SerializeField]
	private Rigidbody _targetRigidbody;

	public void OnDrawGizmos()
	{
		if (_targetRigidbody != null)
			Debug.DrawLine(this.transform.position, _targetRigidbody.transform.position, Color.yellow);
	}

	public void OnTriggerEnter(Collider col)
	{
		if (_targetRigidbody != null)
		{
			_targetRigidbody.isKinematic = false;
			_targetRigidbody.WakeUp();
		}
	}
}