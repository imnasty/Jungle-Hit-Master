

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(SpriteRenderer))]
public class Circle : MonoBehaviour {

	public int totalKnife = 5;
	public List<RotationVariation> RandomRotation = new List<RotationVariation>();
	public List<LevelVariation> RandomLevels = new List<LevelVariation>();

	public ParticleSystem hitParticle, splashParticle;
	[Space(20)]
	public bool isBoss = false;
	public Sprite woodSprite, blueWoodSprite;
	public ParticleSystem WoodSplatParticle, BlueWoodSplatParticle;
	[Space(20)]
	public bool isRandomClockWise = false;

	public List<Knife> hitedKnife = new List<Knife>();
	public AudioClip woodHitSfx, LasthitSfx;

	[Header("Цвет попадания")] public Color hitedColor;

	int currentRoationIndex=0;
	int currentLevelndex=0;
	// Use this for initialization
	float valueZ ;
	void Start () {

		GamePlayManager.currentBossTag = gameObject.tag;

		if (!isBoss) {
			GetComponent<SpriteRenderer>().sprite=GameManager.Stage%10<5?woodSprite:blueWoodSprite;
		}

		if (RandomRotation.Count > 0) {
			ApplyRotation ();
		}
		currentLevelndex = Random.Range (0,RandomLevels.Count);
		//print ("Current Level"+currentLevelndex);
		if (RandomLevels [currentLevelndex].candyPosibility > Random.value) {
			SpawnCandy ();
		}
		SpawnKnife ();
	}
	void ApplyRotation()
	{
		currentRoationIndex = (currentRoationIndex + 1) % RandomRotation.Count;
		LeanTween.rotateZ (gameObject, transform.localRotation.eulerAngles.z + RandomRotation [currentRoationIndex].z, RandomRotation [currentRoationIndex].time).setOnComplete (ApplyRotation).setEase (RandomRotation [currentRoationIndex].curve);
	}

	void SpawnCandy()
	{
		foreach (float item in RandomLevels[currentLevelndex].CandyAngles) {
			GameObject tempCandy = Instantiate<GameObject> (GamePlayManager.instance.CandyPrefab);
			tempCandy.transform.SetParent (transform);
			setPosInCircle (transform, tempCandy.transform,item, 0.28f, -90f);
			tempCandy.transform.localScale = Vector3.one;
		}
	}
	void SpawnKnife()
	{
		foreach (float item in RandomLevels[currentLevelndex].KnifeAngles) {
			GameObject tempKnife = Instantiate<GameObject> (GamePlayManager.instance.knifePrefab.gameObject);
			tempKnife.transform.SetParent (transform);
			tempKnife.GetComponent<Knife> ().isHitted = true;
			tempKnife.GetComponent<Knife> ().isFire = true;
			tempKnife.GetComponents<BoxCollider2D> () [0].enabled = true;
			tempKnife.GetComponents<BoxCollider2D> () [1].enabled = true;
			setPosInCircle (transform, tempKnife.transform,item, 0.12f, 90f);
			tempKnife.transform.localScale = new Vector3 (0.8f, 0.8f, 0.8f);
		}
	}
	void setPosInCircle(Transform circle,Transform obj,float angle,float spaceBetweenCircleAndObject,float objAngelOffset)
	{
		angle = angle + 90f;
		Vector2 offset = new Vector2(Mathf.Sin(angle*Mathf.Deg2Rad), Mathf.Cos(angle*Mathf.Deg2Rad)) * (circle.GetComponent<CircleCollider2D>().radius+spaceBetweenCircleAndObject);
		obj.localPosition = (Vector2)circle.localPosition + offset;
		obj.localRotation = Quaternion.Euler (0, 0, -angle+90f+objAngelOffset);
	}

