using UnityEngine;
using System.Collections;

public class Ring : MonoBehaviour {
	private Ring() { }

	private static GameObject prefab;
	private static RingManager manager;

	public static void Init(GameObject prefab, RingManager manager) {
		Ring.prefab = prefab;
		Ring.manager = manager;
	}

	public static void CreateNote(float speed, Transform location, AudioClip audioClip)
	{
		GameObject gameObject = (GameObject)Instantiate(prefab, location.position, location.rotation);
		Ring ring = gameObject.GetComponent<Ring>();

		ring._speed = new Vector3(speed, speed, 0);
		ring._audioClip = audioClip;
	}

	private const float INDICATION = 1f;
	private const float RANK_PERFECT = 0.5f;
	private const float RANK_GREAT = 2f;
	private const float RANK_GOOD = 3f;
	private const float RANK_BAD = 5f;

	[SerializeField]private GameObject _realRing;
	[SerializeField]private GameObject _changeRing;

	private Vector3 _speed;
	private AudioClip _audioClip;

	public void Update() {
		TouchCheck();
		ChangeScale();
	}

	public AudioClip GetAudioClip()
	{
		return _audioClip;
	}

	private void ChangeScale() {
		float changeSize = _changeRing.transform.localScale.x;
		float originalSize = _realRing.transform.localScale.x;
		bool overSize =  changeSize  > originalSize + RANK_BAD;

		if (overSize)
			manager.ClearRing(Clear.MISS, this);
		
		_changeRing.transform.localScale = _changeRing.transform.localScale + _speed * Time.deltaTime;
	}

	private void TouchCheck() {
		if (!Input.GetMouseButtonDown(0)) return;
		if (isIndication()) return;

		float gap = Mathf.Abs(_realRing.transform.localScale.x- _changeRing.transform.localScale.x);

		if (gap > RANK_BAD) return;

		Clear type = Clear.BAD;
		if (gap < RANK_GOOD) type = Clear.GOOD;
		if (gap < RANK_GREAT) type = Clear.GREAT;
		if (gap < RANK_PERFECT) type = Clear.PERFACT;

		manager.ClearRing(type, this);
	}

	private bool isIndication()
	{
		Vector3 pinger;
		if (Application.platform == RuntimePlatform.Android)
			pinger = Input.GetTouch(0).position;
		else
			pinger = Input.mousePosition;
		Vector3 touchPos = Camera.main.ScreenToWorldPoint(pinger);
		//Debug.Log("Indication : " + SimilarDistance(mousePos, finishPos));
		return (SimilarDistance(touchPos, transform.position) > INDICATION);
	}

	private float SimilarDistance(Vector2 A, Vector2 B)
	{
		Vector2 result;
		result = A - B;
		result.x = Mathf.Abs(result.x);
		result.y = Mathf.Abs(result.y);
		return Mathf.Max(result.x, result.y);
	}
}
