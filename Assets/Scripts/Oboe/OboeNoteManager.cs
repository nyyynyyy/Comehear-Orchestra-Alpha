using UnityEngine;
using System.Collections;

public class OboeNoteManager : MonoBehaviour {

    private const int SIZE = 5;

    private const float WHOLE = 100f;
    private const float HALF = 50f;
    private const float QUARTER = 25f;
    private const float ElEGHTH = 12.5f;
    private const float SIXTEENTH = 6.25f;

    public static OboeNoteManager instance;

    public GameObject[] _hole;
    public GameObject _note;

    public float _noteSpeed;

    private AudioSource _audio;

    public AudioClip[] _clips;

    private WaitForSeconds _beat = new WaitForSeconds(0.5f);
    
    void Awake()
    {
        if (instance)
        {
            Debug.LogWarning("OBOE NOTE MANAGER 다중 인스턴스 실행 중");
        }
        instance = this;

        _audio = GetComponent<AudioSource>();
    }

    // Use this for initialization
    void Start() {
        StartCoroutine(PlayGame());
    }

    // Update is called once per frame
    void Update() {

    }

    private IEnumerator PlayGame()
    {
        CreateNote(Random.Range(0, 6), QUARTER);
        yield return _beat;
        CreateNote(Random.Range(0, 6), QUARTER);
        yield return _beat;
        CreateNote(Random.Range(0, 6), QUARTER);
        yield return _beat;
        CreateNote(Random.Range(0, 6), QUARTER);
        yield return _beat;
        CreateNote(Random.Range(0, 6), QUARTER);
        yield return _beat;
        CreateNote(Random.Range(0, 6), QUARTER);
        yield return _beat;
        CreateNote(Random.Range(0, 6), HALF);
        yield return _beat;

        yield return new WaitForSeconds(1f);
    }

    public void PlaySound(int pointer)
    {
        StartCoroutine(PlaySoundTest(pointer));
    }

    private void CreateSyllable()
    {

    }

    private void CreateNote(int line, float length)
    {
        length = length * _noteSpeed;
        //생성될 포지션 설정
        Vector3 createPos = GetCreatePosition(line, length);
        //노트 생성
        GameObject note = (GameObject)Instantiate(_note, createPos, Quaternion.identity);
        //크기 및 부모 설정
        note.transform.localScale = new Vector3(0.5f, length);
        note.transform.parent = _hole[line].transform;
        //노트 스크립트 초기 설정
        OboeNote noteCom = note.GetComponent<OboeNote>();
        noteCom.line = line;
        noteCom.length = length;
        //noteCom.index = 0;
    }

    private Vector3 GetCreatePosition(int line, float length)
    {
        float startX = _hole[line].transform.position.x;
        float startY = SIZE + (length / 2f);
        return new Vector3(startX, startY);
    }

    private IEnumerator PlaySoundTest(int pointer)
    {
        _audio.clip = _clips[pointer];
        //_audio.pitch = Mathf.Pow(2, (2f - 4f) / 12f);
        _audio.Play();
        yield return null;
    }
}
