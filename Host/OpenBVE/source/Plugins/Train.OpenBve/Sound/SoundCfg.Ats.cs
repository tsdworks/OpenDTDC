﻿using OpenBveApi.Math;
using SoundManager;
using TrainManager.Trains;

namespace Train.OpenBve
{
	internal partial class SoundCfgParser
	{
		/// <summary>Loads the default ATS plugin sound set</summary>
		/// <param name="train">The train</param>
		private void LoadDefaultATSSounds(TrainBase train)
		{
			Vector3 position = new Vector3(train.Cars[train.DriverCar].Driver.X, train.Cars[train.DriverCar].Driver.Y, train.Cars[train.DriverCar].Driver.Z + 1.0);
			const double radius = 2.0;
			train.Cars[train.DriverCar].Sounds.Plugin = new[] {
				new CarSound(Plugin.currentHost, train.TrainFolder,"ats.wav", radius, position),
				new CarSound(Plugin.currentHost, train.TrainFolder, "atscnt.wav", radius, position),
				new CarSound(Plugin.currentHost, train.TrainFolder, "ding.wav", radius, position),
				new CarSound(Plugin.currentHost, train.TrainFolder, "toats.wav", radius, position),
				new CarSound(Plugin.currentHost, train.TrainFolder, "toatc.wav", radius, position),
				new CarSound(Plugin.currentHost, train.TrainFolder, "eb.wav", radius, position)
			};
		}
	}
}
