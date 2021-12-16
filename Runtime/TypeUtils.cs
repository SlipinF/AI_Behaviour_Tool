using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
#if (UNITY_EDITOR) 
using UnityEditor.UIElements;
#endif


namespace LoD.BT.Runtime {
  public class TypeUtils {
#if (UNITY_EDITOR)
        static Dictionary<string, Type> FIELDS = new Dictionary<string, Type>() {
      { typeof(Single).Name, typeof(FloatField) },
      { typeof(Int32).Name, typeof(IntegerField) },
      { typeof(String).Name, typeof(TextField) },
      { typeof(Boolean).Name, typeof(Toggle) },
      { typeof(Enum).Name, typeof(EnumField) },
      { typeof(AnimationCurve).Name, typeof(CurveField) },
      { typeof(Vector2).Name, typeof(Vector2Field) },
      { typeof(Vector3).Name, typeof(Vector3Field) },
    };
    static Dictionary<string, Func<object>> DEFAULT_VALUES = new Dictionary<string, Func<object>>() {
      { typeof(Single).Name, () => 0f },
      { typeof(Int32).Name, () => 0 },
      { typeof(String).Name, () => "" },
      { typeof(Boolean).Name, () => false },
      { typeof(AnimationCurve).Name, () => null },
      { typeof(Vector2).Name, () => new Vector2() },
      { typeof(Vector3).Name, () => new Vector3() },
    };

    public static Type FieldTypeForType(Type type) {
      return FIELDS[type.Name];
    }

    public static object DefaultValueForType(Type type) {
      return DEFAULT_VALUES[type.Name]();
    }

    public static BaseField<T> FieldForType<T>() {
      Type fieldType = FieldTypeForType(typeof(T));

      var ctors = fieldType.GetConstructors();

      BaseField<T> field = (BaseField<T>) ctors[0].Invoke(new object[] { });

      return field;
    }
#endif

        public static Type GetDecoratorType(string typeName) {
      switch(typeName) {
        case "Repeater":
          return typeof(Repeater);
        case "Succeeder":
          return typeof(Succeeder);
        case "Inverter":
          return typeof(Inverter);
        case "Once":
          return typeof(Once);
        default:
          throw new Exception($"Unknown decorator type {typeName}");
      }
    }

    public static Type GetCompositeType(string typeName) {
      switch(typeName) {
        case "Parallel":
          return typeof(Parallel);
        case "Sequence":
          return typeof(Sequence);
        case "Selector":
          return typeof(Selector);
        case "Randomizer":
          return typeof(Randomizer);
        default:
          throw new Exception($"Unknown composite type {typeName}");
      }
    }
  }
}
