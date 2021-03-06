﻿// Copyright (c) 2016 SIL International
// This software is licensed under the LGPL, version 2.1 or later
// (http://www.gnu.org/licenses/lgpl-2.1.html)

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using SIL.LCModel.Core.KernelInterfaces;

namespace SIL.LCModel.Core.Text
{
	/// <summary>
	/// This class represents the text properties for a run in a <see cref="TsString"/>.
	/// </summary>
	public class TsTextProps : TsPropsBase, ITsTextProps, IEquatable<TsTextProps>
	{
		internal static TsTextProps EmptyProps => new TsTextProps();

		private static readonly ConcurrentDictionary<TsTextProps, TsTextProps> TextPropsCache = new ConcurrentDictionary<TsTextProps, TsTextProps>();

		static TsTextProps()
		{
			TextPropsCache.TryAdd(EmptyProps, EmptyProps);
		}

		/// <summary>
		/// Gets the interned text props for the specified properties. This ensures that there is only a single
		/// copy of each text props in memory. This replicates the behavior of the C++ implementation. This is
		/// done, because some of the FW code depends on text props being interned.
		/// </summary>
		internal static TsTextProps GetInternedTextProps(IDictionary<int, TsIntPropValue> intProps, IDictionary<int, string> strProps)
		{
			var textProps = new TsTextProps(intProps, strProps);
			return TextPropsCache.GetOrAdd(textProps, textProps);
		}

		/// <summary>
		/// Gets the interned text props for the writing system.
		/// </summary>
		internal static TsTextProps GetInternedTextProps(int ws)
		{
			var textProps = new TsTextProps(ws);
			return TextPropsCache.GetOrAdd(textProps, textProps);
		}

		internal TsTextProps(IDictionary<int, TsIntPropValue> intProps, IDictionary<int, string> strProps)
			: base(intProps, strProps)
		{
		}

		internal TsTextProps(int ws)
		{
			IntProperties.Add((int) FwTextPropType.ktptWs, new TsIntPropValue((int) FwTextPropVar.ktpvDefault, ws));
		}

		private TsTextProps()
		{
		}

		/// <summary>
		/// Creates a builder object from the text property object. The builder contains a copy of
		/// the text properties object's internal data which can be modified through the methods
		/// provided by the builder. (Note that this modifies a copy of the data, not the original
		/// TsTextProps.)
		/// </summary>
		public ITsPropsBldr GetBldr()
		{
			return new TsPropsBldr(IntProperties, StringProperties);
		}

		/// <summary>
		/// Indicates whether the current object is equal to another object of the same type.
		/// </summary>
		public bool Equals(TsTextProps other)
		{
			return other != null && PropertiesEqual(IntProperties, other.IntProperties) && PropertiesEqual(StringProperties, other.StringProperties);
		}

		private static bool PropertiesEqual<T>(SortedList<int, T> props1, SortedList<int, T> props2)
		{
			if (props1.Count != props2.Count)
				return false;

			using (IEnumerator<KeyValuePair<int, T>> enum1 = props1.GetEnumerator())
			using (IEnumerator<KeyValuePair<int, T>> enum2 = props2.GetEnumerator())
			{
				while (enum1.MoveNext() && enum2.MoveNext())
				{
					if (enum1.Current.Key != enum2.Current.Key
						|| !EqualityComparer<T>.Default.Equals(enum1.Current.Value, enum2.Current.Value))
					{
						return false;
					}
				}
				return true;
			}
		}

		/// <summary>
		/// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
		/// </summary>
		public override bool Equals(object obj)
		{
			var other = obj as TsTextProps;
			return other != null && Equals(other);
		}

		/// <summary>
		/// Returns a hash code for this instance.
		/// </summary>
		public override int GetHashCode()
		{
			int code = 23;
			code = code * 31 + GetPropsHashCode(IntProperties);
			code = code * 31 + GetPropsHashCode(StringProperties);
			return code;
		}

		private static int GetPropsHashCode<T>(SortedList<int, T> props)
		{
			int code = 23;
			foreach (KeyValuePair<int, T> prop in props)
			{
				code = code * 31 + prop.Key.GetHashCode();
				code = code * 31 + EqualityComparer<T>.Default.GetHashCode(prop.Value);
			}
			return code;
		}
	}
}
