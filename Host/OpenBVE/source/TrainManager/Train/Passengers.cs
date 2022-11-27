﻿using System;

namespace TrainManager.Trains
{
	/// <summary>The passenger loading of a train at any given point in time</summary>
	public struct TrainPassengers
	{
		/// <summary>The current passenger ratio: 
		/// Must be a number between 0 and 250, where 100 represents a nominally loaded train</summary>
		public double PassengerRatio;

		/// <summary>The current acceleration as being felt by the passengers</summary>
		private double CurrentAcceleration;

		/// <summary>The speed difference from that of the last frame</summary>
		private double CurrentSpeedDifference;

		/// <summary>Whether the passengers have fallen over (Used for scoring purposes)</summary>
		public bool FallenOver;

		/// <summary>Called once a frame to update the status of the passengers</summary>
		/// <param name="Acceleration">The current average acceleration of the train</param>
		/// <param name="TimeElapsed">The frame time elapsed</param>
		public void Update(double Acceleration, double TimeElapsed)
		{
			double accelerationDifference = Acceleration - CurrentAcceleration;
			double jerk = 0.25 + 0.10 * Math.Abs(accelerationDifference);
			double accelerationQuanta = jerk * TimeElapsed;
			if (Math.Abs(accelerationDifference) < accelerationQuanta)
			{
				CurrentAcceleration = Acceleration;
				accelerationDifference = 0.0;
			}
			else
			{
				CurrentAcceleration += Math.Sign(accelerationDifference) * accelerationQuanta;
				accelerationDifference = Acceleration - CurrentAcceleration;
			}

			CurrentSpeedDifference += accelerationDifference * TimeElapsed;
			double acceleration = 0.10 + 0.35 * Math.Abs(CurrentSpeedDifference);
			double speedQuanta = acceleration * TimeElapsed;
			if (Math.Abs(CurrentSpeedDifference) < speedQuanta)
			{
				CurrentSpeedDifference = 0.0;
			}
			else
			{
				CurrentSpeedDifference -= Math.Sign(CurrentSpeedDifference) * speedQuanta;
			}

			if (PassengerRatio > 0.0)
			{
				double threshold = 1.0 / PassengerRatio;
				FallenOver = Math.Abs(CurrentSpeedDifference) > threshold;
			}
			else
			{
				FallenOver = false;
			}
		}
	}
}
