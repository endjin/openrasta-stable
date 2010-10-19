#region License

/* Authors:
 *      Sebastien Lambla (seb@serialseb.com)
 * Copyright:
 *      (C) 2007-2009 Caffeine IT & naughtyProd Ltd (http://www.caffeine-it.com)
 * License:
 *      This file is distributed under the terms of the MIT License found at the end of this file.
 */
#endregion

namespace OpenRasta.Web
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;

    using OpenRasta.Contracts.Diagnostics;
    using OpenRasta.Diagnostics;
    using OpenRasta.IO;

    public class MultipartReader
    {
        private readonly BoundaryStreamReader reader;

        private string currentLine;
        private ILogger log;

        public MultipartReader(string boundary, Stream inputStream)
        {
            this.reader = new BoundaryStreamReader(boundary, inputStream, Encoding.ASCII);
        }

        public bool AtBeginBoundary
        {
            get { return this.reader.AtBoundary; }
        }

        public bool AtBoundary
        {
            get { return this.AtBeginBoundary || this.AtEndBoundary; }
        }

        public bool AtEndBoundary
        {
            get { return this.reader.AtEndBoundary; }
        }

        public ILogger Log
        {
            get { return this.log; }
            set { this.log = this.reader.Log = value; }
        }

        private IMultipartHttpEntity CurrentEntity { get; set; }

        public IEnumerable<IMultipartHttpEntity> GetParts()
        {
            if (this.AtEndBoundary)
            {
                throw new InvalidOperationException("Can only read through the enumerator once.");
            }

            this.reader.SeekToNextPart(); // seeks to the first part

            if (this.AtEndBoundary)
            {
                yield break;
            }

            while (this.ReadEntity())
            {
                yield return this.CurrentEntity;
            }

            yield break;
        }

        public void GoToNextBoundary()
        {
            this.reader.SeekToNextPart();
        }

        public bool ReadNextLine()
        {
            this.currentLine = this.reader.ReadLine();
            
            return this.currentLine != null;
        }

        private bool ReadEntity()
        {
            if (this.AtEndBoundary)
            {
                return false;
            }

            var entity = new MultipartHttpEntity();

            // TODO: Handle split headers
            while (this.ReadNextLine() && !string.IsNullOrEmpty(this.currentLine) && !this.AtBoundary && !this.AtEndBoundary)
            {
                int columnIndex = this.currentLine.IndexOf(":");
                if (columnIndex != -1)
                {
                    entity.Headers[this.currentLine.Substring(0, columnIndex).Trim()] =
                        this.currentLine.Substring(columnIndex + 1).Trim();
                }
            }
            
            if (this.currentLine == null)
            {
                return false;
            }

            if (this.currentLine.Length == 0)
            {
                entity.Stream = this.reader.GetNextPart();
            }

            this.CurrentEntity = entity;
            
            return true;
        }
    }
}

#region Full license

// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
#endregion