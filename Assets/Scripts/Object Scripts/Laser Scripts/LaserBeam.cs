using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// A script to handle the behavior of a laser beam
/// </summary>
public class LaserBeam
{
    GameObject laserObject; // An object containing the line renderer
    LineRenderer laser; // The line renderer to render the laser beam
    List<Vector3> laserIndices = new List<Vector3>(); // A list of points, being the path of the laser beam
    GameObject parentObject; // The laser source, the laser beam originates from
    float laserWavenlength; // The wavelength of the laser beam

    /// <summary>
    /// The constructor of the laser beam class, instantiating all relevant variables
    /// </summary>
    /// <param name="parentObject">The laser source, the beam originates from</param>
    /// <param name="material">The material of the laser beam</param>
    /// <param name="wavelength">The wavelenght of the laser, indicating the color of the laser beam</param>
    public LaserBeam(GameObject parentObject, Material material, float wavelength)
    {
        this.parentObject = parentObject;
        this.laserWavenlength = wavelength;

        this.laser = new LineRenderer();
        this.laserObject = new GameObject();
        this.laserObject.name = "Laser Beam";

        this.laser = this.laserObject.AddComponent(typeof(LineRenderer)) as LineRenderer;
        this.laser.startWidth = 0.1f;
        this.laser.endWidth = 0.1f;
        this.laser.material = material;
        this.laser.startColor = LaserHelperFunctions.RgbFromWavelength(wavelength);
        this.laser.endColor = LaserHelperFunctions.RgbFromWavelength(wavelength);
    }

    /// <summary>
    /// The function casts the laser beam from a given point to a given direction
    /// </summary>
    /// <param name="pos">The position from which the beam is cast</param>
    /// <param name="dir">The direction of the beam</param>
    void CastRay(Vector3 pos, Vector3 dir)
    {
        laserIndices.Add(pos);

        Ray ray = new Ray(pos, dir);
        RaycastHit hit = new RaycastHit();
        LayerMask layers = new LayerMask();
        layers |= 1 << LayerMask.NameToLayer("Player");
        layers |= 1 << LayerMask.NameToLayer("ItemBarrier");

        if (Physics.Raycast(ray, out hit, 100, ~layers))
        {
            CheckHit(hit, dir);
        }
        else
        {
            laserIndices.Add(ray.GetPoint(200));
            UpdateLineRenderer();
        }
    }

    /// <summary>
    /// Handles the case of the laser beam hitting an object, handling each relevant object seperatly
    /// </summary>
    /// <param name="hitInfo">Information about the hit of the beam of an object</param>
    /// <param name="dir">The direction from which the laser hits the object</param>
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
        else if (tag == "LaserDetector")
        {
            HandleLaserDetector(hitInfo);
        }
        else if (tag == "LaserCheckpoint")
        {
            HandleLaserCheckpoint(hitInfo, dir);
        }
        else
        {
            laserIndices.Add(hitInfo.point);
            UpdateLineRenderer();
        }
    }

    /// <summary>
    /// Reflects the laser if it hits a mirror object in a physically correct way
    /// </summary>
    /// <param name="hitInfo">Information about the hit of the beam of an object</param>
    /// <param name="dir">The direction from which the laser hits the object</param>
    void HandleMirror(RaycastHit hitInfo, Vector3 dir)
    {
        Vector3 position = hitInfo.point;
        Vector3 direction = Vector3.Reflect(dir, hitInfo.normal);

        CastRay(position, direction);
    }

    /// <summary>
    /// Refracts the laser if it hits a lens object in a physically correct way
    /// </summary>
    /// <param name="hitInfo">Information about the hit of the beam of an object</param>
    /// <param name="dir">The direction from which the laser hits the object</param>
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

    /*
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
    */

    /// <summary>
    /// Handles the activation af a detector
    /// Informs a detector if a laser with the correct wavelength did hit it
    /// </summary>
    /// <param name="hitInfo">Information about the hit of the beam of an object</param>
    void HandleLaserDetector(RaycastHit hitInfo)
    {
        LaserDetector detector = hitInfo.collider.gameObject.GetComponent<LaserDetector>();

        if (detector != null && detector.acceptedWavelength == this.laserWavenlength)
        {
            hitInfo.transform.SendMessage("HitByLaser");
        }

        laserIndices.Add(hitInfo.point);
        UpdateLineRenderer();
    }

    /// <summary>
    /// Handles the activation af a checkpoint
    /// Informs a checkpoint if a laser with the correct wavelength did hit it
    /// </summary>
    /// <param name="hitInfo">Information about the hit of the beam of an object</param>
    /// <param name="dir">The direction from which the laser hits the object</param>
    void HandleLaserCheckpoint(RaycastHit hitInfo, Vector3 dir)
    {
        GameObject checkpointObject = hitInfo.collider.gameObject.transform.parent.gameObject;
        LaserCheckpoint checkpointScript = checkpointObject.GetComponent<LaserCheckpoint>();

        if (checkpointScript != null && checkpointScript.acceptedWavelength == this.laserWavenlength)
        {
            checkpointObject.transform.SendMessage("HitByLaser");
        }
        CastRay(hitInfo.point + dir.normalized * 0.01f, dir);
    }

    /// <summary>
    /// Redraws the laser by updating the line renderer
    /// </summary>
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

    /// <summary>
    /// The function recalculates the path of the laser beam
    /// </summary>
    public void UpdateLaser()
    {
        this.laserIndices.Clear();
        CastRay(this.parentObject.transform.position, -this.parentObject.transform.right);
    }
}
