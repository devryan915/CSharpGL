﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace CSharpGL
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEventArgs"></typeparam>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void GUIEventHandler<TEventArgs>(object sender, TEventArgs e);

    public abstract partial class GLControl
    {
        /// <summary>
        /// 
        /// </summary>
        [Category(strGLControl)]
        public event GUIEventHandler<GUIKeyEventArgs> KeyDown;
        /// <summary>
        /// 
        /// </summary>
        [Category(strGLControl)]
        public event GUIEventHandler<GUIKeyEventArgs> KeyUp;
        /// <summary>
        /// 
        /// </summary>
        [Category(strGLControl)]
        public event GUIEventHandler<GUIMouseEventArgs> MouseDown;
        /// <summary>
        /// 
        /// </summary>
        [Category(strGLControl)]
        public event GUIEventHandler<GUIMouseEventArgs> MouseUp;
        /// <summary>
        /// 
        /// </summary>
        [Category(strGLControl)]
        public event GUIEventHandler<GUIMouseEventArgs> MouseMove;

        /// <summary>
        /// 
        /// </summary>
        [Category(strGLControl)]
        public int TabIndex { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Category(strGLControl)]
        public bool Focused { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="args"></param>
        public virtual void InvokeEvent(EventType eventType, GUIEventArgs args)
        {
            switch (eventType)
            {
                case EventType.KeyDown:
                    if (this.Focused)
                    {
                        var keyDown = this.KeyDown;
                        if (keyDown != null)
                        {
                            var e = args as GUIKeyEventArgs;
                            keyDown(this, e);
                        }
                    }
                    break;
                case EventType.KeyUp:
                    if (this.Focused)
                    {
                        var keyUp = this.KeyUp;
                        if (keyUp != null)
                        {
                            var e = args as GUIKeyEventArgs;
                            keyUp(this, e);
                        }
                    }
                    break;
                case EventType.MouseDown:
                    {
                        var mouseDown = this.MouseDown;
                        if (mouseDown != null)
                        {
                            var e = args as GUIMouseEventArgs;
                            mouseDown(this, e);
                        }
                    }
                    break;
                case EventType.MouseUp:
                    {
                        var mouseUp = this.MouseUp;
                        if (mouseUp != null)
                        {
                            var e = args as GUIMouseEventArgs;
                            mouseUp(this, e);
                        }
                    }
                    break;
                case EventType.MouseMove:
                    {
                        var mouseMove = this.MouseMove;
                        if (mouseMove != null)
                        {
                            var e = args as GUIMouseEventArgs;
                            mouseMove(this, e);
                        }
                    }
                    break;
                default:
                    throw new NotDealWithNewEnumItemException(typeof(EventType));
            }
        }

        /// <summary>
        /// Indicates whether the specified point(x, y) is inside the conrol or not?
        /// </summary>
        /// <param name="x">absolute location of x(0 -- x -- width).</param>
        /// <param name="y">absolute location of y(0 means bottom, height means top).</param>
        /// <returns></returns>
        public bool ContainsPoint(int x, int y)
        {
            if (x < this.absLeft || this.absLeft + this.Width <= x) { return false; }
            if (y < this.absBottom || this.absBottom + this.Height <= y) { return false; }

            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        public enum EventType
        {
            /// <summary>
            /// 
            /// </summary>
            KeyDown,

            /// <summary>
            /// 
            /// </summary>
            KeyUp,

            /// <summary>
            /// 
            /// </summary>
            MouseDown,

            /// <summary>
            /// 
            /// </summary>
            MouseUp,

            /// <summary>
            /// 
            /// </summary>
            MouseMove,
            // ...
        }
    }

}
