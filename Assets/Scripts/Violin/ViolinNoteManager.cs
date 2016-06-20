using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public enum Beat {
    Half = 2,
    Qu = 1,
}

public enum Syllable
{
    F = 3, G = 4, /*BF = 7,*/
    B = 6, /*CS = 8,*/ E = 2,
    /*DS = 9,*/ /*FS = 10,*/ A = 5,
    /*GS = 11,*/ C = 0, D = 1,
    /*EF = 12,*/
}

[System.Serializable]
public class OneNote
{
    [HideInInspector]public string name = "OneNote";
    public Beat beat;
    public Syllable syllable;
}

public class ViolinNoteManager : MonoBehaviour {

    public Text _textRank;
    public Text _textCombo;
    public Text _textScore;

    public GameObject _noteSample;
    public int _maxNote = 30;
    public float _noteSpeed;

    public AudioSource _noteSource;
    public AudioClip[] _noteClip;

    public OneNote[] _note;

    public GameObject[] _indication;
    public GameObject[] _line;

    private int _noteIndex = 0;

    private int _score = 0;
    private int _combo = 0;

    private int _textOverlap;

    private WaitForSeconds time = new WaitForSeconds(0.5f); //박자

    private List<ViolinNote> NoteList;

    void Awake()
    {

    }

	void Start ()
    {
        ResetNote();
        StartCoroutine("PlayGame");
        StartCoroutine("ReStart");
	}
	
	void Update ()
    {

	}

    void OnApplicationPause(bool pause)
    { 
        if (pause)
        {
            Application.Quit();
        }
    }

    private void GameReset()
    {
        _noteIndex = 0;

        _score = 0;
        _combo = 0;

        _textOverlap = 0;
    }

    private IEnumerator ReStart()
    {
        while(true)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                //Application.Quit();//종료 함수..
                StopCoroutine("PlayGame");
                yield return new WaitForSeconds(3f);
                GameReset();
                StartCoroutine("PlayGame");
            }
            yield return null;
        }
    }

    private IEnumerator PlayGame()
    {
        yield return time;

        for(int i = 0;i < _note.Length; i++)
        {
            CreateNote((int)_note[i].syllable);
            Debug.Log("SYLLABLR : " + (int)_note[i].syllable);
            for(int j = 0;j < (int)_note[i].beat; j++)
            {
                yield return time;
            }
        }
    }

    private void ResetNote()
    {
        //create parent
        NoteList = new List<ViolinNote>();
        GameObject master = new GameObject();
        master.name = "Notes";
        //create parent

        //carete note
        Vector3 waitPos = new Vector3(0, 12, 0);
        for (int i = 0; i < _maxNote; i++)
        {
            GameObject temp = (GameObject)Instantiate(_noteSample, waitPos, Quaternion.identity);
            temp.transform.parent = master.transform;
            NoteList.Add(temp.GetComponent<ViolinNote>());
            NoteList[i]._waitPos = waitPos;
            NoteList[i]._speed = _noteSpeed;
        }
        //create note
    }

    private void CreateNote(int magicNumber)
    {
        ViolinNote selectedNote = NoteList.Find(o => o.GetUsed() == false);

        Vector3 finishPos = SyllableToPosition(magicNumber);
        Vector2 startPos = finishPos + new Vector3(0, 10f);

        selectedNote.transform.position = startPos; // 출발지 지정
        selectedNote._finishPos = finishPos; // 목적지 지정

        selectedNote.SetIndex(magicNumber); // 인덱스 번호 지정

        selectedNote.SetColor(magicNumber / 3); // 세 개가 같은 색깔을 가짐
        selectedNote.SetUsed(true); // 노트 실행

        _noteIndex++;
    }

    public Vector2 SyllableToPosition(int value)
    {
        int L = value % 3;
        int I = value / 3;

        if(value == 12) //EF의 경우 예외처리
        {
            L = 1;
            I = 1;
        }

        float X = _line[L].transform.position.x;
        float Y = _indication[I].transform.position.y;

        return new Vector2(X, Y);
    }

    public void PlayNote(int index)
    {
        Debug.Log("index : " + index);
        _noteSource.clip = _noteClip[index];
        _noteSource.Play();
    }

    public void ViewScore(int value)
    {
        _score += value;
       // textScore.text = score.ToString();
    }

    public IEnumerator ViewRank(string value)
    {
        _textOverlap++;
        //textRank.text = value;
        yield return new WaitForSeconds(0.3f);
        //if (textOverlap == 1) textRank.text = null;
        _textOverlap--;
    }

    public void ViewCombo(bool success)
    {
        _combo=success?_combo+1:0;
        //textCombo.text = combo.ToString();
    }
}

//잠깐 버림 누군가 이걸 읽는다면 영원히 버린거
/*CreateNote((int)Syllable.C);
        yield return time;
        CreateNote((int)Syllable.C);
        yield return time;
        CreateNote((int)Syllable.G);
        yield return time;
        CreateNote((int)Syllable.G);
        yield return time;
        CreateNote((int)Syllable.A);
        yield return time;
        CreateNote((int)Syllable.A);
        yield return time;
        CreateNote((int)Syllable.G);
        yield return time;
        yield return time;
        //도도솔솔라라솔

        CreateNote((int)Syllable.F);
        yield return time;
        CreateNote((int)Syllable.F);
        yield return time;
        CreateNote((int)Syllable.E);
        yield return time;
        CreateNote((int)Syllable.E);
        yield return time;
        CreateNote((int)Syllable.D);
        yield return time;
        CreateNote((int)Syllable.D);
        yield return time;
        CreateNote((int)Syllable.C);
        yield return time;
        yield return time;
        //파파미미레레도

        CreateNote((int)Syllable.G);
        yield return time;
        CreateNote((int)Syllable.G);
        yield return time;
        CreateNote((int)Syllable.F);
        yield return time;
        CreateNote((int)Syllable.F);
        yield return time;
        CreateNote((int)Syllable.E);
        yield return time;
        CreateNote((int)Syllable.E);
        yield return time;
        CreateNote((int)Syllable.D);
        yield return time;
        yield return time;
        //솔솔파파미미레

        CreateNote((int)Syllable.G);
        yield return time;
        CreateNote((int)Syllable.G);
        yield return time;
        CreateNote((int)Syllable.F);
        yield return time;
        CreateNote((int)Syllable.F);
        yield return time;
        CreateNote((int)Syllable.E);
        yield return time;
        CreateNote((int)Syllable.E);
        yield return time;
        CreateNote((int)Syllable.D);
        yield return time;
        yield return time;
        //솔솔파파미미레

        CreateNote((int)Syllable.C);
        yield return time;
        CreateNote((int)Syllable.C);
        yield return time;
        CreateNote((int)Syllable.G);
        yield return time;
        CreateNote((int)Syllable.G);
        yield return time;
        CreateNote((int)Syllable.A);
        yield return time;
        CreateNote((int)Syllable.A);
        yield return time;
        CreateNote((int)Syllable.G);
        yield return time;
        yield return time;
        //도도솔솔라라솔

        CreateNote((int)Syllable.F);
        yield return time;
        CreateNote((int)Syllable.F);
        yield return time;
        CreateNote((int)Syllable.E);
        yield return time;
        CreateNote((int)Syllable.E);
        yield return time;
        CreateNote((int)Syllable.D);
        yield return time;
        CreateNote((int)Syllable.D);
        yield return time;
        CreateNote((int)Syllable.C);
        yield return time;
        yield return time;
        //파파미미레레도*/
