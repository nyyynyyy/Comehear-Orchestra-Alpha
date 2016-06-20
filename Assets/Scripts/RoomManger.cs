using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class RoomManger : MonoBehaviour {

    public GameObject _unit;
    public GameObject _canvas;

    private int _index;

	// Use this for initialization
	void Start () {
        CreateUnit("악장",Global._name, Global._type);
        CreateUnit("최선한", InstrumentType.Violin);
        CreateUnit("정숙경", InstrumentType.Timpani);
        CreateUnit("이세라", InstrumentType.Oboe);
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    private void CreateUnit(string name, InstrumentType instrument)
    {
        //유닛 생성
        GameObject note = Instantiate(_unit);

        //RectTransform 설정
        RectTransform noteRT = note.GetComponent<RectTransform>();

        noteRT.parent = _canvas.transform;
        noteRT.localPosition = IndexToPostion(_index);
        noteRT.localScale = new Vector3(1, 1, 1);

        //자식 설정
        Text nameText = note.transform.FindChild("Name").GetComponent<Text>();
        Text instrumnetText = note.transform.FindChild("Instrument").GetComponent<Text>();

        //자식 값 설정
        nameText.text = name;
        instrumnetText.text = instrument.ToString();

        //인덱스 증가
        _index++;
    }

    private void CreateUnit(string title, string name, InstrumentType instrument)
    {
        CreateUnit("["+title+"]"+name, instrument);
    }

    private Vector3 IndexToPostion(int index)
    {
        Vector3 result = new Vector3();
        result.x = index % 2 == 0 ? -450 : 450;
        result.y = 200 - 200 * (_index / 2);
        return result;
    }

    public void EnterOrchestra()
    {
        Debug.Log(Global._type + " 잠시후 입장합니다.");
        SceneManager.LoadScene(Global._type.ToString());
    }
}
