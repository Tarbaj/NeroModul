/*
' Copyright (c) 2026 Nero
'  All rights reserved.
' 
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED
' TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
' THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
' CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
' DEALINGS IN THE SOFTWARE.
' 
*/

using DotNetNuke.Common.Utilities;
using DotNetNuke.ComponentModel.DataAnnotations;
using DotNetNuke.Entities.Content;
using System;
using System.Web.Caching;

namespace picturpictur.Models
{
    public class UserImage
    {
        ///<summary>
        /// The ID of the image
        ///</summary>
        public int ImageId { get; set; }

        ///<summary>
        /// A string with the name of the file
        ///</summary>
        public string FileName { get; set; }

        ///<summary>
        /// An integer with the user id of the assigned user for the object
        ///</summary>
        public int UserId { get; set; }

        ///<summary>
        /// The ModuleId of where the object was created and gets displayed
        ///</summary>
        public int ModuleId { get; set; }

        ///<summary>
        /// top color of image in hex
        ///</summary>
        public string TopColorHex { get; set; }

        ///<summary>
        /// The date the object was created
        ///</summary>
        public DateTime CreatedOnDate { get; set; }
    }
}
