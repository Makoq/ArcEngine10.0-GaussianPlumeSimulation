using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Output;

namespace 地信开发大作业
{
    class ExportMap
    {

        public static void ExportView(IActiveView view, IGeometry pGeo, int OutputResolution, int Width, int Height, string ExpPath, bool bRegion)
        {
            IExport pExport = null;
            tagRECT exportRect = new tagRECT();
            IEnvelope pEnvelope = pGeo.Envelope;
            string sType = System.IO.Path.GetExtension(ExpPath);
            switch (sType)
            {
                case ".jpg":
                    pExport = new ExportJPEGClass();
                    break;
                case ".bmp":
                    pExport = new ExportBMPClass();
                    break;
                case ".gif":
                    pExport = new ExportGIFClass();
                    break;
                case ".tif":
                    pExport = new ExportTIFFClass();
                    break;
                case ".png":
                    pExport = new ExportPNGClass();
                    break;
                case ".pdf":
                    pExport = new ExportPDFClass();
                    break;
                default:
                    MessageBox.Show("没有输出格式，默认到JPEG格式");
                    pExport = new ExportJPEGClass();
                    break;
            }
            pExport.ExportFileName = ExpPath;

            exportRect.left = 0; exportRect.top = 0;
            exportRect.right = Width;
            exportRect.bottom = Height;
            if (bRegion)
            {
                view.GraphicsContainer.DeleteAllElements();
                view.Refresh();
            }
            IEnvelope envelope = new EnvelopeClass();
            envelope.PutCoords((double)exportRect.left, (double)exportRect.top, (double)exportRect.right, (double)exportRect.bottom);
            pExport.PixelBounds = envelope;
            view.Output(pExport.StartExporting(), OutputResolution, ref exportRect, pEnvelope, null);
            pExport.FinishExporting();
            pExport.Cleanup();
        }

        /// <summary>
        /// 全域导出
        /// </summary>
        /// <param name="OutputResolution">输出分辨率</param>
        /// <param name="ExpPath">输出路径</param>
        /// <param name="view">视图</param>
        public static void ExportActiveView(int OutputResolution, string ExpPath, IActiveView view)
        {
            IExport pExport = null;
            tagRECT exportRect;
            IEnvelope envelope2 = view.Extent;
            int num = (int)Math.Round(view.ScreenDisplay.DisplayTransformation.Resolution);
            string sType = System.IO.Path.GetExtension(ExpPath);
            switch (sType)
            {
                case ".jpg":
                    pExport = new ExportJPEGClass();
                    break;
                case ".bmp":
                    pExport = new ExportBMPClass();
                    break;
                case ".gif":
                    pExport = new ExportGIFClass();
                    break;
                case ".tif":
                    pExport = new ExportTIFFClass();
                    break;
                case ".png":
                    pExport = new ExportPNGClass();
                    break;
                case ".pdf":
                    pExport = new ExportPDFClass();
                    break;
                default:
                    MessageBox.Show("没有输出格式，默认到JPEG格式");
                    pExport = new ExportJPEGClass();
                    break;
            }
            pExport.ExportFileName = ExpPath;
            exportRect.left = 0; exportRect.top = 0;
            exportRect.right = (int)Math.Round((double)(view.ExportFrame.right * (((double)OutputResolution) / ((double)num))));
            exportRect.bottom = (int)Math.Round((double)(view.ExportFrame.bottom * (((double)OutputResolution) / ((double)num))));
            IEnvelope envelope = new EnvelopeClass();
            envelope.PutCoords((double)exportRect.left, (double)exportRect.top, (double)exportRect.right, (double)exportRect.bottom);
            pExport.PixelBounds = envelope;
            view.Output(pExport.StartExporting(), OutputResolution, ref exportRect, envelope2, null);
            pExport.FinishExporting();
            pExport.Cleanup();
        }





















    }
}
