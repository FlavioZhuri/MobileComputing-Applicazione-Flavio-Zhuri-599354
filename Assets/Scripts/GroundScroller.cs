using UnityEngine;

// NOTE: PUT THIS ON THE CAMERA

public class GroundScroller : MonoBehaviour {
    public GameObject[] scrollingObjects;
    private Camera mainCamera;
    private Vector2 cameraBounds;
    public float horizontalOffset;
    public float scrollSpeed;

    void Start() {
        mainCamera = gameObject.GetComponent<Camera>();
        cameraBounds = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, mainCamera.transform.position.z));
        foreach (GameObject obj in scrollingObjects) {
            loadChildObjects(obj);
        }
    }

    void loadChildObjects(GameObject obj) {
        //we use a SpriteRenderer cause I'm fucking done with canvas related bullshit..
        float objectWidth = obj.GetComponent<SpriteRenderer>().bounds.size.x - horizontalOffset;

        //check how many times we need to spam the object so it fills the view
        int requiredFill = (int)Mathf.Ceil(cameraBounds.x * 2 / objectWidth); //cast to int cause apparantly Mathf.Ceil is not int here, and System.Math.Ceiling is also a double ???

        //get the object to clone
        GameObject clone = Instantiate(obj) as GameObject;

        //we create the required objects as children of the cloned object
        for (int i = 0; i <= requiredFill; i++) {
            GameObject c = Instantiate(clone) as GameObject;
            c.transform.SetParent(obj.transform);
            c.transform.position = new Vector3(objectWidth * i, obj.transform.position.y, obj.transform.position.z);
            c.name = obj.name + i;
        }

        //we destroy the clone variable
        Destroy(clone);
        //we destroy the SpriteRenderer of the parent (which is now acting as an empty container)
        Destroy(obj.GetComponent<SpriteRenderer>());
    }

    void reorderChildren(GameObject obj) {
        Transform[] children = obj.GetComponentsInChildren<Transform>();
        if (children.Length > 1) {
            GameObject frontObj = children[1].gameObject;
            GameObject backObj = children[children.Length - 1].gameObject;
            float halfObjectWidth = backObj.GetComponent<SpriteRenderer>().bounds.extents.x - horizontalOffset;
            if (transform.position.x + cameraBounds.x > backObj.transform.position.x + halfObjectWidth) {
                frontObj.transform.SetAsLastSibling();
                frontObj.transform.position = new Vector3(
                    backObj.transform.position.x + halfObjectWidth * 2,
                    backObj.transform.position.y,
                    backObj.transform.position.z
                );

            } else if (transform.position.x - cameraBounds.x < frontObj.transform.position.x - halfObjectWidth) {
                backObj.transform.SetAsFirstSibling();
                backObj.transform.position = new Vector3(
                    frontObj.transform.position.x - halfObjectWidth * 2,
                    frontObj.transform.position.y,
                    frontObj.transform.position.z
                );
            }
        }
    }

    //In Update we scroll the objects on screen
    void Update() {

        Vector3 velocity = Vector3.zero;

        foreach (GameObject obj in scrollingObjects) {
            Transform[] children = obj.GetComponentsInChildren<Transform>();
            if (children == null)
                return;

            foreach (Transform l in children) {
                Vector3 nextPos = l.transform.position + new Vector3(scrollSpeed, 0, 0);
                Vector3 interPos = Vector3.SmoothDamp(l.transform.position, nextPos, ref velocity, 0);
                l.transform.position = interPos;
            }
        }
    }

    //In LateUpdate we call reorder to fix invalid positioning and reorder the objects on screen instead of despawning them to save memory and cpu.
    void LateUpdate() {
        foreach (GameObject obj in scrollingObjects) {
            reorderChildren(obj);
        }
    }
}
