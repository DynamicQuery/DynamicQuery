using System.Text;

namespace DynamicSelect
{
    public class DynamicSelectQueryBuilder
    {
        public string Build(Node node)
        {
            StringBuilder sb = new StringBuilder();

            if (node.Type == Node.NodeType.ROOT) //root node
            {
                foreach (var childNode in node.ChildNodes)
                {
                    string build = Build(childNode);
                    sb.Append(build);
                }

                string build2 = sb.ToString();
                build2 = build2.Substring(0, build2.Length - 1);
                string ret = $"new {{ {build2 } }}";
                return ret;
            }
            else if (node.Type == Node.NodeType.PROPERTY) // leaf node
            {
                string ret = $"{node.Projection.PropertyId} as {node.Projection.DisplayName},";
                return ret;
            }
            else if (node.Type == Node.NodeType.OBJECT)
            {
                foreach (var childNode in node.ChildNodes)
                {
                    string build = Build(childNode);
                    build = $"{node.Projection.DisplayName}.{build}";
                    sb.Append(build);
                }

                string build2 = sb.ToString();
                build2 = build2.Substring(0, build2.Length - 1);
                string ret = $"new {{ { build2} }} as {node.Projection.DisplayName},";
                return ret;
            }
            else
            {
                foreach (var childNode in node.ChildNodes)
                {
                    string build = Build(childNode);
                    build = $"{build}";
                    sb.Append(build);
                }

                string build2 = sb.ToString();
                build2 = build2.Substring(0, build2.Length - 1);
                string ret = $"{node.Projection.DisplayName}.Select(new {{ {build2} }}) as {node.Projection.DisplayName},";
                return ret;
            }
        }
    }
}
