using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class LaserBeam
{

    GameObject laserObject;
    LineRenderer laser;
    List<Vector3> laserIndices = new List<Vector3>();
    GameObject parentObject;
    float laserWavenlength;
    int checkpoints;
    List<GameObject> checkpointList = new List<GameObject>();

    public LaserBeam(GameObject parentObject, Material material, float wavelength, int checkpoints)
    {
        this.parentObject = parentObject;
        this.laserWavenlength = wavelength;
        this.checkpoints = checkpoints;

        this.laser = new LineRenderer();
        this.laserObject = new GameObject();
        this.laserObject.name = "Laser Beam";

        this.laser = this.laserObject.AddComponent(typeof(LineRenderer)) as LineRenderer;
        this.laser.startWidth = 0.1f;
        this.laser.endWidth = 0.1f;
        this.laser.material = material;
        this.laser.startColor = LaserHelperFunctions.RgbFromWavelength(wavelength);
        this.laser.endColor = LaserHelperFunctions.RgbFromWavelength(wavelength);

        CastRay(this.parentObject.transform.position, this.parentObject.transform.forward);
    }

    void CastRay(Vector3 pos, Vector3 dir)
    {
        laserIndices.Add(pos);

        Ray ray = new Ray(pos, dir);
        RaycastHit hit = new RaycastHit();

        if (Physics.Raycast(ray, out hit, 100))
        {
            CheckHit(hit, dir);
        }
        else
        {
            laserIndices.Add(ray.GetPoint(200));
            UpdateLineRenderer();
        }
    }

    void CheckHit(RaycastHit hitInfo, Vector3 dir)
    {
        string tag = hitInfo.collider.tag;
        if (tag == "Mirror")
        {
            HandleMirror(hitInfo, dir);
        }
        else if (tag == "Lens")
        {
            HandleLens(hitInfo, dir);
        }
        else if (tag == "Prism")
        {
            HandlePrism(hitInfo, dir);
        }
        else if (tag == "LaserDetector")
        {
            HandleLaserDetector(hitInfo);
        }
        else if (tag == "LaserCheckpoint")
        {
            HandheldLaserCheckpoint(hitInfo, dir);
        }
        else
        {
            laserIndices.Add(hitInfo.point);
            UpdateLineRenderer();
        }
    }

    void HandleMirror(RaycastHit hitInfo, Vector3 dir)
    {
        Vector3 position = hitInfo.point;
        Vector3 direction = Vector3.Reflect(dir, hitInfo.normal);

        CastRay(position, direction);
    }

    void HandleLens(RaycastHit hitInfo, Vector3 dir)
    {
        laserIndices.Add(hitInfo.point);
        GameObject lens = hitInfo.collider.gameObject;

        float focalLength = lens.GetComponent<Lens>().focalLength;
        Vector3 focalLengthVector = new Vector3(focalLength, 0, 0);

        if (lens.GetComponent<Lens>().isConvex)
        {
            focalLengthVector = Vector3.RotateTowards(focalLengthVector, lens.transform.right, 2 * Mathf.PI, 0);
        }
        else
        {
            focalLengthVector = Vector3.RotateTowards(focalLengthVector, lens.transform.up, 2 * Mathf.PI, 0);
        }


        if (Vector3.Dot(dir, focalLengthVector) < 0)
        {
            focalLengthVector *= -1;
        }

        if (!lens.GetComponent<Lens>().isConvex)
        {
            focalLengthVector *= -1;
        }

        Vector3 focalPoint = lens.transform.position + focalLengthVector;
        Vector3 newDirection;

        if (lens.GetComponent<Lens>().isConvex)
        {
            newDirection = focalPoint - hitInfo.point;
        }
        else
        {
            newDirection = hitInfo.point - focalPoint;
        }


        CastRay(hitInfo.point + newDirection * 0.05f, newDirection);
    }

    void HandlePrism(RaycastHit hitInfo, Vector3 dir)
    {
        float n1 = 1.000293f;
        float n2 = 1.52f;

        laserIndices.Add(hitInfo.point);

        Vector3 refractedRay = RefractRay(n1, n2, hitInfo.normal, dir);
        Ray ray = new Ray(hitInfo.point, refractedRay);
        RaycastHit hit = new RaycastHit();

        if (Physics.Raycast(ray, out hit, 100) && hit.collider.gameObject.tag == "Prism")
        {
            laserIndices.Add(hit.point);
            Vector3 refractedRay2 = RefractRay(n2, n1, hit.normal, refractedRay);
            CastRay(hit.point + refractedRay2 * 0.1f, refractedRay2);
        }
        else
        {
            Debug.DrawRay(hitInfo.point, refractedRay);
        }
    }

    Vector3 RefractRay(float n1, float n2, Vector3 norm, Vector3 dir)
    {
        dir.Normalize();

        Vector3 refractedVector = (n1 / n2 * Vector3.Cross(norm, Vector3.Cross(-norm, dir)) - norm * Mathf.Sqrt(1 - Vector3.Dot(Vector3.Cross(norm, dir) * (n1 / n2 * n1 / n2), Vector3.Cross(norm, dir)))).normalized;

        return refractedVector;
    }

    void HandleLaserDetector(RaycastHit hitInfo)
    {
        LaserDetector detector = hitInfo.collider.gameObject.GetComponent<LaserDetector>();

        if (detector != null && detector.acceptedWavelength == this.laserWavenlength && (checkpoints == 0 || checkpointList.Count == checkpoints))
        {
            hitInfo.transform.SendMessage("HitByLaser");
        }

        laserIndices.Add(hitInfo.point);
        UpdateLineRenderer();
    }

    void HandheldLaserCheckpoint(RaycastHit hitInfo, Vector3 dir)
    {
        GameObject checkpointObject = hitInfo.collider.gameObject.transform.parent.gameObject;
        LaserCheckpoint checkpointScript = checkpointObject.GetComponent<LaserCheckpoint>();

        if (checkpointScript != null && checkpointScript.acceptedWavelength == this.laserWavenlength)
        {
            checkpointObject.transform.SendMessage("HitByLaser");
            if (!checkpointList.Contains(checkpointObject))
            {
                checkpointList.Add(checkpointObject);
            }
        }
        CastRay(hitInfo.point + dir.normalized * 0.01f, dir);
    }

    void UpdateLineRenderer()
    {
        int count = 0;
        laser.positionCount = laserIndices.Count;

        foreach (Vector3 vector in laserIndices)
        {
            laser.SetPosition(count, vector);
            count++;
        }
    }

    public void UpdateLaser()
    {
        this.laserIndices.Clear();
        this.checkpointList.Clear();
        CastRay(this.parentObject.transform.position, this.parentObject.transform.forward);
    }
}
