using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using LitJson;

using ViconPegasusSDK.DotNET;


namespace TestUnityVicon
{

    public class Program : MonoBehaviour
    {

        public bool drawBones;
        public string SubjectName;
        public bool distortionEnabled;
        public DistortionsController distortionsController;
        public int participantId;
        private Vector3 curPos;
        private Vector3 prevPos;
        private string pose;
        private string translate;
        public int trialId;
        public bool drawOnce;
        public bool saveTrackingData;
        List<TrackingData> trackingData;
        public float offset;
        public String HostName; 

        RigidController rigidController;

        ViconPegasusSDK.DotNET.Client MyClient = new ViconPegasusSDK.DotNET.Client();

        public Program()
        {
        }

        void Start()
        {
            offset = 0.001f;

            if (saveTrackingData)
            {
                trackingData = new List<TrackingData>();
                participantId = GameObject.Find("ExperimentManager").GetComponent<ExperimentController>().participantId;
            }


            if (gameObject.GetComponent<RigidController>() != null)
            {
                rigidController = gameObject.GetComponent<RigidController>();
            }

            print("Starting...");

            // Make a new client
            Output_GetVersion OGV = MyClient.GetVersion();
            print("GetVersion Major: " + OGV.Major);

            // Connect to a server
            //string HostName = "172.21.101.12:801";
            int noAttempts = 0;

            print("Connecting to " + HostName + "...");
            while (!MyClient.IsConnected().Connected)
            {
                // Direct connection
                Output_Connect OC = MyClient.Connect(HostName);
                print("Connect result: " + OC.Result);

                noAttempts += 1;
                if (noAttempts == 3)
                    break;
                System.Threading.Thread.Sleep(200);
            }

            MyClient.EnableSegmentData();

            // get a frame from the data stream so we can inspect the list of subjects
            //MyClient.GetFrame();

            ////Output_GetSubjectCount OGSC = MyClient.GetSubjectCount ();
            ////print("GetSubjectCount: "+ OGSC.Result + "|" + OGSC.SubjectCount);

            ////// the first subjects in the data stream will be the original subjects unmodified by pegasus
            ////Output_GetSubjectName OGSN = MyClient.GetSubjectName(OGSC.SubjectCount - 1);
            ////print("GetSubjectName: "+ OGSN.Result + "|" + +OGSN.SubjectName);

            ////SubjectName = OGSN.SubjectName;

            //// get the position of the root and point the camera at it
            //Output_GetSubjectRootSegmentName OGSRSN = MyClient.GetSubjectRootSegmentName(SubjectName);
            //Output_GetSegmentGlobalTranslation RootPos = MyClient.GetSegmentGlobalTranslation(SubjectName, OGSRSN.SegmentName);
            //transform.localPosition = new Vector3(0.001f * -(float)RootPos.Translation[0], 0.001f * (float)RootPos.Translation[1], 0.001f * (float)RootPos.Translation[2]);

            if (distortionEnabled)
            {
                distortionsController = gameObject.GetComponent<DistortionsController>();
                trialId = distortionsController.trialId;
            }
        }

        void LateUpdate()
        {
            // save tracking data if no more trials or create new entry
            if (saveTrackingData && distortionsController.numTrialsCompleted == distortionsController.numTrials)
            {
                SaveTrackingDataToFile();
                saveTrackingData = false;
            } else if (saveTrackingData)
            {
                TrackingData temp = new TrackingData(DateTime.Now);
                trackingData.Add(temp);
            }


            MyClient.GetFrame();

            Output_GetSubjectRootSegmentName OGSRSN = MyClient.GetSubjectRootSegmentName(SubjectName);
            Transform Root = transform.FindChild(OGSRSN.SegmentName);

            if (distortionsController.trialId != trialId)
            {
                trialId = distortionsController.trialId;

                if (rigidController)
                {
                    rigidController.Reset();
                }


                if (gameObject.GetComponent<Rigidbody>() == null)
                {


                }
                else if (distortionsController.illusions[distortionsController.trialId].virtualPropPos != Vector3.zero)
                {
                    
                    gameObject.GetComponent<RigidController>().enabled = true;


                    Transform temp = transform.Find("b");
                    temp.localPosition = distortionsController.illusions[distortionsController.trialId].virtualPropPos;
                    temp.localRotation = distortionsController.illusions[distortionsController.trialId].virtualPropRot;

                    rigidController.enabled = true;

                } else if (rigidController != null)
                {
                    rigidController.enabled = false;
                }
           

            }


            if (trialId != -1)
            {
                
                ApplyBoneTransform(Root);

                if (Input.GetKey(KeyCode.F11) && SubjectName == "4_12_2017" && distortionsController.filename == "/TrialData/trialdata2.txt")
                {
                    pose = "";
                    translate = "";
                    SavePoseToString(Root);
                    print(pose);
                    print(translate);
                }

                if (distortionEnabled && distortionsController.needUpdate)
                {
                    distortionsController.needUpdate = false;
                }
            }

          

        }


