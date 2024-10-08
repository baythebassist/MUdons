﻿using UdonSharp;
using UnityEngine;

namespace Mascari4615
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class MTargetBool : MBool
	{
		[field: Header("_" + nameof(MTargetBool))]
		[field: SerializeField] public MTarget MTarget { get; private set; }

		protected override void Start()
		{
			Init();
		}

		private void Init()
		{
			MTarget.RegisterListener(this, nameof(UpdateValue));
			UpdateValue();
		}

		public void UpdateValue()
		{
			SetValue(MTarget.IsTargetPlayer());
		}
	}
}