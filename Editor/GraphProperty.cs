using UnityEngine;
using System;
using LoD.BT.Extensions;

namespace LoD.BT.Editor {
  [Serializable]
  public abstract class GraphProperty {
    public string name;
    public abstract Type type { get; }
    public abstract object value { get; }
  }

  public class GraphProperty<T> : GraphProperty {
    public T defaultValue;
    public override object value { get { return defaultValue; } }
    public override Type type { get { return typeof(T); } }

    public GraphProperty(T defaultValue, string name = "") {
      this.name = string.IsNullOrEmpty(name)
        ? $"new {type.GetFriendlyName()} property"
        : name;
      this.defaultValue = defaultValue;
    }
  }
}

