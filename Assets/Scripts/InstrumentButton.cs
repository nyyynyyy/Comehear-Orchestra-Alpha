using UnityEngine;
using System.Collections;

public class InstrumentButton : MonoBehaviour {

	void OnMouseDown()
    {
        SelectInstrumentManager.instance.SelectType(name);
    }
}
