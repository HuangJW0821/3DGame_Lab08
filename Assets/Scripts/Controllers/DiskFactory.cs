using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MyException : System.Exception{
    public MyException(){}
    public MyException(string message) : base(message) {}
}

public class DiskAttributes : MonoBehaviour{
    //public GameObject gameobj;
    public int score;
    public float speedX;
    public float speedY;
}

public class DiskFactory : MonoBehaviour{
    
    List<GameObject> used;
    List<GameObject> free;
    System.Random rand;

    // Start is called before the first frame update
    void Start(){
        used = new List<GameObject>();
        free = new List<GameObject>();
        rand = new System.Random(); 
    }

    // Update is called once per frame
    void Update(){}

    public GameObject GetDisk(int round){
        GameObject disk;

        int score = rand.Next(1,4);
        string prefab_name = "Prefabs/UFO_Color" + score;

        if (free.Count != 0) {
            disk = free[0];
            free.Remove(disk);
        }else{
            disk = GameObject.Instantiate(Resources.Load(prefab_name, typeof(GameObject))) as GameObject;
            disk.AddComponent<DiskAttributes>();
            disk.AddComponent<Rigidbody>();
            // disk.AddComponent<Constant Force>();
        }
        
        // disk.transform.localEulerAngles = new Vector3(-rand.Next(20,40),0,0);

        DiskAttributes attri = disk.GetComponent<DiskAttributes>();
        attri.score = score;
        attri.speedX = (rand.Next(1,20) + attri.score + round) * 0.2f;
        
        int direction = rand.Next(1,5);
        float zDistance = rand.Next(5, 20)*1.0f; // 修改后的 z 距离，可以根据需要调整更远或更近

        if (direction == 1) {
            disk.transform.Translate(Camera.main.ScreenToWorldPoint(new Vector3(0, Camera.main.pixelHeight * 1.5f, zDistance)));
            disk.GetComponent<Rigidbody>().AddForce(20, 0, 10);
        }
        else if (direction == 2) {
            disk.transform.Translate(Camera.main.ScreenToWorldPoint(new Vector3(0, Camera.main.pixelHeight * 0f, zDistance)));
            disk.GetComponent<Rigidbody>().AddForce(20, 20, 10);
            attri.speedX *= -1;
        }
        else if (direction == 3) {
            disk.transform.Translate(Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth, Camera.main.pixelHeight * 1.5f, zDistance)));
            disk.GetComponent<Rigidbody>().AddForce(-30, 0, 10);
        }
        else if (direction == 4) {
            disk.transform.Translate(Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth, Camera.main.pixelHeight * 0f, zDistance)));
            disk.GetComponent<Rigidbody>().AddForce(-20, 0, 10);
            disk.GetComponent<Rigidbody>().AddForce(0, 20, 0, ForceMode.Impulse);
            attri.speedX *= -1;
        }

        used.Add(disk);
        disk.SetActive(true);
        Debug.Log("generate disk");

        return disk;
    }

    public void FreeDisk(GameObject disk) {
        disk.SetActive(false);
        // //将位置和大小恢复到预制，这点很重要！
        // disk.transform.position = new Vector3(0, 0,0);
        // disk.transform.localScale = new Vector3(2f,0.1f,2f);
        if (!used.Contains(disk)) {
            throw new MyException("Try to remove a item from a list which doesn't contain it.");
        }
        Debug.Log("free disk");
        used.Remove(disk);
        free.Add(disk);
    }
}