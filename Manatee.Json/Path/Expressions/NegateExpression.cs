using System;

namespace Manatee.Json.Path.Expressions
{
	internal class NegateExpression<T> : ExpressionTreeNode<T>, IEquatable<NegateExpression<T>>
	{
		public ExpressionTreeNode<T> Root { get; set; }

		public override object Evaluate(T json, JsonValue root)
		{
			return -Convert.ToDouble(Root.Evaluate(json, root));
		}
		public override string ToString()
		{
			return Root is ExpressionTreeBranch<T>
					   ? $"-({Root})"
				       : $"-{Root}";
		}
		public bool Equals(NegateExpression<T> other)
		{
			if (ReferenceEquals(null, other)) return false;
			if (ReferenceEquals(this, other)) return true;
			return Equals(Root, other.Root);
		}
		public override bool Equals(object obj)
		{
			return Equals(obj as NegateExpression<T>);
		}
		public override int GetHashCode()
		{
			return Root?.GetHashCode() ?? 0;
		}
	}
}