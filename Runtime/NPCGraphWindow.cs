using Echoes.Runtime;
using UnityEditor;
using UnityEngine;

namespace Echoes.Runtime
{
    using UnityEditor;
    using UnityEngine;
    using System.Collections.Generic;

    namespace Echoes
    {
        public class NPCGraphWindow : EditorWindow
        {
            private List<EchoesNpcComponent> allNPCs;
            private Dictionary<EchoesNpcComponent, Rect> nodePositions;
            private GUIStyle nodeStyle;

            [MenuItem("Window/Echoes/NPC Graph")]
            public static void ShowWindow()
            {
                GetWindow<NPCGraphWindow>("NPC Graph");
            }

            private void OnEnable()
            {
                RefreshNPCs();
                nodeStyle = new GUIStyle(EditorStyles.helpBox);
                nodeStyle.alignment = TextAnchor.MiddleCenter;
                nodeStyle.fontSize = 14;
                nodeStyle.fontStyle = FontStyle.Bold;
            }

            private void RefreshNPCs()
            {
                // Find all NPC assets in the project
                allNPCs = EchoesGlobal.GetAllNPCs();
                nodePositions = new Dictionary<EchoesNpcComponent, Rect>();

                Vector2 center = new Vector2(500, 300); // where the "circle" is centered
                float radius = 200f; // distance from center
                int n = allNPCs.Count;

                for (int i = 0; i < n; i++)
                {
                    float angle = (2 * Mathf.PI / n) * i; // angle step
                    float x = center.x + Mathf.Cos(angle) * radius;
                    float y = center.y + Mathf.Sin(angle) * radius;
                    nodePositions[allNPCs[i]] = new Rect(x - 75, y - 30, 150, 60);
                }
            }

            private void OnGUI()
            {
                if (allNPCs == null)
                {
                    RefreshNPCs();
                    return;
                }
                
                // Draw connections
                Handles.BeginGUI();
                foreach (var npc in allNPCs)
                {
                    Rect fromRect = nodePositions[npc];
                    foreach (var contact in npc.npcData.Contacts)
                    {
                        if (nodePositions.TryGetValue(contact, out Rect toRect))
                        {
                            DrawArrow(fromRect, toRect);
                        }
                    }
                }
                
                Handles.EndGUI();
                
                foreach (var kvp in nodePositions)
                {
                    EchoesNpcComponent npc = kvp.Key;
                    Rect rect = kvp.Value;
                    GUI.Box(rect, npc.name, nodeStyle);
                }

                // Refresh button
                if (GUILayout.Button("Refresh NPC Graph"))
                {
                    RefreshNPCs();
                }
            }
            
            private void DrawArrow(Rect from, Rect to)
            {
                Vector2 fromCenter = from.center;
                Vector2 toCenter = to.center;

                // Direction vector
                Vector2 dir = (toCenter - fromCenter).normalized;

                // Adjust so line starts/ends at box edge (not center)
                Vector2 fromEdge = fromCenter + dir * (from.width * 0.5f);
                Vector2 toEdge   = toCenter - dir * (to.width * 0.5f);

                // Draw line
                Handles.BeginGUI();
                Handles.color = Color.white;
                Handles.DrawLine(fromEdge, toEdge);

                // Draw arrowhead
                Vector2 perp = new Vector2(-dir.y, dir.x);
                float arrowSize = 10f;
                Vector3 p1 = toEdge;
                Vector3 p2 = toEdge - dir * arrowSize + perp * (arrowSize * 0.5f);
                Vector3 p3 = toEdge - dir * arrowSize - perp * (arrowSize * 0.5f);
                Handles.DrawAAConvexPolygon(p1, p2, p3);
                Handles.EndGUI();
            }

        }
    }
}