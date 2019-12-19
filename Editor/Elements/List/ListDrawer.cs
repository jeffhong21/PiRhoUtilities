﻿using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace PiRhoSoft.Utilities.Editor
{
	[CustomPropertyDrawer(typeof(ListAttribute))]
	class ListDrawer : PropertyDrawer
	{
		private const string _invalidTypeWarning = "(PULDIT) invalid type for ListAttribute on field '{0}': List can only be applied to SerializedList or SerializedArray fields";
		private const string _invalidAddCallbackWarning = "(PULDIAC) invalid add callback for ListAttribute on field '{0}': The method must accept an int or have no parameters";
		private const string _invalidAddReferenceCallbackWarning = "(PULDIAC) invalid add callback for ListAttribute on field '{0}': The method must accept an int and/or an object in either order or have no parameters";
		private const string _invalidRemoveCallbackWarning = "(PULDIRMC) invalid remove callback for ListAttribute on field '{0}': The method must accept an int or have no parameters";
		private const string _invalidReorderCallbackWarning = "(PULDIROC) invalid reorder callback for ListAttribute on field '{0}': The method must accept two ints or have no parameters";
		private const string _invalidChangeCallbackWarning = "(PULDICC) invalid change callback for ListAttribute on field '{0}': The method must have no parameters";

		public override VisualElement CreatePropertyGUI(SerializedProperty property)
		{
			var items = property.FindPropertyRelative(SerializedList<string>.ItemsProperty);

			if (items != null && items.isArray)
			{
				var isReference = fieldInfo.FieldType.BaseType.GetGenericTypeDefinition() == typeof(ReferenceList<>);
				var referenceType = isReference ? fieldInfo.GetFieldType() : null;
				var declaringType = fieldInfo.DeclaringType;
				var listAttribute = attribute as ListAttribute;
				var drawer = this.GetNextDrawer();
				var proxy = new PropertyListProxy(property, drawer);

				var field = new ListField();
				field.SetProxy(proxy, referenceType, true);
				field.bindingPath = property.propertyPath;
				// TODO: other stuff from ConfigureField

				if (!string.IsNullOrEmpty(listAttribute.EmptyLabel))
					field.EmptyLabel = listAttribute.EmptyLabel;

				field.AllowAdd = listAttribute.AllowAdd != ListAttribute.Never;
				field.AllowRemove = listAttribute.AllowRemove != ListAttribute.Never;
				field.AllowReorder = listAttribute.AllowReorder;

				SetupAdd(listAttribute, proxy, field, property, declaringType, isReference);
				SetupRemove(listAttribute, proxy, field, property, declaringType);
				SetupReorder(listAttribute, field, property, declaringType);
				SetupChange(listAttribute, field, property, declaringType);

				return field;
			}
			else
			{
				Debug.LogWarningFormat(_invalidTypeWarning, property.propertyPath);
				return new FieldContainer(property.displayName, string.Empty);
			}
		}

		private void SetupAdd(ListAttribute listAttribute, PropertyListProxy proxy, ListField field, SerializedProperty property, Type declaringType, bool isReference)
		{
			if (field.AllowAdd)
			{
				if (!string.IsNullOrEmpty(listAttribute.AllowAdd))
				{
					proxy.CanAddCallback = ReflectionHelper.CreateValueSourceFunction(property, field, declaringType, listAttribute.AllowAdd, true, nameof(ListAttribute), nameof(ListAttribute.AllowAdd));
				}

				if (!string.IsNullOrEmpty(listAttribute.AddCallback))
				{
					if (!isReference)
					{
						var addCallback = ReflectionHelper.CreateActionCallback(property, declaringType, listAttribute.AddCallback, nameof(ListAttribute), nameof(ListAttribute.AddCallback));
						if (addCallback != null)
						{
							field.RegisterCallback<ListField.ItemAddedEvent>(evt => addCallback.Invoke());
						}
						else
						{
							var addCallbackIndex = ReflectionHelper.CreateActionCallback<int>(property, declaringType, listAttribute.AddCallback, nameof(ListAttribute), nameof(ListAttribute.AddCallback));
							if (addCallbackIndex != null)
								field.RegisterCallback<ListField.ItemAddedEvent>(evt => addCallbackIndex.Invoke(evt.Index));
							else
								Debug.LogWarningFormat(_invalidAddCallbackWarning, property.propertyPath);
						}
					}
					else
					{
						var addCallback = ReflectionHelper.CreateActionCallback(property, declaringType, listAttribute.AddCallback, nameof(ListAttribute), nameof(ListAttribute.AddCallback));
						if (addCallback != null)
						{
							field.RegisterCallback<ListField.ItemAddedEvent>(evt => addCallback.Invoke());
						}
						else
						{
							var addCallbackIndex = ReflectionHelper.CreateActionCallback<int>(property, declaringType, listAttribute.AddCallback, nameof(ListAttribute), nameof(ListAttribute.AddCallback));
							if (addCallbackIndex != null)
								field.RegisterCallback<ListField.ItemAddedEvent>(evt => addCallbackIndex.Invoke(evt.Index));
							else
								Debug.LogWarningFormat(_invalidAddReferenceCallbackWarning, property.propertyPath);
						}
					}
				}
			}
		}

		private void SetupRemove(ListAttribute listAttribute, PropertyListProxy proxy, ListField field, SerializedProperty property, Type declaringType)
		{
			if (field.AllowRemove)
			{
				if (!string.IsNullOrEmpty(listAttribute.AllowRemove))
				{
					proxy.CanRemoveCallback = ReflectionHelper.CreateFunctionCallback<int, bool>(property, declaringType, listAttribute.AllowRemove, nameof(ListAttribute), nameof(ListAttribute.AllowRemove));

					if (proxy.CanRemoveCallback == null)
					{
						var canRemove = ReflectionHelper.CreateValueSourceFunction(property, field, declaringType, listAttribute.AllowRemove, true, nameof(ListAttribute), nameof(ListAttribute.AllowRemove));
						proxy.CanRemoveCallback = index => canRemove();
					}
				}

				if (!string.IsNullOrEmpty(listAttribute.RemoveCallback))
				{
					var removeCallback = ReflectionHelper.CreateActionCallback(property, declaringType, listAttribute.RemoveCallback, nameof(ListAttribute), nameof(ListAttribute.RemoveCallback));
					if (removeCallback != null)
					{
						field.RegisterCallback<ListField.ItemRemovedEvent>(evt => removeCallback.Invoke());
					}
					else
					{
						var removeCallbackIndex = ReflectionHelper.CreateActionCallback<int>(property, declaringType, listAttribute.RemoveCallback, nameof(ListAttribute), nameof(ListAttribute.RemoveCallback));
						if (removeCallbackIndex != null)
							field.RegisterCallback<ListField.ItemRemovedEvent>(evt => removeCallbackIndex.Invoke(evt.Index));
						else
							Debug.LogWarningFormat(_invalidRemoveCallbackWarning, property.propertyPath);
					}
				}
			}
		}

		private void SetupReorder(ListAttribute listAttribute, ListField field, SerializedProperty property, Type declaringType)
		{
			if (field.AllowReorder)
			{
				if (!string.IsNullOrEmpty(listAttribute.ReorderCallback))
				{
					var reorderCallback = ReflectionHelper.CreateActionCallback(property, declaringType, listAttribute.ReorderCallback, nameof(ListAttribute), nameof(ListAttribute.ReorderCallback));
					if (reorderCallback != null)
					{
						field.RegisterCallback<ListField.ItemReorderedEvent>(evt => reorderCallback.Invoke());
					}
					else
					{
						var reorderCallbackFromTo = ReflectionHelper.CreateActionCallback<int, int>(property, declaringType, listAttribute.ReorderCallback, nameof(ListAttribute), nameof(ListAttribute.ReorderCallback));
						if (reorderCallbackFromTo != null)
							field.RegisterCallback<ListField.ItemReorderedEvent>(evt => reorderCallbackFromTo.Invoke(evt.FromIndex, evt.ToIndex));
						else
							Debug.LogWarningFormat(_invalidReorderCallbackWarning, property.propertyPath);
					}
				}
			}
		}

		private void SetupChange(ListAttribute listAttribute, ListField field, SerializedProperty property, Type declaringType)
		{
			if (!string.IsNullOrEmpty(listAttribute.ChangeCallback))
			{
				var changeCallback = ReflectionHelper.CreateActionCallback(property, declaringType, listAttribute.ChangeCallback, nameof(ListAttribute), nameof(ListAttribute.AllowRemove));
				if (changeCallback != null)
					field.RegisterCallback<ListField.ItemsChangedEvent>(evt => changeCallback.Invoke());
				else
					Debug.LogWarningFormat(_invalidChangeCallbackWarning, property.propertyPath);
			}
		}
	}
}
