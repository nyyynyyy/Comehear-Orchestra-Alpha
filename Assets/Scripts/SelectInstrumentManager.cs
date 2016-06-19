using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SelectInstrumentManager : MonoBehaviour {

    public Text _log;

    public GameObject Oboe;
    public GameObject Violin;
    public GameObject Timpani;

    public static SelectInstrumentManager instance;

    void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
        else
        {
            Debug.LogError("SELECTINSTRUMENT : 다중 인스턴스");
        }
    }

	// Use this for initialization
	void Start () {
        _log.text = Global._name + "님 환영합니다.";
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    public void SelectType(string name)
    {
        switch (name)
        {
            case "Violin":
                Global._type = InstrumentType.Violin;
                break;
            case "Timpani":
                Global._type = InstrumentType.Timpani;
                break;
            case "Oboe":
                Global._type = InstrumentType.Oboe;
                break;
        }

        SceneManager.LoadScene("Room");
        SocketManager.Socket.Emit("selinstrument", Global._type.ToString());
    }
}
