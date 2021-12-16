using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;
using System;

namespace LoD.BT.Editor {

  public class CustomPort : Port {
    static public CustomPort Create(Orientation orientation, Direction direction, Port.Capacity capacity, Type type, IEdgeConnectorListener connectorListener = null) {
      CustomPort port = new CustomPort(orientation, direction, capacity, type) {
        m_EdgeConnector = new EdgeConnector<Edge>(connectorListener),
      };

      if(direction == Direction.Output) {
        port.AddManipulator(port.m_EdgeConnector);
      }

      return port;
    }

    public CustomPort(Orientation orientation, Direction direction, Port.Capacity capacity, Type type) : base(orientation, direction, capacity, type) {}
  }
}
