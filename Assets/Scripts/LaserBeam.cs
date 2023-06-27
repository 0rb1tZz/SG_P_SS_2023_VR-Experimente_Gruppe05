using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeam
{

    GameObject laserObject;
    LineRenderer laser;
    List<Vector3> laserIndices = new List<Vector3>();
    GameObject parentObject;

    public LaserBeam(GameObject parentObject, Material material)
    {
        this.parentObject = parentObject;

        this.laser = new LineRenderer();
        this.laserObject = new GameObject();
        this.laserObject.name = "Laser Beam";

        this.laser = this.laserObject.AddComponent(typeof(LineRenderer)) as LineRenderer;
        this.laser.startWidth = 0.1f;
        this.laser.endWidth = 0.1f;
        this.laser.material = material;
        this.laser.startColor = Color.red;
        this.laser.endColor = Color.red;

        CastRay(this.parentObject.transform.position, this.parentObject.transform.forward);
    }

    void CastRay(Vector3 pos, Vector3 dir)
    {
        laserIndices.Add(pos);

        Ray ray = new Ray(pos, dir);
        RaycastHit hit = new RaycastHit();

        if (Physics.Raycast(ray, out hit, 100, 1))
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
        if (hitInfo.collider.gameObject.tag == "Mirror")
        {
            Vector3 position = hitInfo.point;
            Vector3 direction = Vector3.Reflect(dir, hitInfo.normal);

            CastRay(position, direction);
        } else if (hitInfo.collider.gameObject.tag != "Mirror")
        {
            laserIndices.Add(hitInfo.point);
            UpdateLineRenderer();
        }
        else
        {
            CastRay(hitInfo.point, dir);
        }
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
        CastRay(this.parentObject.transform.position, this.parentObject.transform.forward);
    }

}