	/*void OnCollisionEnter2D(Collision2D coll) {
		if (coll.gameObject.tag == "Knife")
		{			
			
			LeanTween.moveLocalY (gameObject, 0.1f, 0.05f).setLoopPingPong(1).setEaseInBounce();
			coll.gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
			coll.rigidbody.velocity = Vector2.zero;
			coll.gameObject.transform.SetParent (transform);
			coll.gameObject.GetComponent<Knife> ().isHitted = true;
			hitedKnife.Add (coll.gameObject.GetComponent<Knife> ());
			playParticle(coll.collider.transform.position,hitParticle);

			if (hitedKnife.Count >= totalKnife) {
				if (!GameManager.isGameOver) {
					StartCoroutine (RelaseAllKnife ());
					SoundManager.instance.PlaySingle (LasthitSfx);
				}
			} else {
				playParticle(GamePlayManager.instance.circleSpawnPoint.transform.position,splashParticle);
				SoundManager.instance.PlaySingle (woodHitSfx);
			}
			GameManager.score++;
		}
	}*/
	public  void OnKnifeHit(Knife k){
			
			k.rb.isKinematic = true;
			k.rb.velocity = Vector2.zero;
			k.transform.SetParent (transform);
			k.isHitted = true;
			hitedKnife.Add (k);

			playParticle(k.transform.position,hitParticle);
			LeanTween.moveLocalY (gameObject, 0.08f, 0.05f).setLoopPingPong(1);
		if (hitedKnife.Count >= totalKnife) {
				if (!GameManager.isGameOver) {
					StartCoroutine (RelaseAllKnife ());
					SoundManager.instance.PlaySingle (LasthitSfx);
				}
		}
			else 
			{
				//playParticle(GamePlayManager.instance.circleSpawnPoint.transform.position,splashParticle);
				StartCoroutine(HitSplashEffect());
				SoundManager.instance.PlaySingle (woodHitSfx);
			}
				GameManager.Score++;
	}


	public void playParticle(Vector3 pos,ParticleSystem _particle)
	{
		ParticleSystem tempParticle= Instantiate(_particle);
		tempParticle.transform.position = pos;
		tempParticle.Play ();

	}

	public IEnumerator RelaseAllKnife()
	{
		LeanTween.cancel (gameObject);
		if (!isBoss) {
			playParticle (transform.position, GameManager.Stage % 10 < 5 ? WoodSplatParticle : BlueWoodSplatParticle);
		} else {
			playParticle (transform.position, WoodSplatParticle);
		}
		gameObject.GetComponent<SpriteRenderer> ().enabled = false;
		yield return new WaitForSeconds (0.02f);
		foreach (Transform item in transform) {
			//print (item.name);
			if (item.transform.tag.Equals ("Knife")) {				
				item.GetComponents<BoxCollider2D> ()[0].enabled = false;
				item.GetComponents<BoxCollider2D> ()[1].enabled = false;

				item.GetComponent<Knife> ().rb.isKinematic = false;
				//item.GetComponent<Knife>().transform.parent = null;
				item.GetComponent<Knife> ().rb.gravityScale = 2f;
				item.GetComponent<Knife> ().rb.freezeRotation = false;
				item.GetComponent<Knife> ().rb.angularVelocity = Random.Range (-20f, 20f) * 35f;
				item.GetComponent<Knife> ().rb.AddForce (new Vector2 (Random.Range (-10f, 10f), Random.Range (3f, 10f)), ForceMode2D.Impulse);
				item.GetComponent<SpriteRenderer> ().sortingOrder = GetComponent<SpriteRenderer> ().sortingOrder + 1;
				item.GetComponent<Knife> ().DestroyMe ();

			} else if (item.transform.tag.Equals ("Candy")) {
				item.GetComponent<Candy> ().rb.isKinematic = false;
				//item.transform.parent = null;
				item.GetComponent<Candy> ().rb.gravityScale = 2.5f;
				item.GetComponent<Candy> ().rb.freezeRotation = false;
				item.GetComponent<Candy> ().rb.angularVelocity = Random.Range (-20f, 20f) * 35f;
				item.GetComponent<Candy> ().rb.AddForce (new Vector2 (Random.Range (-6f, 6f), Random.Range (3f, 10f)), ForceMode2D.Impulse);
				item.GetComponent<CircleCollider2D> ().enabled = false;
				item.GetComponent<SpriteRenderer> ().sortingOrder = GetComponent<SpriteRenderer> ().sortingOrder + 1;
			}
		}

		GetComponent<CircleCollider2D> ().enabled = false;
		yield return new WaitForSeconds (1.5f);
		GamePlayManager.instance.NextLevel ();

	}
	public void destroyMeAndAllKnives()
	{
		foreach (Knife item in hitedKnife) {
			if(item != null)
				Destroy (item.gameObject);
		}
		Destroy (gameObject);

	}


	private IEnumerator HitSplashEffect()
    {
		GetComponent<SpriteRenderer>().color = hitedColor;
        yield return new WaitForSeconds(.1f);
		GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 255);
	}
}

[System.Serializable]
public class RotationVariation{
	[Range(0f,2f)]
	public float time=0f;
	[Range(-180,180f)]
	public float z=0f;
	public AnimationCurve curve;
}

[System.Serializable]
public class LevelVariation{
	[Range(0f,1f)]
	public float candyPosibility=0.5f;
	//[Header("Candy SpawnPointList")]
	public List<float> CandyAngles = new List<float>();
	//[Header("Knife SpawnPointList")]
	public List<float> KnifeAngles= new List<float>();
}
