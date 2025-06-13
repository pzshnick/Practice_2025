using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;

public class SelectController : MonoBehaviour
{
    public GameObject cube;
    public List<GameObject> players;
    public LayerMask layer, layerMask;
    private Camera _cam;
    private GameObject _cubeSelection;
    private RaycastHit _hit;

    private void Awake()
    {
        _cam = GetComponent<Camera>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1) && players.Count > 0)
        {
            Ray ray = _cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit agentTarget, 1000f, layer))
            {
                foreach (var el in players)
                {
                    el.GetComponent<NavMeshAgent>().SetDestination(agentTarget.point);
                }
            }
        }

        // Start of object selection on the map
        if (Input.GetMouseButtonDown(1))
        {
            /*
                The health status element, which is the 
                first child of the car prefab, becomes 
                active to indicate that it is selected 
            */
            foreach (var el in players)
            {
                el.transform.GetChild(0).gameObject.SetActive(false);
            }

            players.Clear();

            Ray ray = _cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out _hit, 1000f, layer))
            {
                _cubeSelection = Instantiate(cube, new Vector3(_hit.point.x, 0.6f, _hit.point.z), Quaternion.identity);
            }
        }

        // Applies the correct transformation to the removal object.
        if (_cubeSelection)
        {
            Ray ray = _cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hitDrag, 1000f, layer))
            {
                float scaleX = (_hit.point.x - hitDrag.point.x) * -1;
                float scaleZ = _hit.point.z - hitDrag.point.z;

                if (scaleX < 0.0f && scaleZ < 0.0f)
                {
                    _cubeSelection.transform.localRotation = Quaternion.Euler(0, 180, 0);
                }
                else if (scaleX < 0.0f)
                {
                    _cubeSelection.transform.localRotation = Quaternion.Euler(0, 0, 180);
                }
                else if (scaleZ < 0.0f)
                {
                    _cubeSelection.transform.localRotation = Quaternion.Euler(180, 0, 0);
                }
                else
                {
                    _cubeSelection.transform.localRotation = Quaternion.Euler(0, 0, 0);
                }

                _cubeSelection.transform.localScale = new Vector3(Mathf.Abs(scaleX), 0.6f, Mathf.Abs(scaleZ));
            }
        }

        // Checks which objects are inside a rectangular area (box) on a specific layer.
        if (Input.GetMouseButtonUp(1) && _cubeSelection)
        {
            RaycastHit[] hits = Physics.BoxCastAll(_cubeSelection.transform.position,
                               _cubeSelection.transform.localScale,
                               Vector3.up,
                               Quaternion.identity,
                               0,
                               layerMask);

            foreach (var hit in hits)
            {
                players.Add(hit.transform.gameObject);
                hit.transform.GetChild(0).gameObject.SetActive(true);
            }

            Destroy(_cubeSelection);
        }
    }
}
