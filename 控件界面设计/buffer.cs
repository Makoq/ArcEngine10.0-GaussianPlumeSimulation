using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.SystemUI;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesFile;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.DataSourcesRaster;
using System.IO;

using ESRI.ArcGIS.Geoprocessor;

namespace 地信开发大作业
{
    public partial class buffer : Form
    {
        AxMapControl aMap;

        public buffer(AxMapControl ax)
        {
            InitializeComponent();
            aMap = ax;
        }
        private void buffer_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string text;
            text = textBox1.Text;


            Geoprocessor GP = new Geoprocessor();
            GP.OverwriteOutput = true;
            ESRI.ArcGIS.AnalysisTools.Buffer pBuffer = new ESRI.ArcGIS.AnalysisTools.Buffer();

            ILayer pLayer = aMap.get_Layer(0);
            IFeatureLayer featLayer = pLayer as IFeatureLayer;

            pBuffer.in_features = featLayer;
            string filepath = @"c:\temp";

            pBuffer.out_feature_class = filepath + "\\" + pLayer.Name + ".shp";

            pBuffer.buffer_distance_or_field = text;
            pBuffer.dissolve_option = "ALL";

            GP.Execute(pBuffer, null);

            aMap.AddShapeFile(filepath, pLayer.Name);

            aMap.MoveLayerTo(1, 0);

        }
   
    
    
    
    
    
    
    
    
    
    
    
    
    
    }
}
