﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace CSharpGL
{

    public partial class Scene
    {

        /// <summary>
        /// Pick <see cref="RendererBase"/>s at specified positon.
        /// </summary>
        /// <param name="x">Left Down is (0, 0)</param>
        /// <param name="y">Left Down is (0, 0)</param>
        /// <param name="deltaX"></param>
        /// <param name="deltaY"></param>
        /// <param name="selectBufferLength"></param>
        /// <returns></returns>
        public List<HitTarget> Pick(int x, int y, int deltaX = 1, int deltaY = 1, int selectBufferLength = 512)
        {
            //	Create a select buffer.
            var selectBuffer = new uint[selectBufferLength];
            GL.Instance.SelectBuffer(selectBuffer.Length, selectBuffer);

            //	Enter select mode.
            GL.Instance.RenderMode(GL.GL_SELECT);
            //	Initialise the names, and add the first name.
            GL.Instance.InitNames();
            GL.Instance.PushName(0);

            var viewport = new int[4];
            GL.Instance.GetIntegerv((uint)GetTarget.Viewport, viewport);
            mat4 pickMatrix = glm.pickMatrix(new ivec2(x, y), new ivec2(deltaX, deltaY), new ivec4(viewport[0], viewport[1], viewport[2], viewport[3]));
            var arg = new LegacyPickEventArgs(pickMatrix, this, x, y);
            uint currentName = 1;
            this.RenderForPicking(this.RootElement, arg, ref currentName);
            //	Flush commands.
            GL.Instance.Flush();

            List<HitTarget> pickedRenderer = null;
            //	End selection.
            int hits = GL.Instance.RenderMode(GL.GL_RENDER);
            if (hits < 0)// select buffer is not long enough.
            {
                pickedRenderer = this.Pick(x, y, deltaX, deltaY, selectBufferLength * 2);
            }
            else
            {
                //  Create  result set.
                pickedRenderer = new List<HitTarget>();
                uint posinarray = 0;
                //  Go through each name.
                for (int hit = 0; hit < hits; hit++)
                {
                    uint nameCount = selectBuffer[posinarray++];
                    uint zNear = selectBuffer[posinarray++];
                    uint zFar = selectBuffer[posinarray++];

                    if (nameCount == 0) { continue; }

                    //	Add each hit element to the result set to the array.
                    for (int i = 0; i < nameCount; i++)
                    {
                        uint hitName = selectBuffer[posinarray++];
                        pickedRenderer.Add(new HitTarget(arg.hitMap[hitName], zNear, zFar));
                    }
                }
            }

            //  Return the result set.
            return pickedRenderer;
        }

        private void RenderForPicking(RendererBase sceneElement, LegacyPickEventArgs arg, ref uint currentName)
        {
            if (sceneElement != null)
            {
                var pickable = sceneElement as ILegacyPickable;
                if (pickable != null && pickable.LegacyPickingEnabled)
                {
                    //  Load and map the name.
                    GL.Instance.LoadName(currentName);
                    arg.hitMap[currentName] = sceneElement;

                    pickable.RenderForLegacyPicking(arg);

                    //  Increment the name.
                    currentName++;
                }

                var node = sceneElement as ITreeNode;
                if (node != null)
                {
                    foreach (var item in node.Children)
                    {
                        this.RenderForPicking(item as RendererBase, arg, ref currentName);
                    }
                }
            }
        }
    }
}