        private void SavePoseToString(Transform Bone)
        {
            String s;
            Output_GetSegmentLocalRotationQuaternion ORot = MyClient.GetSegmentLocalRotationQuaternion(SubjectName, Bone.gameObject.name);
            if (ORot.Result == Result.Success)
            {
                //Quaternion rot = new Quaternion(-(float)ORot.Rotation[0], (float)ORot.Rotation[1], (float)ORot.Rotation[2], -(float)ORot.Rotation[3]);
                Quaternion rot = Bone.localRotation;
                Vector3 eul = rot.eulerAngles;
                s = "\"" + Bone.gameObject.name + ":" + eul[0].ToString() + ":" +  eul[0].ToString() + ":" + eul[1].ToString() + ":" + eul[1].ToString() + ":" + eul[2].ToString() + ":" + eul[2].ToString() + "\",";
                pose = pose + s;
            }


            Output_GetSegmentLocalTranslation OTran = MyClient.GetSegmentLocalTranslation(SubjectName, Bone.gameObject.name);
            if (OTran.Result == Result.Success)
            {
                Vector3 trans = Bone.localPosition;
                //Vector3 trans = new Vector3(0.001f * -(float)OTran.Translation[0], 0.001f * (float)OTran.Translation[1], 0.001f * (float)OTran.Translation[2]);
                s = "\"" + Bone.gameObject.name + ":" + trans[0].ToString() + ":" + trans[1].ToString() + ":" + trans[2].ToString() + "\",";
                translate = translate + s;
            }


            // recurse through children
            for (int iChild = 0; iChild < Bone.childCount; iChild++)
            {
                SavePoseToString(Bone.GetChild(iChild));
            }
        }

