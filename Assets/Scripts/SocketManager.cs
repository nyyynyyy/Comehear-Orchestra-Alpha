using UnityEngine;
using SocketIOClient;
using SocketIOClient.Eventing;

public class SocketManager : MonoBehaviour {

	private const string ADRESS = "http://insi_server.iptime.org:52252";

    private SocketManager() {
    }

    public static Client Socket {
		get;
		private set;
	}

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);    // 씬이 바뀌어도 해당 게임오브젝트는 살려둠
        Socket = new Client(ADRESS);
        Socket.Opened += SocketOpened;
        Socket.Connect();
    }


	private void SocketOpened(object sender, System.EventArgs e) {
		Debug.Log ("Socket Opened");
	}

    void OnDisable()
    {
        Socket.Close();
        Debug.Log("소켓죽음");
    }
}
