// Copyright (c) 2010-2013 SIL International
// This software is licensed under the LGPL, version 2.1 or later
// (http://www.gnu.org/licenses/lgpl-2.1.html)
//
// File: SilTimeUtils.cs
// Responsibility: mcconnel
//
// <remarks>
// </remarks>

using System;
using SIL.LCModel.Core.KernelInterfaces;

namespace SIL.LCModel.Core.Cellar
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Methods for converting between SilTime (long integer) and DateTime.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public static class SilTime
	{
		/// <summary>The number of msec between 1AD and 1601AD</summary>
		public const long kNumberOfMsecBetween1601And0001 = (146097 * 86400000L * 1600 / 400);

		/// <summary>
		/// Convert an SilTime long integer value to the corresponding DateTime value.
		/// </summary>
		public static DateTime ConvertFromSilTime(long silTime)
		{
			// Change the time from SilTime-relative time (i.e. relative to the year 1601),
			// ticks are relative to the year 0001
			long msTime = silTime + kNumberOfMsecBetween1601And0001;
			// Ticks are in 100-nanosecond intervals. SilTime expects milliseconds
			return new DateTime(msTime * 10000);
		}

		/// <summary>
		/// Convert a DateTime object to the corresponding SilTime long integer value.
		/// </summary>
		public static long ConvertToSilTime(DateTime dt)
		{
			// Ticks are in 100-nanosecond intervals. SilTime expects milliseconds
			long msTime = dt.Ticks / 10000;
			// Now we need to change the time to SilTime-relative time (i.e. relative to the
			// year 1601), ticks are relative to year 0001
			return msTime - kNumberOfMsecBetween1601And0001;
		}

		/// <summary>
		/// Get a Time property value coverted to a DateTime value.
		/// </summary>
		public static DateTime GetTimeProperty(ISilDataAccess sda, int hvo, int flid)
		{
			long silTime;
			try
			{
				silTime = sda.get_TimeProp(hvo, flid);
				return ConvertFromSilTime(silTime);
			}
			catch
			{
				return DateTime.MinValue;
			}
		}

		/// <summary>
		/// Set a Time property to a given DateTime value.
		/// </summary>
		public static void SetTimeProperty(ISilDataAccess sda, int hvo, int flid, DateTime dt)
		{
			long silTime = ConvertToSilTime(dt);
			sda.SetTime(hvo, flid, silTime);
		}
	}
}
