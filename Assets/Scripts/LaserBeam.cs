using System.Collections.Generic;
using UnityEngine;

/**
 * Thanks to https://samirgeorgy.medium.com/reflecting-laser-light-in-unity-6d8aa1a4daa2
 */

public class LaserBeam
{
    private float laserWidth;
    private Vector3 laserStartPosition;
    private Vector3 laserStartDirection;
    private List<float> waveLengths;
    private List<Vector3> laserIndices;
    private List<GameObject> cylinders;

    public LaserBeam(List<float> waveLengths, float laserWidth, Vector3 laserStartPosition, Vector3 laserStartDirection)
    {
        this.waveLengths = waveLengths;
        this.laserWidth = laserWidth;
        this.laserStartDirection = laserStartDirection;
        this.laserStartPosition = laserStartPosition;

        this.laserIndices = new List<Vector3>();
        this.cylinders = new List<GameObject>();
        
        CastLaser(this.laserIndices, this.laserStartPosition, this.laserStartDirection);
    }

    private void CastLaser(List<Vector3> indices, Vector3 position, Vector3 direction)
    {
        indices.Add(position);

        Ray ray = new Ray(position, direction);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100, 1))
        {
            CheckHit(indices, hit, direction);
        }
        else
        {
            indices.Add(ray.GetPoint(200));
            DrawLaser();
        }
    }

    private void CheckHit(List<Vector3> indices, RaycastHit hit, Vector3 direction)
    {
        Debug.Log(hit.point);
        if (hit.collider.CompareTag("Mirror"))
        {
            Vector3 pos = hit.point;
            Vector3 dir = Vector3.Reflect(direction, hit.normal);

            CastLaser(indices, pos, dir);
        }
        else
        {
            indices.Add(hit.point);
            DrawLaser();
        }
    }

    private void DrawLaser()
    {
        for (int i = 0; i < this.laserIndices.Count - 1; i++)
        {
            this.cylinders.Add(BuildCylinder(laserIndices[i], laserIndices[i + 1]));
        }
    }

    private GameObject BuildCylinder(Vector3 startPoint, Vector3 endPoint)
    {
        Vector3 deltaVector = endPoint - startPoint;
        GameObject cylinder = GameObject.CreatePrimitive(PrimitiveType.Cylinder);

        float realSizeX = cylinder.GetComponent<Renderer>().bounds.size.x;
        float realSizeZ = cylinder.GetComponent<Renderer>().bounds.size.z;

        Vector3 scale = cylinder.transform.localScale;
        scale.y = deltaVector.magnitude / 2f;
        scale.x = this.laserWidth * scale.x / realSizeX;
        scale.z = this.laserWidth * scale.z / realSizeZ;
        cylinder.transform.localScale = scale;

        cylinder.name = "Beam";
        cylinder.transform.position = (startPoint + endPoint) / 2f;
        cylinder.transform.up = deltaVector;
        cylinder.tag = "LaserBeam";
        MeshRenderer meshRenderer = cylinder.GetComponent<MeshRenderer>();
        meshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        meshRenderer.receiveShadows = false;

        return cylinder;
    }

    public void UpdateLaser()
    {
        if (CheckPath())
        {
            this.cylinders.ForEach(obj => Object.Destroy(obj));
            this.laserIndices.Clear();
            CastLaser(this.laserIndices, this.laserStartPosition, this.laserStartDirection);
        }
    }

    private bool CheckPath()
    {
        for (int i = 0; i < this.laserIndices.Count - 1; i++)
        {
            Vector3 firstPoint = laserIndices[i];
            Vector3 secondPoint = laserIndices[i + 1];

            Ray ray = new Ray(firstPoint, secondPoint - firstPoint);
            RaycastHit hit = new RaycastHit();
            Debug.DrawRay(firstPoint, secondPoint - firstPoint, Color.cyan);

            if (Physics.Raycast(ray, out hit, Vector3.Distance(firstPoint, secondPoint), 1))
            {
                if (!hit.collider.gameObject.CompareTag("LaserBeam"))
                {
                    return true;
                }
            }
            if (Physics.Raycast(ray, out hit, 100, 1))
            {
                if (!hit.collider.gameObject.CompareTag("LaserBeam"))
                {
                    return true;
                }
            }
        }

        /*List<Vector3> newPath = new List<Vector3>();
        CastLaser(newPath, this.laserStartPosition, this.laserStartDirection);

        foreach (Vector3 point in newPath)
        {
            if (!this.laserIndices.Contains(point))
            {
                return true;
            }
        }

        foreach (Vector3 point in this.laserIndices)
        {
            if (!newPath.Contains(point))
            {
                return true;
            }
        }*/

        return false;
    }
}
