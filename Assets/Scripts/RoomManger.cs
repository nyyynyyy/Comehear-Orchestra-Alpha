using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System;

public class User
{
    private string name;
    private InstrumentType instrument;
    private bool isOwner;

    public User(string name, InstrumentType instrument, bool isOwner)
    {
        this.name = name;
        this.instrument = instrument;
        this.isOwner = isOwner;
    }

    public string GetName()
    {
        return name;
    }

    public InstrumentType GetInstrument()
    {
        return instrument;
    }

    public bool IsOwner()
    {
        return isOwner;
    }
}

public class UserParser
{
    public static List<User> users = new List<User>();

    public UserParser(string str)
	{
        foreach (string data in str.Split('\n'))
        {
			if (data.Equals (""))	// 마지막 데이터라면
				break;
            string[] elements = data.Split('|');

            string name = elements[0];								// 이름
            InstrumentType instrument = fromString(elements[1]);	// 악기
            bool isOwner = Convert.ToBoolean(elements[2]);			// 방장 여부

            users.Add(new User(name, instrument, isOwner));
        }

		RoomManger.GetInstance ().isUpdate = true;
    }

    private InstrumentType fromString(string str)
    {
        switch (str)
        {
            case "Violin":
                return InstrumentType.Violin;
            case "Timpani":
                return InstrumentType.Timpani;
            case "Oboe":
                return InstrumentType.Oboe;
            default:
                return InstrumentType.Null;
        }
    }
}

public class RoomManger : MonoBehaviour {

	private static RoomManger instance;

    public GameObject _unit;
    public GameObject _canvas;

    private List<GameObject> notes = new List<GameObject>();

    private int _index;

	public bool isUpdate = false;

    void Awake()
    {
		instance = this;
    }

	// Use this for initialization
	void Start () {
        SocketEvent();
        SocketManager.Socket.Emit("getroomusers", null);
    }
	
	// Update is called once per frame
	void Update () {
		if (isUpdate) {
			UpdateUserList ();
			isUpdate = !isUpdate;
		}
	}

	public void UpdateUserList() {
		ClearUnits();

		foreach (User user in UserParser.users.ToArray())
		{
			string name = user.GetName();
			InstrumentType instrument = user.GetInstrument();
			bool isOwner = user.IsOwner();

			if (isOwner)
				CreateUnit("악장", name, instrument);
			else
				CreateUnit(name, instrument);
		}
		UserParser.users.Clear();
	}


    private void SocketEvent()
    {
        SocketManager.Socket.On("users", (data) => {
            new UserParser((string) data.Json.args[0]);
        });
    }

    private void CreateUnit(string name, InstrumentType instrument)
    {
        //유닛 생성
        GameObject note = Instantiate(_unit);
        notes.Add(note);


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

    private void ClearUnits()
    {
        foreach (GameObject gameObject in notes)
        {
            Destroy(gameObject);
        }
		_index = 0;
		notes.Clear ();
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
        //UserParser.users.
        Debug.Log(Global._type + " 잠시후 입장합니다.");
        SceneManager.LoadScene(Global._type.ToString());
    }

	public static RoomManger GetInstance() {
		return instance;
	}
}
