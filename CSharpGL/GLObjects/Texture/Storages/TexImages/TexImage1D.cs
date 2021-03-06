﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharpGL
{
    /// <summary>
    /// Set up texture's content with 'glTexImage1D()'.
    /// </summary>
    public class TexImage1D : TexStorageBase
    {
        private int width;
        private uint format;
        private uint type;
        private TexImageDataProvider<LeveledData> dataProvider;

        /// <summary>
        /// Set up texture's content with 'glTexImage1D()'.
        /// </summary>
        /// <param name="internalFormat"></param>
        /// <param name="mipmapLevelCount"></param>
        /// <param name="border"></param>
        /// <param name="width"></param>
        /// <param name="format"></param>
        /// <param name="type"></param>
        /// <param name="dataProvider"></param>
        public TexImage1D(uint internalFormat, int mipmapLevelCount, int border, int width, uint format, uint type, LeveledDataProvider dataProvider = null)
            : base(TextureTarget.Texture1D, internalFormat, mipmapLevelCount, border)
        {
            this.width = width;
            this.format = format;
            this.type = type;
            if (dataProvider == null)
            {
                this.dataProvider = new LeveledDataProvider();
            }
            else
            {
                this.dataProvider = dataProvider;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Apply()
        {
            foreach (var item in dataProvider)
            {
                int level = item.level;
                IntPtr pixels = item.LockData();

                GL.Instance.TexImage1D(GL.GL_TEXTURE_1D, level, internalFormat, width, border, format, type, pixels);

                item.FreeData();
            }
        }
    }
}
