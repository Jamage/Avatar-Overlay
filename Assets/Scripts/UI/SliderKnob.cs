using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class SliderKnob : MonoBehaviour
{

    public static UnityAction<Vector3> OnValueDragging;
    public static UnityAction<Vector3> OnValueChanged;

    private void OnMouseDown()
    {
        
    }

    private void OnMouseDrag()
    {
        //adjust horizontal position
        //On Value Changed (internal)
        
        OnValueDragging?.Invoke(transform.localPosition);
    }

    private void OnMouseUp()
    {
        //On Value Changed (external)
        OnValueChanged?.Invoke(transform.localPosition);
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
