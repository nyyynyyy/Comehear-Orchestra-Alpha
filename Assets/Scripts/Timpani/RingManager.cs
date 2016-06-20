using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RingManager : MonoBehaviour {
	public GameObject ringPrefab;

	public Sprite[] clearImage;
	public AudioClip[] audioClips;
	public Transform[] createLocations;

	private AudioSource audioSource;

	void Awake() { 
		Ring.Init(ringPrefab, this);
	}

	void Start () {
		audioSource = gameObject.AddComponent<AudioSource>();
		StartCoroutine(StartNote());
	}

	private IEnumerator StartNote() {
        int index = 0;
        foreach (AudioClip audioClip in audioClips) {
            index++;
            yield return new WaitForSeconds(index % 7 == 0?1f:0.5f);
			Ring.CreateNote(20, createLocations[Random.Range(0, createLocations.Length-1)], audioClip);
		}
	}

	public void ClearRing(Clear type, Ring ring) {
		Destroy(ring.gameObject);

		GameObject spriteImage = (GameObject) Instantiate(new GameObject(), ring.transform.position, Quaternion.identity);
		SpriteRenderer sprite = spriteImage.AddComponent<SpriteRenderer>();

		/*sprite.sprite = clearImage[(int)type];
		sprite.transform.localScale *= 5;

		StartCoroutine(DelayDestory(0.5f, spriteImage));*/

		if (type == Clear.MISS)
			return;
		
		audioSource.clip = ring.GetAudioClip();
		audioSource.Play();
	}

	private IEnumerator DelayDestory(float delay, GameObject targetObject)
	{
		yield return new WaitForSeconds(delay);
		Destroy(targetObject);
	}
}
