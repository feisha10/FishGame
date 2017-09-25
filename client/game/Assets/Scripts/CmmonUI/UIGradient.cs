using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

[AddComponentMenu("UI/Effects/UIGradient")]
public class UIGradient : BaseMeshEffect
{
    [SerializeField]
    private Color32 topColor = Color.white;
    [SerializeField]
    private Color32 bottomColor = Color.black;

    public override void ModifyMesh(VertexHelper vh)
    {
        if (!IsActive())
            return;

        
        int count = vh.currentVertCount;
        if (count > 0)
        {
            List<UIVertex> vertexList = new List<UIVertex>(count);
            for (int i = 0; i < count; i++)
            {
                var vertex = new UIVertex();
                vh.PopulateUIVertex(ref vertex, i);
                vertexList.Add(vertex);
            }


            float bottomY = vertexList[0].position.y;
            float topY = vertexList[0].position.y;

            for (int i = 1; i < count; i++)
            {
                float y = vertexList[i].position.y;
                if (y > topY)
                    topY = y;
                else if (y < bottomY)
                    bottomY = y;
            }

            float uiElementHeight = topY - bottomY;
            for (int i = 0; i < count; i++)
            {
                UIVertex uiVertex = vertexList[i];
                var color = Color32.Lerp(bottomColor, topColor, (uiVertex.position.y - bottomY)/uiElementHeight);
                uiVertex.color = color;
                vh.SetUIVertex(uiVertex, i);
            }
        }
    }
}
