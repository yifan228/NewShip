using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Scripts
{
    public class ObjTrackerSystem : MonoBehaviour
    {
        [SerializeField] Transform player;
         List<ObjTracker> objTrackers = new List<ObjTracker>();
        [SerializeField] TrackerUI trackerTagUITemplate;
        [SerializeField] Canvas UIcanvas;
        Dictionary<ObjTracker, TrackerUI> objDict = new Dictionary<ObjTracker, TrackerUI>();
        // Use this for initialization
        void Start()
        {
            var objs = (ObjTracker[])Resources.FindObjectsOfTypeAll(typeof(ObjTracker));
            for (int i = 0; i < objs.Length; i++) {
                Debug.Log($"{objs[i].gameObject.activeSelf} is obj");
                if (!objs[i].gameObject.activeInHierarchy) continue;
                objDict.Add(objs[i], Instantiate(trackerTagUITemplate, UIcanvas.transform));
                //objDict[objTrackers[i]] = Instantiate(trackerTagUITemplate, UIcanvas.transform);
                objDict[objs[i]].SetUp(objs[i].Icon);
                objTrackers.Add(objs[i]);
            }
        }

        // Update is called once per frame
        void Update()
        {
            UpdateFrame();
        }
        void UpdateFrame()
        {
            for (int i = 0; i < objTrackers.Count; i++)
            {
                objDict[objTrackers[i]].UpdateDistence(
                    objTrackers[i].transform,
                    player.InverseTransformDirection(objTrackers[i].transform.position),
                    Vector2.Distance(objTrackers[i].transform.position, player.position)
                );
            }
        }
    }
}