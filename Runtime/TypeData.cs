using UnityEngine;
using System;
using System.Collections.Generic;

namespace LoD.BT.Runtime {
  [Serializable]
  public class TypeData : ISerializationCallbackReceiver {
    public bool boolData;
    public string stringData;
    public float floatData;
    public int intData;
    public AnimationCurve curveData;
    public Vector2 vec2Data;
    public Vector3 vec3Data;

    public string valueType;

    public object value { get; set; }

    public Type type { get { return Type.GetType(valueType); } }

    public TypeData (object value) {
      this.value = value;
    }

    public void OnBeforeSerialize() {
      valueType = value.GetType().AssemblyQualifiedName;
      switch(value) {
        case AnimationCurve c:
          curveData = c;
          break;
        case float f:
          floatData = f;
          break;
        case int i:
          intData = i;
          break;
        case string s:
          stringData = s;
          break;
        case bool b:
          boolData = b;
          break;
        case Vector2 v2:
          vec2Data = v2;
          break;
        case Vector3 v3:
          vec3Data = v3;
          break;
      }
    }

    public void OnAfterDeserialize() {
      var @switch = new Dictionary<Type, Action> {
        { typeof(bool), () => value = boolData },
        { typeof(float), () => value = floatData },
        { typeof(int), () => value = intData },
        { typeof(string), () => value = stringData },
        { typeof(AnimationCurve), () => value = curveData },
        { typeof(Vector2), () => value = vec2Data },
        { typeof(Vector3), () => value = vec3Data },
      };
      @switch[type]();
    }
  }
}


