﻿#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2008-2011 Jonathan Skeet. All rights reserved.
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
#endregion

using System;
using System.Collections.Generic;

namespace Managed.Adb.MoreLinq {
	public static partial class MoreEnumerable {
		/// <summary>
		/// Executes the given action on each element in the source sequence
		/// and yields it.
		/// </summary>
		/// <remarks>
		/// The returned sequence is essentially a duplicate of
		/// the original, but with the extra action being executed while the
		/// sequence is evaluated. The action is always taken before the element
		/// is yielded, so any changes made by the action will be visible in the
		/// returned sequence. This operator uses deferred execution and streams it results.
		/// </remarks>
		/// <typeparam name="T">The type of the elements in the sequence</typeparam>
		/// <param name="source">The sequence of elements</param>
		/// <param name="action">The action to execute on each element</param>
		public static IEnumerable<T> Pipe<T> ( this IEnumerable<T> source, Action<T> action ) {
			source.ThrowIfNull ( "source" );
			action.ThrowIfNull ( "action" );
			return PipeImpl ( source, action );
		}

		private static IEnumerable<T> PipeImpl<T> ( this IEnumerable<T> source, Action<T> action ) {
			foreach ( T element in source ) {
				action ( element );
				yield return element;
			}
		}
	}
}
