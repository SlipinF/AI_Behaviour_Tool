using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UnityEditor.Experimental.GraphView;
using System;
using System.Reflection;
using LoD.BT.Runtime;

namespace LoD.BT.Editor {
  public class BlackboardFieldView : BlackboardField {
    public GraphProperty property;
    private BehaviourGraphView graphView;

    private VisualElement _defaultValueView;

    public VisualElement defaultValueView { get { return _defaultValueView; } }

    public BlackboardFieldView(GraphProperty property, BehaviourGraphView graphView, Texture icon, string text, string typeText) : base(icon, text, typeText) {
      this.property = property;
      this.graphView = graphView;

      BuildDefaultValueView(property);
    }

    void BuildDefaultValueView(GraphProperty property) {
      MethodInfo method = typeof(BlackboardFieldView).GetMethod("BuildDefaultValueViewForType");
      MethodInfo genericMethod = method.MakeGenericMethod(property.type);
      genericMethod.Invoke(this, new object[] { property });
    }

    public void BuildDefaultValueViewForType<T>(GraphProperty<T> property) {
      BaseField<T> field = TypeUtils.FieldForType<T>();

      if(property.defaultValue != null) {
        field.value = property.defaultValue;
      }

      field.RegisterValueChangedCallback((ChangeEvent<T> evt) => {
        property.defaultValue = evt.newValue;
        graphView.UpdateNodesBoundsToProperty(property);
      });

      VisualElement container = new VisualElement() { name = "container" };
      VisualElement labelContainer = new VisualElement { name = "label" };
      VisualElement valueContainer = new VisualElement { name = "value" };

      labelContainer.Add(new Label("Default Value"));
      valueContainer.Add(field);

      container.Add(labelContainer);
      container.Add(valueContainer);

      _defaultValueView = container;
    }
  }
}