        private void ApplyBoneTransform(Transform Bone)
        {
            bool rotated = false;
            bool translated = false;


            if (distortionsController.illusions[trialId].hide.Contains(Bone.gameObject.name))
            {
                Bone.gameObject.SetActive(false);
            } else if (distortionsController.illusions[distortionsController.trialId].virtualPropPos != Vector3.zero)
            {

            }
            else if (distortionsController.illusions[distortionsController.trialId].virtualPropPos == Vector3.zero) 
            {
                if (distortionsController.illusions[trialId].hide.Count > 0)
                {
                    Bone.gameObject.SetActive(true);
                }

                if (Bone.gameObject.GetComponent<Rigidbody>() == null)
                {

                } else if (distortionsController.illusions[distortionsController.trialId].solid)
                {
                    Bone.gameObject.GetComponent<Rigidbody>().isKinematic = false;
                }
                else
                {
                    Bone.gameObject.GetComponent<Rigidbody>().isKinematic = true;
                }


                // update the bone transform from the data stream
                Output_GetSegmentLocalRotationQuaternion ORot = MyClient.GetSegmentLocalRotationQuaternion(SubjectName, Bone.gameObject.name);
                if (ORot.Result == Result.Success)
                {
                    //Bone.localPosition = new Vector3(1.0f, 1.0f, 1.0f);

                    if (distortionEnabled && IsKink() && !distortionsController.illusions[distortionsController.trialId].fixedPos)
                    {
                        Bone.localEulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
                        rotated = true;
                    }


                    if (distortionEnabled && distortionsController.illusions[distortionsController.trialId].fixedPos)
                    {
                        Bone.localEulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
                        rotated = true;
                    }

                    if (distortionEnabled && distortionsController.illusions[trialId].parent.ContainsKey(Bone.gameObject.name))
                    {
                        string parent = distortionsController.illusions[trialId].parent[Bone.gameObject.name];
                        rotated = true;
                    }

                    if (distortionEnabled && distortionsController.illusions[trialId].parent.ContainsKey(Bone.gameObject.name))
                    {
                        string parent = distortionsController.illusions[trialId].parent[Bone.gameObject.name];
                        Output_GetSegmentLocalRotationQuaternion OParentRot = MyClient.GetSegmentLocalRotationQuaternion(SubjectName, parent);
                        Quaternion parentRot = new Quaternion(-(float)OParentRot.Rotation[0], (float)OParentRot.Rotation[1], (float)OParentRot.Rotation[2], -(float)OParentRot.Rotation[3]);
                        Bone.localRotation = parentRot;


                        rotated = true;
                    }


                    if (distortionEnabled && distortionsController.illusions[distortionsController.trialId].dof.ContainsKey(Bone.gameObject.name))
                    {
                        Quaternion rot = new Quaternion(-(float)ORot.Rotation[0], (float)ORot.Rotation[1], (float)ORot.Rotation[2], -(float)ORot.Rotation[3]);
                        Vector3 eulerRotation = rot.eulerAngles;
                        List<float> restricted = distortionsController.illusions[distortionsController.trialId].dof[Bone.gameObject.name];
                        Bone.localEulerAngles = new Vector3(ConstrainValue(eulerRotation[0], restricted[0], restricted[1]),
                            ConstrainValue(eulerRotation[1], restricted[2], restricted[3]),
                            ConstrainValue(eulerRotation[2], restricted[4], restricted[5]));
                        rotated = true;
                    }

                    if (!rotated)
                    {
                        Bone.localRotation = new Quaternion(-(float)ORot.Rotation[0], (float)ORot.Rotation[1], (float)ORot.Rotation[2], -(float)ORot.Rotation[3]);
                        rotated = true;
                    }
                }

                Output_GetSegmentLocalTranslation OTran = MyClient.GetSegmentLocalTranslation(SubjectName, Bone.gameObject.name);
                if (OTran.Result == Result.Success)
                {

                    prevPos = curPos;
                    curPos = new Vector3(offset * -(float)OTran.Translation[0], offset * (float)OTran.Translation[1], offset * (float)OTran.Translation[2]);

                    // Bone.localPosition = new Vector3(-0.001f * (float)OTran.Translation[0], 0.001f * (float)OTran.Translation[1], 0.001f * (float)OTran.Translation[2]);

                 
                    if (distortionEnabled && distortionsController.illusions[distortionsController.trialId].originOffset && (Bone.gameObject.name == "Arm" || Bone.gameObject.name == "b"))
                    {
                        Vector3 temp = curPos;
                        temp[0] = temp[0] + distortionsController.illusions[distortionsController.trialId].origin[0];
                        temp[1] = temp[1] + distortionsController.illusions[distortionsController.trialId].origin[1];
                        temp[2] = temp[2] + distortionsController.illusions[distortionsController.trialId].origin[2];

                        
                        Bone.localPosition = temp;
                        translated = true;
                    }

                    if (distortionEnabled && Bone.gameObject.name == "Arm" && distortionsController.illusions[distortionsController.trialId].vel != new Vector3(1.0f, 1.0f, 1.0f))
                    {

                        var bonePos = Bone.localPosition;
                        bonePos[0] = curPos[0] * distortionsController.illusions[distortionsController.trialId].vel[0];
                        bonePos[1] = curPos[1] * distortionsController.illusions[distortionsController.trialId].vel[1];
                        bonePos[2] = curPos[2] * distortionsController.illusions[distortionsController.trialId].vel[2];

                        Bone.localPosition = bonePos;
                    

                        translated = true;
                    }

                    //if (distortionEnabled && distortionsController.illusions[distortionsController.trialId].fixedPos && Bone.gameObject.name == "Arm")
                    //{
                    //    Bone.localPosition = distortionsController.illusions[distortionsController.trialId].pos;
                    //    translated = true;
                    //}



                    if (!translated && Bone.gameObject.name == "b")
                    {
                       // print("here");
                        //curPos[2] = curPos[2] + distortionsController.illusions[distortionsController.trialId].propSize[2] / 2.0f;
                        //Bone.localPosition = curPos;
                    } else if  (!translated)
                    {
                        Debug.Log("here");


                        Bone.localPosition = curPos;
                        translated = true;
                    }
                }

                if (distortionEnabled && distortionsController.illusions[distortionsController.trialId].size.ContainsKey(Bone.gameObject.name))
                {
                    Bone.localPosition = distortionsController.illusions[distortionsController.trialId].size[Bone.gameObject.name];
                }

                if (distortionEnabled && distortionsController.illusions[distortionsController.trialId].prop.ContainsKey(Bone.gameObject.name))
                {
                    Vector3 newProps = distortionsController.illusions[distortionsController.trialId].prop[Bone.gameObject.name];
                    Vector3 curScale = Bone.localPosition;
                    curScale[0] = curScale[0] * newProps[0];
                    curScale[1] = curScale[1] * newProps[1];
                    curScale[2] = curScale[2] * newProps[2];
                    Bone.localPosition = curScale;
                }

                if (distortionEnabled && distortionsController.illusions[distortionsController.trialId].propSize != new Vector3(0.0f, 0.0f, 0.0f) && Bone.gameObject.name == "bc")
                {

                    Bone.localScale = distortionsController.illusions[distortionsController.trialId].propSize;
                }


                if (drawBones)
                {
                    CreateCylinder(Bone);
                }

                if (saveTrackingData && translated && rotated)
                {
                    trackingData[trackingData.Count - 1].AddData(Bone.gameObject.name, Bone.transform.localEulerAngles, Bone.transform.localPosition, Bone.transform.eulerAngles, Bone.transform.position);
                }

                // recurse through children
                for (int iChild = 0; iChild < Bone.childCount; iChild++)
                {
                    ApplyBoneTransform(Bone.GetChild(iChild));


                }

            }
                


        }

