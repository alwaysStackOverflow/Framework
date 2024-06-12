using UnityEngine;
using UnityGameFramework;

public class Test : MonoBehaviour
{
	public Rigidbody _rigidbody;
	public Animator animator;
	public int State;
	public GameObject O;
	public void Update()
	{
		O.transform.localPosition = Vector3.zero;
		if (Input.GetKeyDown(KeyCode.W))
		{
			_rigidbody.velocity = transform.forward.normalized * 2f;
			Debug.Log($"Key W Down {_rigidbody.velocity}");
			animator.SetInteger("State", 1);
		}
		if (Input.GetKeyUp(KeyCode.W))
		{
			_rigidbody.velocity = transform.forward.normalized * 0;
			Debug.Log($"Key W Up {_rigidbody.velocity}");
			animator.SetInteger("State", 0);
			O.transform.transform.localRotation = Quaternion.identity;
		}
	}
}
