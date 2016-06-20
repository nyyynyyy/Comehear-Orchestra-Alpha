using UnityEngine;
using System.Collections;

public class ViolinNote : MonoBehaviour {

    private ViolinNoteManager _noteM;

    [HideInInspector] public Vector2 _finishPos;
    [HideInInspector] public Vector2 _waitPos;
    [HideInInspector] public float _speed;

    private int _index;

    private bool _isUsed;
    private bool _isChecked;

    private SpriteRenderer _sprite;

    /*/INDICATION CONSTANT*/
    private const float INDICATION = 0.8f;
    private const float RANK_PERFECT = 0.5f;
    private const float RANK_GREAT = 0.8f;
    private const float RANK_GOOD = 1.5f;
    private const float RANK_BAD = 2.0f;
    private enum RANK
    {
       MISS,
       BAD,
       GOOD,
       GREAT,
       PERFECT,
    }
    /*INDICATION CONSTANT*/

    /*COLOR CONSTANT*/
    private Color[] COLOR = new Color[4]
    {
        new Color(255, 120, 230), // PINK
        new Color(125, 110, 235), // BLUE
        new Color(100, 215, 240), // SKY
        new Color(100, 240, 135), // GREEN
    };
    /*COLOR CONSTANT*/

    void Awake()
    {
        _sprite = GetComponent<SpriteRenderer>();
        _noteM = GameObject.Find("Manager").GetComponent<ViolinNoteManager>();
    }

	void Start ()
    {
        //isUsed = false;
        _isChecked = false;
        //finishPos = Vector2.zero;
    }
	
	void Update ()
    {
        MoveNote();
        CheckTouch();
        DeleteNote();
	}

    private void MoveNote()
    {
        if (!_isUsed) return;

        if (transform.position.y < _finishPos.y - RANK_BAD) // 노트가 BAD 판정범위 보다 낙하했을 때
        {
            StartCoroutine(_noteM.ViewRank("MISS"));
            _noteM.ViewCombo(false);
            _isUsed = false;
            _isChecked = true;
            return;
        }

        transform.position += new Vector3(0, -_speed);
    }

    private void CheckTouch()
    {
        if (!_isUsed) return;

        if (!Input.GetMouseButtonDown(0)) return;
        //Debug.Log("SUCCESS TOUCH");
        
        if (isIndication()) return;
        //Debug.Log("SUCCESS Indication");

        float distance = SimilarDistance(transform.position, _finishPos);
        //Debug.Log("B : " + distance);

        if (distance > RANK_BAD) return;

        //아래부터는 판정 완료

        RANK rank = RANK.MISS;

        if (distance < RANK_BAD) rank = RANK.BAD;
        if (distance < RANK_GOOD) rank = RANK.GOOD;
        if (distance < RANK_GREAT) rank = RANK.GREAT;
        if (distance < RANK_PERFECT) rank = RANK.PERFECT;

        StartCoroutine(_noteM.ViewRank(rank.ToString()));
        _noteM.ViewScore(RankToScore(rank));
        _noteM.ViewCombo(rank!=RANK.BAD); // BAD가 아니라면 콤보 유지
        _noteM.PlayNote(_index);

        _isChecked = true;
    }

    private bool isTouch()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            for (int i = 0; i < 5; i++)
            {
                if (Input.GetTouch(i).phase != TouchPhase.Began) continue;

                Vector3 pos = Input.GetTouch(i).position;
                Vector3 worldpos = Camera.main.ScreenToWorldPoint(pos);
                worldpos.z = 0;

                return true;
            }

            return false;
        }
        else
        {
            return Input.GetMouseButtonDown(0);
        }
        
    }

    private int RankToScore(RANK rank)
    {
        switch (rank)
        {
            case RANK.BAD:
                return 10;
            case RANK.GOOD:
                return 55;
            case RANK.GREAT:
                return 130;
            case RANK.PERFECT:
                return 200;
            default:
                return 0;
        }
    }

    private bool isIndication()
    {
        Vector3 pinger;
        if (Application.platform == RuntimePlatform.Android)
            pinger =  Input.GetTouch(0).position;
        else
            pinger =  Input.mousePosition;
        Vector3 touchPos = Camera.main.ScreenToWorldPoint(pinger);
        //Debug.Log("Indication : " + SimilarDistance(mousePos, finishPos));
        return (SimilarDistance(touchPos, _finishPos) > INDICATION);
    }

    private float SimilarDistance(Vector2 A, Vector2 B)
    {
        Vector2 result;
        result = A - B;
        result.x = Mathf.Abs(result.x);
        result.y = Mathf.Abs(result.y);
        return Mathf.Max(result.x,result.y);
    }

    private void DeleteNote()
    {
        if (!_isChecked) return;

        transform.position = _waitPos;
        _isUsed = false;
        _isChecked = false;
    }

    /*SETER INDEX*/
    public void SetIndex(int value)
    {
        _index = value;
    }
    /*SETER INDEX*/

    /*SETER COLOR*/
    public void SetColor(int line)
    {
        Color temp = COLOR[line] / 255f;
        temp.a = 1; // 알파값 255
        _sprite.color = temp;
    }
    /*SETER COLOR*/

    /*SETER OR GETER USED*/
    public void SetUsed(bool value)
    {
        _isUsed = value;
    }

    public bool GetUsed()
    {
        return _isUsed;
    }
    /*SETER OR GETER USED*/
}
