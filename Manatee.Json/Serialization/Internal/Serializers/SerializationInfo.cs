using System;
using System.Linq.Expressions;
using System.Reflection;
using Manatee.Json.Internal;

namespace Manatee.Json.Serialization.Internal.Serializers
{
	internal abstract class SerializationInfo
	{
		public MemberInfo MemberInfo { get; }
		public string SerializationName { get; }

		public SerializationInfo(MemberInfo memberInfo, string serializationName)
		{
			MemberInfo = memberInfo;
			SerializationName = serializationName;
		}

		public abstract object Get(object source);
		public abstract void Set(object source, object value);

		public override bool Equals(object obj)
		{
			return obj != null && MemberInfo.Equals(((SerializationInfo) obj).MemberInfo);
		}
		public override int GetHashCode()
		{
			return MemberInfo.GetHashCode();
		}
	}

	internal class SerializationInfo<TObject, TMember> : SerializationInfo
	{
		private readonly Func<TObject, TMember> _getter;
		private readonly Action<TObject, TMember> _setter;

		public SerializationInfo(MemberInfo info, Func<TObject, TMember> getter, Action<TObject, TMember> setter, string serializationName)
			: base(info, serializationName)
		{
			_getter = getter;
			_setter = setter;
		}

		public override object Get(object source)
		{
			return _getter((TObject) source);
		}

		public override void Set(object source, object value)
		{
			_setter((TObject) source, (TMember) value);
		}
	}

	internal static class SerializationInfoFactory
	{
		public static SerializationInfo Get(MemberInfo info, string serializationName)
		{
			Type memberType;
			string factoryMethodName;
			var propertyInfo = info as PropertyInfo;
			if (propertyInfo != null)
			{
				memberType = propertyInfo.PropertyType;
				factoryMethodName = nameof(GetProperty);
			}
			else
			{
				memberType = ((FieldInfo) info).FieldType;
				factoryMethodName = nameof(GetField);
			}

#if IOS
			var method = typeof(SerializationInfoFactory).GetMethod(factoryMethodName)
														 .MakeGenericMethod(info.DeclaringType, memberType);
#else
			var method = typeof(SerializationInfoFactory).GetMethod(factoryMethodName, BindingFlags.Static | BindingFlags.NonPublic)
														 .MakeGenericMethod(info.DeclaringType, memberType);
#endif

			return (SerializationInfo) method.Invoke(null, new object[] {info, serializationName});
		}

		private static SerializationInfo GetProperty<TObject, TMember>(MemberInfo info, string serializationName)
		{
			var getter = MemberExpression.GetPropGetter<TObject, TMember>(info.Name);
			var setter = MemberExpression.GetPropSetter<TObject, TMember>(info.Name);

			return new SerializationInfo<TObject, TMember>(info, getter, setter, serializationName);
		}

		private static SerializationInfo GetField<TObject, TMember>(MemberInfo info, string serializationName)
		{
			var getter = MemberExpression.GetFieldGetter<TObject, TMember>(info.Name);
			var setter = MemberExpression.GetFieldSetter<TObject, TMember>(info.Name);

			return new SerializationInfo<TObject, TMember>(info, getter, setter, serializationName);
		}
	}
}