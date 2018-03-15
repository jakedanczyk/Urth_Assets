using UnityEngine;
using System.Collections;

namespace Footsteps {

	[RequireComponent(typeof(Collider), typeof(Rigidbody))]
	public class FootstepTrigger : MonoBehaviour {

		public Collider thisCollider;
		public CharacterFootsteps footsteps;
        IEnumerator wait;


        void Start()
        {
            //wait = Wait();
			SetCollisions();
		}

		void OnEnable() {
			SetCollisions();
		}

        //      bool isWaiting;
        //void OnTriggerEnter(Collider other) {
        //          if (!isWaiting && footsteps) {
        //              isWaiting = true;
        //              StopCoroutine(wait);
        //              StartCoroutine(wait);
        //		footsteps.TryPlayFootstep(other,transform.position);
        //	}
        //}


        //      IEnumerator Wait()
        //      {
        //          yield return new WaitForSeconds(0.2f);
        //          isWaiting = false;
        //      }

        float lastStepTime,nowTime;
        void OnTriggerEnter(Collider other)
        {
            nowTime = Time.time;
            if (nowTime - lastStepTime > 0.5f && footsteps)
            {
                lastStepTime = nowTime;
                footsteps.TryPlayFootstep(other, transform.position);
            }
        }

        void SetCollisions() {
			if(!footsteps) return;

			Collider[] allColliders = footsteps.GetComponentsInChildren<Collider>();

			foreach(var collider in allColliders) {
				if(collider != GetComponent<Collider>()) {
					Physics.IgnoreCollision(thisCollider, collider);
				}
			}
		}
	}
}
