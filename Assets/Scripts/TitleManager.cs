using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System;

public class TitleManager : MonoBehaviour {

    public Text _name;
    public Text _log;

    public void OnClickStart()
    {
        if (_name.text.ToString() != "")
        {
            Global._name = _name.text.ToString();
            SceneManager.LoadScene("SelectInstrument");
        }
        else
        {
            _log.text = "여기를 누른 후 이름을 알려주세요.";
        }
    }
}