        float ConstrainValue(float val, float min, float max)
        {
            if (min == -360.0f && max == 360.0f)
            {
                return val;
            }
            else
            {

                if (val > max)
                {
                    return max;
                }
                else if (val < min)
                {
                    return min;
                }
                else
                {
                    return val;
                }

            }

        }

        bool IsKink()
        {
            return false;
            //if (!distortionsController.illusions[distortionsController.trialId].kink.ContainsKey("Hand"))
            //{
            //    return false;
            //}

            //Output_GetSegmentLocalRotationQuaternion ORot = MyClient.GetSegmentLocalRotationQuaternion(SubjectName, "Hand");
            //if (ORot.Result != Result.Success)
            //{
            //    return false;
            //}

            //Quaternion rot = new Quaternion(-(float)ORot.Rotation[0], (float)ORot.Rotation[1], (float)ORot.Rotation[2], -(float)ORot.Rotation[3]);
            //Vector3 eulerRotation = rot.eulerAngles;
            //List<float> restricted = distortionsController.illusions[distortionsController.trialId].kink["Hand"];
            
            //if (eulerRotation[0] != ConstrainValue(eulerRotation[0], restricted[0], restricted[1]) || 
            //    eulerRotation[1] != ConstrainValue(eulerRotation[1], restricted[2], restricted[3]) ||
            //    eulerRotation[2] != ConstrainValue(eulerRotation[2], restricted[4], restricted[5]))
            //{
            //    return false;
            //}

            //return true;

        }

        void CreateCylinder(Transform bone)
        {
            Transform segObj = bone.parent.FindChild("Cylinder" + bone.gameObject.name);

            if (segObj)
            {
                Vector3 newScale = segObj.transform.localScale;
                Vector3 newPos = segObj.transform.localPosition;
                newPos.y = Vector3.Distance(new Vector3(0.0f, 0.0f, 0.0f), bone.localPosition) / 2.0f;
                newScale.y = Vector3.Distance(new Vector3(0.0f, 0.0f, 0.0f), bone.localPosition) / 2.0f;
                segObj.transform.localScale = newScale;
                segObj.transform.localPosition = newPos;
            }
            else
            {
            }

        }



        void SaveTrackingDataToFile()
        {
            JsonData json = JsonMapper.ToJson(trackingData);
            File.WriteAllText(Application.dataPath + "/TrialData/tracking" + participantId + ".txt", json.ToString());
        }

    }
}


