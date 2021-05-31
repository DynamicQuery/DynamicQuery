using System;
using System.Collections.Generic;
using System.Linq;

namespace DynamicSelect
{
    public class Node
    {
        public enum NodeType { ROOT, PROPERTY, OBJECT, LIST }

        public Projection Projection { get; set; } = new Projection();

        public List<Node> ChildNodes { get; set; } = new List<Node>();

        public NodeType Type { get; set; } = NodeType.ROOT;

        public Node Add(string propertyId)
        {
            if (IsLeafProjection(propertyId))
            {
                if (NoSimilarChildNodes(propertyId))
                {
                    Node node = new Node();
                    node.Projection.DisplayName = propertyId;
                    node.Projection.PropertyId = propertyId;
                    node.Type = NodeType.PROPERTY;
                    ChildNodes.Add(node);
                }

            }
            else if (IsPropertyProjection(propertyId))
            {
                int periodIndex = propertyId.IndexOf(".");
                Node node = new Node();
                node.Projection.DisplayName = propertyId.Substring(0, periodIndex);
                node.Projection.PropertyId = propertyId.Substring(periodIndex + 1);
                node.Type = NodeType.OBJECT;
                if (NoSimilarChildNodes(node.Projection.DisplayName))
                {
                    ChildNodes.Add(node);
                    node.Add(node.Projection.PropertyId);
                }
                else
                {
                    Node n = ChildNodes.Find(x => x.Projection.DisplayName == node.Projection.DisplayName);
                    n.Add(node.Projection.PropertyId);

                }
            }
            else
            {
                int openIndex = propertyId.IndexOf("[");
                int closeIndex = propertyId.LastIndexOf("]");

                Node node = new Node();
                node.Projection.DisplayName = new string(propertyId.Take(openIndex).ToArray());
                node.Projection.PropertyId = new string(propertyId.Skip(openIndex + 1).Take(closeIndex - openIndex - 1).ToArray());
                node.Type = NodeType.LIST;
                if (NoSimilarChildNodes(node.Projection.DisplayName))
                {
                    ChildNodes.Add(node);
                    node.Add(node.Projection.PropertyId);
                }
                else
                {
                    Node n = ChildNodes.Find(x => x.Projection.DisplayName == node.Projection.DisplayName);
                    n.Add(node.Projection.PropertyId);
                }
            }

            return this;
        }

        private bool IsPropertyProjection(string propertyId)
        {
            if (!propertyId.Contains("."))
                return false;
            else
            {
                if (!propertyId.Contains("["))
                    return true;
                return propertyId.IndexOf(".") < propertyId.IndexOf("[");
            }
        }

        private bool NoSimilarChildNodes(string displayName) =>
            !ChildNodes.Any(node => node.Projection.DisplayName == displayName);

        private bool IsLeafProjection(string propertyId) =>
            ".[".All(c => !propertyId.Contains(c));
    }
}

