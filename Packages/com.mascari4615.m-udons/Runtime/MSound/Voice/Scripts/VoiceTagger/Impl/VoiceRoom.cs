﻿using System;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace Mascari4615
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class VoiceRoom : VoiceTagger
	{
		[Header("_" + nameof(VoiceRoom))]
		[SerializeField] private MTarget[] mTargets;
		[field: SerializeField] public MBool[] IsPlayerInside { get; private set; }

		// [SerializeField] private CustomBool isLocked;
		[SerializeField] private Timer isLocked_Timer;

		public override bool IsPlayerIn(VRCPlayerApi player)
		{
			for (int j = 0; j < mTargets.Length; j++)
			{
				if (mTargets[j].CurTargetPlayerID == player.playerId && IsPlayerInside[j].Value)
				{
					return true;
				}
			}
			
			return false;
		}

		public void GoRoom()
		{
			// MDebugLog(nameof(GoRoom));

			int localPlayerNum = GetLocalPlayerNum();
			if (localPlayerNum == NONE_INT)
				return;

			int inPlayerCount = 0;
			for (int i = 0; i < mTargets.Length; i++)
			{
				if (IsPlayerInside[i].Value)
					inPlayerCount++;
			}

			if (IsPlayerInside[localPlayerNum].Value)
			{
				IsPlayerInside[localPlayerNum].SetValue(false);

				// if ((inPlayerCount == 1) && (isLocked.Value == true))
				if ((inPlayerCount == 1) && (isLocked_Timer.IsExpired == false))
				{
					// isLocked.SetValue(false);
					isLocked_Timer.ResetTime();
				}
			}
			else
			{
				// if (isLocked.Value)
				if (isLocked_Timer.IsExpired == false)
					return;

				IsPlayerInside[localPlayerNum].SetValue(true);
			}
		}

		private int GetLocalPlayerNum()
		{
			for (int i = 0; i < mTargets.Length; i++)
			{
				if (mTargets[i].IsTargetPlayer())
					return i;
			}

			return NONE_INT;
		}

		public void Lock()
		{
			// isLocked.SetValue(true);
			isLocked_Timer.SetTimer();
		}
		public void Unlock()
		{
			// isLocked.SetValue(false);
			isLocked_Timer.ResetTime();
		}

		public void ResetSync()
		{
			for (int i = 0; i < mTargets.Length; i++)
				IsPlayerInside[i].SetValue(false);

			Unlock();
		}
	}
}