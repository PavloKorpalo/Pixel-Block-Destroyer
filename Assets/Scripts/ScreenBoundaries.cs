using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenBoundaries : MonoBehaviour
{

    [SerializeField] private Camera _MainCamera;
    private Vector2 _screenBounds;
    private float _objectWidth;
    private float _objectHeight;

    // Use this for initialization
    void Start()
    {
        _screenBounds = _MainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, _MainCamera.transform.position.z));
        _objectWidth = transform.GetComponent<SpriteRenderer>().bounds.extents.x; //extents = size of width / 2
        _objectHeight = transform.GetComponent<SpriteRenderer>().bounds.extents.y; //extents = size of height / 2
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 viewPos = transform.position;
        viewPos.x = Mathf.Clamp(viewPos.x, _screenBounds.x * -1 + _objectWidth, _screenBounds.x - _objectWidth);
        viewPos.y = Mathf.Clamp(viewPos.y, _screenBounds.y * -1 + _objectHeight, _screenBounds.y - _objectHeight);
        transform.position = viewPos;
    }

}
