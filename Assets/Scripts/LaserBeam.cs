using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeam
{

    Vector3 pos, dir;

    GameObject laserObject;
    LineRenderer laser;
    List<Vector3> firstLaserIndices = new List<Vector3>();

    public LaserBeam(Vector3 pos, Vector3 dir, Material material)
    {
        /*this.laser = new LineRenderer();
        this.laserObject = new GameObject();
        this.laserObject.name = "Laser Beam";*/
        this.pos = pos;
        this.dir = dir;

        /*this.laser = this.laserObject.AddComponent(typeof(LineRenderer)) as LineRenderer;
        this.laser.startWidth = 0.1f;
        this.laser.endWidth = 0.1f;
        this.laser.material = material;
        this.laser.startColor = Color.red;
        this.laser.startColor = Color.red;*/

        CastRay(pos, dir, this.firstLaserIndices);
    }

    void CastRay(Vector3 pos, Vector3 dir, List<Vector3> laserIndices)
    {
        laserIndices.Add(pos);

        Ray ray = new Ray(pos, dir);
        RaycastHit hit = new RaycastHit();

        if (Physics.Raycast(ray, out hit, 100, 1))
        {
            CheckHit(hit, dir, laserIndices);
        }
        else
        {
            laserIndices.Add(ray.GetPoint(200));
            //UpdateLaser();
        }
    }

    void CheckHit(RaycastHit hitInfo, Vector3 dir, List<Vector3> laserIndices)
    {
        if (hitInfo.collider.gameObject.tag == "Mirror")
        {
            Vector3 position = hitInfo.point;
            Vector3 direction = Vector3.Reflect(dir, hitInfo.normal);

            CastRay(position, direction, laserIndices);
        } else if (hitInfo.collider.gameObject.tag != "Mirror")
        {
            laserIndices.Add(hitInfo.point);
            //UpdateLaser();
        }
        else
        {
            CastRay(hitInfo.point, dir, laserIndices);
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
        //scale.x = this.laserWidth * scale.x / realSizeX;
        //scale.z = this.laserWidth * scale.z / realSizeZ;
        scale.x = 0.1f * scale.x / realSizeX;
        scale.z = 0.1f * scale.z / realSizeZ;
        cylinder.transform.localScale = scale;

        cylinder.name = "Beam";
        cylinder.transform.position = (startPoint + endPoint) / 2f;
        cylinder.transform.up = deltaVector;
        cylinder.tag = "LaserBeam";
        MeshRenderer meshRenderer = cylinder.GetComponent<MeshRenderer>();
        meshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        meshRenderer.receiveShadows = false;

        //cylinder.transform.SetParent(parentObject.transform);

        return cylinder;
    }

    public void UpdateLaser()
    {
        int count = 0;
        laser.positionCount = firstLaserIndices.Count;

        foreach (Vector3 vector in firstLaserIndices)
        {
            laser.SetPosition(count, vector);
            count++;
        }
    }

    public void UpdateRays()
    {
        List<Vector3> checkList = new List<Vector3>();
        CastRay(pos, dir, checkList);
        if (!testLists(firstLaserIndices, checkList))
        {
            firstLaserIndices = checkList;
            Object.Destroy(GameObject.Find("Beam"));
            for (int i = 0; i < firstLaserIndices.Count - 1; i++)
            {
                BuildCylinder(firstLaserIndices[i], firstLaserIndices[i + 1]);
            }
        }
    }

    bool testLists(List<Vector3> list1, List<Vector3> list2)
    {
        if (list1.Count != list2.Count) return false;

        for (int i = 0; i < list1.Count; i++)
        {
            if (list1[i] != list2[i]) return false;
        }

        return true;
    }

}
