﻿//
// Copyright (c) Peach Fuzzer, LLC
//

using System.ComponentModel;
using Peach.Core;
using Peach.Core.Dom;

namespace Peach.Pro.Core.Mutators
{
	[Mutator("StringStatic")]
	[Description("Perform common string mutations")]
	public partial class StringStatic : Mutator
	{
		public StringStatic(DataElement obj)
			: base(obj)
		{
		}

		public new static bool supportedDataElement(DataElement obj)
		{
			if (obj is Peach.Core.Dom.String && obj.isMutable)
				return true;

			return false;
		}

		public override int count
		{
			get
			{
				return values.Length;
			}
		}

		public override uint mutation
		{
			get;
			set;
		}

		public sealed override void sequentialMutation(DataElement obj)
		{
			performMutation(obj, (int)mutation);
		}

		public sealed override void randomMutation(DataElement obj)
		{
			var idx = context.Random.Next(0, values.Length);
			performMutation(obj, idx);
		}

		protected virtual void performMutation(DataElement obj, int index)
		{
			obj.mutationFlags = MutateOverride.Default;
			obj.MutatedValue = new Variant(values[index]);
		}
	}
}
