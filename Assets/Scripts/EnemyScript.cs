using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour {

	public Transform castPoint;//bepaald in de inspector waar de raycasting begint

	public Transform player;//bepaald ook in de inspector waar de player zich bevindt

	public float agroRange;//bepaald de grote van de raycast

	public float moveSpeed;//bepaald de snelheid van de enemey

	Rigidbody2D rb2d;//rigidbody component

	private bool isSearching;//checkt of die niet aan het zoeken is

	bool isFacingLeft = false;//kijkt of we naar links kijken
	private bool isAgro = false;//kijkt of we de speler hebben gezien

	//zorgt dat we starten met de rigidbody
	void Start () {
		rb2d = GetComponent<Rigidbody2D> ();

	}
	
	// Update is called once per frame
	void Update () {
		/*//afstand naar speler
		 * zonder raycast un_comment dit en comment de andere functie 
		 
		float distToPlayer = Vector2.Distance(transform.position, player.position);

		if (distToPlayer < agroRange) {

			ChasePlayer ();

		} else {
			StopChasingPlayer ();
		}
		*/
		//pathfinding met raycast
		if (CanSeePlayer (agroRange)) 
		{
			isAgro = true;
			} else {
			if (isAgro == true) {
				if (!isSearching) {
					isSearching = true;
					Invoke ("StopChasingPlayer", 3);
				}
			}
		}

		if (isAgro) {
			ChasePlayer ();
		}
	}

	//checkt of we speler hwbbwn gezien
	bool CanSeePlayer(float distance)
	{
		bool val = false;
		float castDist = distance;

		if (isFacingLeft) 
		{
			castDist = -distance;
		}

		Vector2 endPos = castPoint.position + Vector3.right * castDist;
		RaycastHit2D hit = Physics2D.Linecast(castPoint.position, endPos, 1 << LayerMask.NameToLayer("Action"));

		if (hit.collider != null) {
			if (hit.collider.gameObject.CompareTag ("Player")) {
				val = true;
			} else {
				val = false;
			} 

			Debug.DrawLine (castPoint.position, hit.point, Color.red);

		} else {
			Debug.DrawLine (castPoint.position, endPos, Color.red);
		}

		return val;
	}

	//hier checkt hij of wel achter de speler aanzit
	void ChasePlayer(){

		if (transform.position.x < player.position.x) {
			rb2d.velocity = new Vector2 (moveSpeed, 0);
			transform.localScale = new Vector2 (1, 1);
			isFacingLeft = false;

		} else if (transform.position.x > player.position.x){
			transform.localScale = new Vector2 (-1, 1);
			rb2d.velocity = new Vector2 (-moveSpeed, 0);
			isFacingLeft = true;

		}
	}
	//hier checkt hij of hij niet meer achter de speler aanzit
	void StopChasingPlayer(){
		isSearching = false;
		isAgro = false;
		rb2d.velocity = new Vector2(0,0);
	}

}
