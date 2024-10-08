﻿using System;
using UdonSharp;
using UnityEngine;
using VRC.Udon.Common.Interfaces;

namespace Mascari4615
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class TimerSound : MBase
	{
		[SerializeField] private Timer timer;

		[SerializeField] private AudioSource loopAudioSource;
		[SerializeField] private AudioSource endingSFXSource;
		[SerializeField] private AudioSource lerpSFXSource;

		[SerializeField] private float[] timeFlags;
		[SerializeField] private AudioClip[] timeSFXs;

		[SerializeField] private AudioClip defaultSFX;
		[SerializeField] private AudioClip endingSFX;

		// HACK:
		[SerializeField] private AudioClip lerpSFX;
		[SerializeField] private UITimer timerUI;

		private void Start()
		{
			Init();
		}

		private void Init()
		{
			timer.RegisterListener(this, nameof(OnEnding_G), (int)TimerEvent.TimeExpired);

			lerpSFXSource.clip = lerpSFX;
			lerpSFXSource.mute = true;
			lerpSFXSource.loop = true;
			lerpSFXSource.Play();

			loopAudioSource.clip = defaultSFX;
			loopAudioSource.mute = true;
			loopAudioSource.loop = true;
			loopAudioSource.Play();
		}

		public void OnEnding_G()
		{
			SendCustomNetworkEvent(NetworkEventTarget.All, nameof(OnEnding));
		}

		public void OnEnding()
		{
			endingSFXSource.clip = endingSFX;
			endingSFXSource.PlayOneShot(endingSFX);
		}

		private void Update()
		{
			UpdateSound();
		}

		private void UpdateSound()
		{
			if (timer == null)
				return;

			lerpSFXSource.volume = timerUI.IsLerping ? 1 : 0;
		
			int expireTime = timer.ExpireTime;

			if (expireTime == NONE_INT)
			{
				loopAudioSource.mute = true;
				return;
			}
			loopAudioSource.mute = false;

			int diff = expireTime - timer.CalcedCurTime;
			TimeSpan timeSpan = TimeSpan.FromMilliseconds(diff);

			bool isInFlag = false;
			for (int i = timeFlags.Length - 1; i >= 0; i--)
			{
				if (timeSpan.TotalSeconds <= timeFlags[i])
				{
					isInFlag = true;
					if (loopAudioSource.clip != timeSFXs[i])
					{
						loopAudioSource.clip = timeSFXs[i];
						loopAudioSource.Play();
					}
					break;
				}
			}

			if (isInFlag == false)
			{
				if (loopAudioSource.clip != defaultSFX)
				{
					loopAudioSource.clip = defaultSFX;
					loopAudioSource.Play();
				}
			}
		}
	}
}