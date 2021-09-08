using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Candy : MonoBehaviour
{

	public ParticleSystem splatCandy;
	public SpriteRenderer Sprite;
	public AudioClip candyHitSfx;

	// Use this for initialization
	public Rigidbody2D rb;
	void Start()
	{
		rb = GetComponentInChildren<Rigidbody2D>();
		rb.isKinematic = true;
	}
	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag.Equals("Knife"))
		{
			//if (!other.gameObject.GetComponent<Knife> ().isHitted) {
			SoundManager.instance.PlaySingle(candyHitSfx);
			if (PlayerPrefs.GetInt("Vip", 0) == 1)
			{
				GameManager.Candy += 2;
            }
            else
            {
				GameManager.Candy++;
            }
			transform.parent = null;
			GetComponent<CircleCollider2D>().enabled = false;
			Sprite.enabled = false;
			splatCandy.Play();
			Destroy(gameObject, 3f);
			//}
		}
	}
}
