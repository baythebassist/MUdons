﻿using UdonSharp;
using UnityEngine;
using VRC.Udon;

namespace Mascari4615
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class ShootingTarget : MBase
	{
		[SerializeField] private ShootingManager shootingManager;
		[SerializeField] private UdonBehaviour[] targetUdonBehaviours;
		[SerializeField] private string[] eventNames;

		public void Ahh()
		{
			Debug.Log($"{gameObject.name} : {nameof(Ahh)}");

			for (var i = 0; i < targetUdonBehaviours.Length; i++)
				targetUdonBehaviours[i].SendCustomEvent(eventNames[i]);
		}
	}
}