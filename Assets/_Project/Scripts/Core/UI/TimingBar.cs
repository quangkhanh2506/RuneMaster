using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimingBar : MonoBehaviour
{
    private bool isEnable = false;
    public int Multipler;
    public void SetValueBar(int value)
    {
        if (isEnable)
        {
            //HapticManager.Instance.PlayHaptic();
        }
        
        Multipler = value;
    }
    // Start is called before the first frame update
    private void OnEnable()
    {
        isEnable = true;
    }
    private void OnDisable()
    {
        isEnable = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
