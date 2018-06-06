using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{

    public GameObject _camera;
    public List<GameObject> catRoomToHide, shopToHide, encToHide, catLabToHide, mapToHide;
    public GameObject _CatRoom;
    public GameObject _Shop;
    public GameObject _Encyclopedia;
    public GameObject _CatLab;
    public GameObject _Map;
    public double x;
    public float speed;
    private GameObject _nowPosition;
    private Transform _startMarker;
    private float _journeyLength, _startTime;
    private List<GameObject> cameraLocations = new List<GameObject>();
    private List<List<GameObject>> everythingToHide = new List<List<GameObject>>();

    // Use this for initialization
    void Start()
    {
        init();
        if (_camera == null)
        {
            Debug.Log("nope");
            _camera = gameObject;
        }
        _startTime = Time.time;
        _journeyLength = 0f;
        _camera.transform.position = GetPositionForCamera(_CatRoom);
        _nowPosition = _CatRoom;
        moveCameraToHome();
    }

    void init()
    {
        cameraLocations.Add(_CatRoom);
        cameraLocations.Add(_Shop);
        cameraLocations.Add(_Encyclopedia);
        cameraLocations.Add(_CatLab);
        cameraLocations.Add(_Map);
        everythingToHide.Add(catRoomToHide);
        everythingToHide.Add(shopToHide);
        everythingToHide.Add(encToHide);
        everythingToHide.Add(catLabToHide);
        everythingToHide.Add(mapToHide);
    }

    // Update is called once per frame
    void Update()
    {
        if (!V3Equal(GetPositionForCamera(_nowPosition), _camera.transform.position))
        {

            float distCovered = (Time.time - _startTime) * speed;
            float fracJourney = distCovered / _journeyLength;
            _camera.transform.position = Vector3.Lerp(_startMarker.position, GetPositionForCamera(_nowPosition), fracJourney);
            //Debug.Log(_nowPosition.transform.position);
            //Debug.Log(Vector3.Lerp(_startMarker.position,GetPositionForCamera(_nowPosition), fracJourney));
        }
    }
    public void moveCameraToCatLab()
    {
        MoveToTarget(_CatLab);
        //ActivateItem(_CatLab);
        //x = -10.9 - _camera.transform.position.x;
    }
    public void moveCameraToHome()
    {
        MoveToTarget(_CatRoom);
        //ActivateItem(_CatRoom);
        //x = -0.1 - _camera.transform.position.x;
    }
    public void moveCameraToEN()
    {
        MoveToTarget(_Encyclopedia);
        //ActivateItem(_Encyclopedia);
        //x = -21.8 - _camera.transform.position.x;
    }
    public void moveCameraToMap()
    {
        MoveToTarget(_Map);
        //ActivateItem(_Map);
        //x = 10.7 - _camera.transform.position.x;
    }
    public void moveCameraToShop()
    {
        MoveToTarget(_Shop);
        //ActivateItem(_Shop);
        //x = 21.6 - _camera.transform.position.x;
    }

    private void MoveToTarget(GameObject target)
    {
        _startTime = Time.time;
        _startMarker = transform;
        _journeyLength = Vector3.Distance(_camera.transform.position, GetPositionForCamera(target));
        _nowPosition = target;
    }

    private Vector3 GetPositionForCamera(GameObject target)
    {
        return new Vector3(target.transform.position.x, target.transform.position.y, transform.position.z);
    }

    private bool V3Equal(Vector3 a, Vector3 b)
    {
        return Vector3.SqrMagnitude(a - b) < 0.0001;
    }

    private void ActivateItem(GameObject item)
    {
        for (int i = 0; i < cameraLocations.Count; i++)
        {
            if(cameraLocations[i] == item){
                SetListActivation(everythingToHide[i], true);
            }else{
                SetListActivation(everythingToHide[i], false);
            }
        }
    }

    private void SetListActivation(List<GameObject> li, bool stat)
    {
        foreach(GameObject go in li){
            go.SetActive(stat);
        }
    }

}
