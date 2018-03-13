using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
	public float speed;
	public BoxCollider2D boundsCollider;

	public GameObject target;
	private GameObject player, enemy;
	private Vector3 targetPos;
	private Vector2 minBounds, maxBounds;
	private float halfHeight, halfWidth, offset;

	void Start () {
		SetPlayerAsTarget ();

		minBounds = boundsCollider.bounds.min;
		maxBounds = boundsCollider.bounds.max;
		offset = 2.5f;

		halfHeight = GetComponent<Camera> ().orthographicSize;
		halfWidth = halfHeight * Screen.width / Screen.height;
	}

	public void SetPlayerAsTarget(){
		target = GameObject.FindGameObjectWithTag ("Player");
	}

	void Update () {
		if (target) {
			targetPos = new Vector3 (target.transform.position.x, target.transform.position.y, transform.position.z);

			transform.position = Vector3.Lerp (transform.position, targetPos, speed * Time.deltaTime);

			float clampedX = Mathf.Clamp (transform.position.x, minBounds.x + halfWidth - offset, maxBounds.x - halfWidth + offset);
			float clampedY = Mathf.Clamp (transform.position.y, minBounds.y + halfHeight - offset, maxBounds.y - halfHeight + offset);

			transform.position = new Vector3 (clampedX, clampedY, transform.position.z);
		}
	}
}
