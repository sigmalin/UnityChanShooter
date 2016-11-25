using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public class BillBoard : MonoBehaviour 
{
	Camera mMainCamera;
	public Camera MainCamera
	{
		get 
		{
			if (mMainCamera == null) 
			{
				uint mainCameraID = (uint)GameCore.GetParameter (ParamGroup.GROUP_CAMERA, CameraParam.MAIN_CAMERA);;
				GameObject mainCameraGO = (GameObject)GameCore.GetParameter (ParamGroup.GROUP_CAMERA, CameraParam.CAMERA_OBJECT, mainCameraID);
				mMainCamera = mainCameraGO.GetComponent<Camera>();
			}
			return mMainCamera;
		}
	}

	// Use this for initialization
	void Start () 
	{
		this.UpdateAsObservable ()
			.Where (_ => MainCamera != null)
			.Subscribe (_ => 
				{
					this.transform.transform.LookAt(this.transform.position + MainCamera.transform.rotation * Vector3.forward, MainCamera.transform.rotation * Vector3.up);
				}
			);
	}
}
