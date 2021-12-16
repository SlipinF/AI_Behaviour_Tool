using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;

namespace LoD.BT.Runtime {
  [Serializable]
  public class BehaviourNodeData {
    public string GUID;
    public Vector2 nodePosition;
    public string nodeType;
    public string compositeType;
    public string decoratorType;
    public string fullName;
    public string name;
    public int portCount;
    public string[] valuesKeys;
    public TypeData[] valuesValues;

    public Dictionary<string, object> values {
      get {
        Dictionary<string, object> _values = new Dictionary<string, object>();
        for(int i = 0; i < valuesKeys.Length; i++) {
          _values[valuesKeys[i]] = valuesValues[i].value;
        }
        return _values;
      }
      set {
        valuesKeys = value.Keys.ToArray();
        valuesValues = value.Values.Select(v => new TypeData(v)).ToArray();
      }
    }
  }
}

