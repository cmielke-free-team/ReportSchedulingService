using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Xml;

namespace Emdat.InVision.SSRSExecution
{
    public class HtmlRenderingDeviceInfo : Emdat.InVision.SSRSExecution.IRenderingDeviceInfo
    {
        /// <summary>
        /// The bookmark ID to jump to in the report.
        /// </summary>        
        public string BookmarkID { get; set; }

        /// <summary>
        /// Indicates whether to show or hide the report document map. The 
        /// default value of this parameter is true.
        /// </summary>        
        public string DocMap { get; set; }

        /// <summary>
        /// Indicates whether the report should be enclosed in a table 
        /// structure which constricts horizontal size.
        /// </summary>        
        public string ExpandContent { get; set; }

        /// <summary>
        /// The text to search for in the report. The default value of this 
        /// parameter is an empty string.
        /// </summary>        
        public string FindString { get; set; }

        /// <summary>
        /// Gets a particular icon for the HTML Viewer user interface.
        /// </summary>        
        public string GetImage { get; set; }

        /// <summary>
        /// Indicates whether an HTML fragment is created in place of a full 
        /// HTML document. An HTML fragment includes the report content in a 
        /// TABLE element and omits the HTML and BODY elements. The default 
        /// value is false. If you are rendering to HTML using the Render 
        /// method of the SOAP API, you need to set this device information to 
        /// true if you are rendering a report with images. Rendering using 
        /// SOAP with the HTMLFragment property set to true creates URLs 
        /// containing session information that can be used to properly request 
        /// images. The images must be uploaded resources in the report server 
        /// database.
        /// </summary>        
        public string HTMLFragment { get; set; }

        /// <summary>
        /// Indicates whether JavaScript is supported in the rendered report. 
        /// The default value is true.
        /// </summary>
        public string JavaScript { get; set; }

        /// <summary>
        /// The target for hyperlinks in the report. You can target a window 
        /// or frame by providing the name of the window, like 
        /// LinkTarget=window_name, or you can target a new window using 
        /// LinkTarget=_blank. Other valid target names include _self, _parent, 
        /// and _top.
        /// </summary>
        public string LinkTarget { get; set; }

        /// <summary>
        /// Indicates that only shared styles for currently rendered page are 
        /// generated.
        /// </summary>
        public string OnlyVisibleStyles { get; set; }

        /// <summary>
        /// Indicates whether to show or hide the parameters area of the 
        /// toolbar. If you set this parameter to a value of true, the 
        /// parameters area of the toolbar is displayed. The default value of 
        /// this parameter is true.
        /// </summary>
        public string Parameters { get; set; }

        /// <summary>
        /// The page number of the report to render. A value of 0 indicates 
        /// that all sections of the report are rendered. The default value is 
        /// 1.
        /// </summary>
        public string Section { get; set; }

        /// <summary>
        /// The path used for prefixing the value of the src attribute of the 
        /// IMG element in the HTML report returned by the report server. By 
        /// default, the report server provides the path. You can use this 
        /// setting to specify a root path for the images in a report (for 
        /// example, http://&lt;servername&gt;/resources/companyimages).
        /// </summary>
        public string StreamRoot { get; set; }

        /// <summary>
        /// Indicates whether styles and scripts are created as a separate 
        /// stream instead of in the document. The default value is false.
        /// </summary>
        public string StyleStream { get; set; }

        /// <summary>
        /// Indicates whether to show or hide the toolbar. The default of this 
        /// parameter is true. If the value of this parameter is false, all 
        /// remaining options (except the document map) are ignored. If you 
        /// omit this parameter, the toolbar is automatically displayed for 
        /// rendering formats that support it.
        /// </summary>
        /// <remarks>
        /// The Report Viewer toolbar is rendered when you use URL access to 
        /// render a report. The toolbar is not rendered through the SOAP API. 
        /// However, the Toolbar device information setting affects the way 
        /// that the report is displayed when using the SOAP Render method. If 
        /// the value of this parameter is true when using SOAP to render to 
        /// HTML, only the first section of the report is rendered. If the 
        /// value is false, the entire HTML report is rendered as a single 
        /// HTML page.
        /// </remarks>
        public string Toolbar { get; set; }

        /// <summary>
        /// The report zoom value as an integer percentage or a string 
        /// constant. Standard string values include Page Width and Whole Page. 
        /// This parameter is ignored by versions of Microsoft Internet Explorer 
        /// earlier than Internet Explorer 5.0 and all non-Microsoft browsers. 
        /// The default value of this parameter is 100.
        /// </summary>
        public string Zoom { get; set; }

        /// <summary>
        /// Return the device info as an XML string.
        /// </summary>
        /// <returns></returns>
        public string ToXmlString()
        {
            StringBuilder sb = new StringBuilder();
            XmlWriterSettings xws = new XmlWriterSettings();
            xws.CloseOutput = true;
            xws.ConformanceLevel = ConformanceLevel.Fragment;
            xws.Indent = false;
            xws.OmitXmlDeclaration = true;
            using (XmlWriter xw = XmlWriter.Create(sb, xws))
            {
                xw.WriteStartElement("DeviceInfo");
                if (this.BookmarkID != null) { xw.WriteElementString("BookmarkID", this.BookmarkID); }
                if (this.DocMap != null) { xw.WriteElementString("DocMap", this.DocMap); }
                if (this.ExpandContent != null) { xw.WriteElementString("ExpandContent", this.ExpandContent); }
                if (this.FindString != null) { xw.WriteElementString("FindString", this.FindString); }
                if (this.GetImage != null) { xw.WriteElementString("GetImage", this.GetImage); }
                if (this.HTMLFragment != null) { xw.WriteElementString("HTMLFragment", this.HTMLFragment); }
                if (this.JavaScript != null) { xw.WriteElementString("JavaScript", this.JavaScript); }
                if (this.LinkTarget != null) { xw.WriteElementString("LinkTarget", this.LinkTarget); }
                if (this.OnlyVisibleStyles != null) { xw.WriteElementString("OnlyVisibleStyles", this.OnlyVisibleStyles); }
                if (this.Parameters != null) { xw.WriteElementString("Parameters", this.Parameters); }
                if (this.Section != null) { xw.WriteElementString("Section", this.Section); }
                if (this.StreamRoot != null) { xw.WriteElementString("StreamRoot", this.StreamRoot); }
                if (this.StyleStream != null) { xw.WriteElementString("StyleStream", this.StyleStream); }
                if (this.Toolbar != null) { xw.WriteElementString("Toolbar", this.Toolbar); }
                if (this.Zoom != null) { xw.WriteElementString("Zoom", this.Zoom); }
                xw.WriteEndElement();
                xw.Close();
            }
            return sb.ToString();
        }
    }
}
