using UnityEngine;
using System.Collections;

public class OboeNote : MonoBehaviour{

    private int _line;
    private float _length;

    private static int _index = -1;
    private int _myIndex;   

    private float _speed;

    private const int SIZE = 5; //camera size
    private const float INDICATION = -3f;

    private Transform _judgePos;

    private bool _isStart = false;

    public int index
    {
        set
        {
            _index = value;
        }
        get
        {
            return _index;
        }
    }
    public int line
    {
        set
        {
            _line = value;
        }
        get
        {
            return _line;
        }
    }
    public float length
    {
        set
        {
            _length = value;
        }
        get
        {
            return _length;
        }
    }

    void Start()
    {
        _myIndex = ++_index;

        _judgePos = transform.FindChild("JudgePos");

        _speed = OboeNoteManager.instance._noteSpeed;

        StartCoroutine(NoteDown());
    }
    
    void Update()
    {
        NoteClick();
    }

    private IEnumerator NoteDown()
    {
        while (true)
        {
            transform.position += new Vector3(0, -_speed);
            if (IsOutScreen()) Destroy(gameObject); 
            yield return new WaitForFixedUpdate();
        }
    }

    private Vector3 GetPingerPos()
    {
        Vector3 pinger;

        if (Application.platform == RuntimePlatform.Android)
            pinger = Input.GetTouch(0).position;
        else
            pinger = Input.mousePosition;

        return Camera.main.ScreenToWorldPoint(pinger);
    }

    private void NoteClick()
    {
        if (!IsTouch()) return;
        //손가락과 노트 포지션 설정
        Vector3 pingerPos = GetPingerPos();
        Vector3 notePos = _judgePos.position;
        //비교
        if (SimilarDistance(pingerPos, notePos) > 2f) return;
        
        if (!_isStart)
        {
            _isStart = true;
            //Debug.Log(_index);
            OboeNoteManager.instance.PlaySound(_myIndex);
            return;
        }
    }

    private float SimilarDistance(Vector2 A, Vector2 B)
    {
        Vector2 result;
        result = A - B;
        result.x = Mathf.Abs(result.x);
        result.y = Mathf.Abs(result.y);
        return Mathf.Max(result.x, result.y);
    }

    private bool IsTouch()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            for (int i = 0; i < 5; i++)
            {
                if (Input.GetTouch(i).phase != TouchPhase.Began) continue;

                return true;
            }

            return false;
        }
        else
        {
            return Input.GetMouseButtonDown(0);
        }
    }

    private bool IsOutScreen()
    {
        return transform.position.y < -((_length / 2) + SIZE);
    }

}
