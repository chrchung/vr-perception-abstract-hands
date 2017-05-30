using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using ViconPegasusSDK.DotNET;


namespace TestUnityVicon
{

    public class Program : MonoBehaviour
    {

        public bool drawBones;
        public string SubjectName;
        public bool distortionEnabled;
        public DistortionsController distortionsController;

        private Vector3 curPos;
        private Vector3 prevPos;

        ViconPegasusSDK.DotNET.Client MyClient = new ViconPegasusSDK.DotNET.Client();

        public Program()
        {
        }

        void Start()
        {
            print("Starting...");

            // Make a new client
            Output_GetVersion OGV = MyClient.GetVersion();
            print("GetVersion Major: " + OGV.Major);

            // Connect to a server
            string HostName = "172.21.12.101:801";
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
            }
        }

        void LateUpdate()
        {
            MyClient.GetFrame();

            Output_GetSubjectRootSegmentName OGSRSN = MyClient.GetSubjectRootSegmentName(SubjectName);
            Transform Root = transform.FindChild(OGSRSN.SegmentName);

            ApplyBoneTransform(Root);

            if (distortionEnabled && distortionsController.needUpdate)
            {
                distortionsController.needUpdate = false;
            }
        }

        private void ApplyBoneTransform(Transform Bone)
        {


            // update the bone transform from the data stream
            Output_GetSegmentLocalRotationQuaternion ORot = MyClient.GetSegmentLocalRotationQuaternion(SubjectName, Bone.gameObject.name);
            if (ORot.Result == Result.Success)
            {
                //Bone.localPosition = new Vector3(1.0f, 1.0f, 1.0f);

                if (distortionEnabled && IsKink() && !distortionsController.illusions[distortionsController.trialId].fixedPos)
                {
                    Bone.localEulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
                }
                else if (distortionEnabled && distortionsController.illusions[distortionsController.trialId].dof.ContainsKey(Bone.gameObject.name))
                {
                    Quaternion rot = new Quaternion(-(float)ORot.Rotation[0], (float)ORot.Rotation[1], (float)ORot.Rotation[2], -(float)ORot.Rotation[3]);
                    Vector3 eulerRotation = rot.eulerAngles;
                    List<float> restricted = distortionsController.illusions[distortionsController.trialId].dof[Bone.gameObject.name];
                    Bone.localEulerAngles = new Vector3(ConstrainValue(eulerRotation[0], restricted[0], restricted[1]),
                        ConstrainValue(eulerRotation[1], restricted[2], restricted[3]),
                        ConstrainValue(eulerRotation[2], restricted[4], restricted[5]));
                }
                else if (distortionEnabled && distortionsController.illusions[distortionsController.trialId].fixedPos)
                {
                    Bone.localEulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
                } else
                {
                    Bone.localRotation = new Quaternion(-(float)ORot.Rotation[0], (float)ORot.Rotation[1], (float)ORot.Rotation[2], -(float)ORot.Rotation[3]);
                }


            }

            Output_GetSegmentLocalTranslation OTran = MyClient.GetSegmentLocalTranslation(SubjectName, Bone.gameObject.name);
            if (OTran.Result == Result.Success)
            {
                prevPos = curPos;
                curPos = new Vector3(0.001f * -(float)OTran.Translation[0], 0.001f * (float)OTran.Translation[1], 0.001f * (float)OTran.Translation[2]);

                // Bone.localPosition = new Vector3(-0.001f * (float)OTran.Translation[0], 0.001f * (float)OTran.Translation[1], 0.001f * (float)OTran.Translation[2]);

                if (distortionEnabled && distortionsController.illusions[distortionsController.trialId].fixedPos && Bone.gameObject.name == "Arm")
                {
                    Bone.localPosition = distortionsController.illusions[distortionsController.trialId].pos;
                } else if (distortionEnabled && Bone.gameObject.name == "Arm")
                {
                    var bonePos = Bone.localPosition;

                    //if (Mathf.Abs(trans[0] - curPos[0]) > 0.1)
                    //{
                        bonePos[0] = bonePos[0] + (curPos[0] - prevPos[0]) * distortionsController.illusions[distortionsController.trialId].vel[0];
                    //}

                    //if (Mathf.Abs(trans[1] - curPos[1]) > 0.1)
                    //{
                        bonePos[1] = bonePos[1] + (curPos[1] - prevPos[1]) * distortionsController.illusions[distortionsController.trialId].vel[1];
                    //}

                    //if (Mathf.Abs(trans[2] - curPos[2]) > 0.1)
                    //{
                        bonePos[2] = bonePos[2] + (curPos[2] - prevPos[2]) * distortionsController.illusions[distortionsController.trialId].vel[2];
                    //}

                    Bone.localPosition = curPos;


                    //Bone.localPosition = new Vector3(0.001f * -(float)OTran.Translation[0] * distortionsController.illusions[distortionsController.trialId].vel[0],
                    //    0.001f * (float)OTran.Translation[1] * distortionsController.illusions[distortionsController.trialId].vel[1],
                    //    0.001f * (float)OTran.Translation[2] * distortionsController.illusions[distortionsController.trialId].vel[2]);
                }
                else
                {
                    Bone.localPosition = curPos;
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

            if (drawBones)
            {
                CreateCylinder(Bone);
            }


            // recurse through children
            for (int iChild = 0; iChild < Bone.childCount; iChild++)
            {
                ApplyBoneTransform(Bone.GetChild(iChild));


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
            if (!distortionsController.illusions[distortionsController.trialId].kink.ContainsKey("Hand"))
            {
                return false;
            }

            Output_GetSegmentLocalRotationQuaternion ORot = MyClient.GetSegmentLocalRotationQuaternion(SubjectName, "Hand");
            if (ORot.Result != Result.Success)
            {
                return false;
            }

            Quaternion rot = new Quaternion(-(float)ORot.Rotation[0], (float)ORot.Rotation[1], (float)ORot.Rotation[2], -(float)ORot.Rotation[3]);
            Vector3 eulerRotation = rot.eulerAngles;
            List<float> restricted = distortionsController.illusions[distortionsController.trialId].kink["Hand"];
            
            if (eulerRotation[0] != ConstrainValue(eulerRotation[0], restricted[0], restricted[1]) || 
                eulerRotation[1] != ConstrainValue(eulerRotation[1], restricted[2], restricted[3]) ||
                eulerRotation[2] != ConstrainValue(eulerRotation[2], restricted[4], restricted[5]))
            {
                return false;
            }

            return true;

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

    }
}


