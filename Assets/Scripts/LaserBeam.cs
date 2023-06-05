using System.Collections.Generic;
using UnityEngine;

/**
 * Thanks to https://samirgeorgy.medium.com/reflecting-laser-light-in-unity-6d8aa1a4daa2
 */

public class LaserBeam
{
    public GameObject laserObject;
    private LineRenderer _laser;
    private List<Vector3> _laserIndices = new();
    private Vector3 startPosition;
    private Vector3 startDirection;
    private float laserWidth;

    private List<GameObject> cylinderObjects = new();
    private GameObject laserParent;

    public LaserBeam(GameObject parent, float startWidth, float endWidth, Color startColor, Color endColor, Material material)
    {
        _laser = new LineRenderer();
        laserObject = new GameObject();
        laserObject.name = "Laser Beam";
        laserObject.transform.parent = parent.transform;

        _laser = this.laserObject.AddComponent(typeof(LineRenderer)) as LineRenderer;
        _laser.startWidth = startWidth;
        _laser.endWidth = endWidth;
        _laser.startColor = startColor;
        _laser.endColor = endColor;
        _laser.material = material;
        _laser.receiveShadows = false;
        _laser.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

        laserWidth = startWidth;

        startPosition = parent.transform.position;
        startDirection = parent.transform.forward;

        laserParent = parent;

        CastLaser(startPosition, startDirection);
    }

    private void CastLaser(Vector3 position, Vector3 direction)
    {
        _laserIndices.Add(position);

        Ray ray = new Ray(position, direction);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100, 1))
        {
            CheckHit(hit, direction);
        }
        else
        {
            _laserIndices.Add(ray.GetPoint(199));
            UpdateLaser();
        }
    }

    private void CheckHit(RaycastHit hit, Vector3 direction)
    {
        if (hit.collider.tag.Equals("Mirror"))
        {
            Vector3 pos = hit.point;
            Vector3 dir = Vector3.Reflect(direction, hit.normal);

            CastLaser(pos, dir);
        }
        else
        {
            _laserIndices.Add(hit.point);
            UpdateLaser();
        }
    }

    private void UpdateLaser()
    {
        int count = 0;
        _laser.positionCount = _laserIndices.Count;
        
        foreach (Vector3 index in _laserIndices) {
            _laser.SetPosition(count, index);
            count++;
        }
        for (int i = 0; i < _laserIndices.Count - 1; i++)
        {
            cylinderObjects.Add(buildCylinder(_laserIndices[i], _laserIndices[i + 1], laserWidth));
        }
    }

    private GameObject buildCylinder(Vector3 startPoint, Vector3 endPoint, float width)
    {
        Vector3 deltaVector = endPoint - startPoint;
        GameObject cylinder = GameObject.CreatePrimitive(PrimitiveType.Cylinder);

        cylinder.name = "Beam";
        cylinder.transform.position = (startPoint + endPoint) / 2f;
        cylinder.transform.up = deltaVector;
        cylinder.tag = "LaserBeam";
        MeshRenderer meshRenderer = cylinder.GetComponent<MeshRenderer>();
        meshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        meshRenderer.receiveShadows = false;

        float realSizeX = cylinder.GetComponent<Renderer>().bounds.size.x;
        float realSizeZ = cylinder.GetComponent <Renderer>().bounds.size.z;

        Vector3 scale = cylinder.transform.localScale;
        scale.y = deltaVector.magnitude / 2f;
        scale.x = width * scale.x / realSizeX;
        scale.z = width * scale.z / realSizeZ;
        Debug.Log(scale);
        cylinder.transform.localScale = scale;
        return cylinder;
    }

    public bool CheckPath()
    {
        if (_laserIndices.Count < 2)
        {
            Ray ray = new Ray(startPosition, startDirection);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100, 1)) {
                return true;
            }
        } 
        else
        {
            for (int i = 0; i < _laserIndices.Count - 1; i++)
            {
                Vector3 firstPoint = _laserIndices[i];
                Vector3 secondPoint = _laserIndices[i + 1];

                Vector3 vector = secondPoint - firstPoint;

                Ray ray = new Ray(firstPoint, vector);
                RaycastHit hit;
                if (Physics.Raycast(ray,out hit, 100, 1))
                {
                    if (!_laserIndices.Contains(hit.point) && !hit.collider.gameObject.CompareTag("LaserBeam"))
                    {
                        Debug.Log("HIT");
                        return true;
                    }
                }
                else
                {
                    return true;
                }
            }
        }
        return false;
    }

    public void UpdateRays()
    {
        startPosition = laserParent.transform.position;
        startDirection = laserParent.transform.forward;
        if (CheckPath())
        {
            cylinderObjects.ForEach(obj => Object.Destroy(obj));
            _laserIndices.Clear();
            CastLaser(startPosition, startDirection);
        }
    }
}
