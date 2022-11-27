﻿using SoundManager;
using TrainManager.Handles;
using TrainManager.Power;

namespace TrainManager.BrakeSystems
{
	public abstract class CarBrake
	{
		internal const double Tolerance = 5000.0;

		/// <summary>Contains a reference to the EB handle of the controlling train</summary>
		internal EmergencyHandle emergencyHandle;

		/// <summary>Contains a reference to the reverser handle of the controlling train</summary>
		internal ReverserHandle reverserHandle;

		/// <summary>Whether this is a main or auxiliary brake system</summary>
		public BrakeType brakeType;

		public EqualizingReservoir equalizingReservoir;

		public MainReservoir mainReservoir;

		public AuxiliaryReservoir auxiliaryReservoir;

		public BrakePipe brakePipe;

		public BrakeCylinder brakeCylinder;

		public Compressor airCompressor;

		internal EletropneumaticBrakeType electropneumaticBrakeType;

		public StraightAirPipe straightAirPipe;

		/// <summary>Stores whether the car is a motor car</summary>
		internal bool isMotorCar;

		/// <summary>The speed at which the brake control system activates in m/s</summary>
		public double brakeControlSpeed;

		/// <summary>The current deceleration provided by the electric motor</summary>
		public double motorDeceleration;

		/// <summary>The air sound currently playing</summary>
		public CarSound airSound = new CarSound();
		/// <summary>Played when the pressure in the brake cylinder is decreased from a non-high to a non-zero value</summary>
		public CarSound Air = new CarSound();
		/// <summary>Played when the pressure in the brake cylinder is decreased from a high value</summary>
		public CarSound AirHigh = new CarSound();
		/// <summary>Played when the pressure in the brake cylinder is decreased to zero</summary>
		public CarSound AirZero = new CarSound();
		/// <summary>Played when the brake shoe rubs against the wheels</summary>
		public CarSound Rub = new CarSound();
		/// <summary>The sound played when the brakes are released</summary>
		public CarSound Release = new CarSound();

		internal AccelerationCurve[] decelerationCurves;
		/// <summary>A non-negative floating point number representing the jerk in m/s when the deceleration produced by the electric brake is increased.</summary>
		public double JerkUp;
		/// <summary>A non-negative floating point number representing the jerk in m/s when the deceleration produced by the electric brake is decreased.</summary>
		public double JerkDown;
		
		/// <summary>Updates the brake system</summary>
		/// <param name="TimeElapsed">The frame time elapsed</param>
		/// <param name="currentSpeed">The current speed of the train</param>
		/// <param name="brakeHandle">The controlling brake handle (NOTE: May either be the loco brake if fitted or the train brake)</param>
		/// <param name="Deceleration">The deceleration output provided</param>
		public abstract void Update(double TimeElapsed, double currentSpeed, AbstractHandle brakeHandle, out double Deceleration);

		internal double GetRate(double Ratio, double Factor)
		{
			Ratio = Ratio < 0.0 ? 0.0 : Ratio > 1.0 ? 1.0 : Ratio;
			Ratio = 1.0 - Ratio;
			return 1.5 * Factor * (1.01 - Ratio * Ratio);
		}

		/// <summary>Calculates the max possible deceleration given a brake notch and speed</summary>
		/// <param name="Notch">The brake notch</param>
		/// <param name="currentSpeed">The speed</param>
		/// <returns>The deceleration in m/s</returns>
		public double DecelerationAtServiceMaximumPressure(int Notch, double currentSpeed)
		{
			if (Notch == 0)
			{
				return this.decelerationCurves[0].GetAccelerationOutput(currentSpeed, 1.0);
			}
			if (this.decelerationCurves.Length >= Notch)
			{
				return this.decelerationCurves[Notch - 1].GetAccelerationOutput(currentSpeed, 1.0);
			}
			return this.decelerationCurves[this.decelerationCurves.Length - 1].GetAccelerationOutput(currentSpeed, 1.0);
		}
	}
}
