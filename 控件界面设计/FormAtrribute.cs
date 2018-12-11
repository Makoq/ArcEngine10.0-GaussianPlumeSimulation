using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System;
using System.Data;
using System.Windows.Forms;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Carto;

using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.SystemUI;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.DataSourcesFile;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.DataSourcesRaster;
using System.IO;


namespace 地信开发大作业
{
    public partial class FormAtrribute : Form
    { 
        
        private IFeatureLayer _curFeatureLayer;



        private string FieldName;//删除属性字段名称
        private ILayer pLayer;//打开属性表的图层

        private IFeatureLayer pFeatureLayer;
        private IFeatureClass pFeatureClass;
        private ILayerFields pLayerFields;
        private IFields pFields;
        private IField pField;
        AxMapControl a;


        public FormAtrribute(AxMapControl ax)
        {
            InitializeComponent();

            pLayer = ax.get_Layer(0);
            a = ax;
        }
        
        
        #region 封装的方法
        public IFeatureLayer CurFeatureLayer
        {
            get { return _curFeatureLayer; }
            set { _curFeatureLayer = value; }
        }
        public void InitUI()
        {
            if (_curFeatureLayer == null) return;

            IFeature pFeature = null;
            DataTable pFeatDT = new DataTable(); //创建数据表
            DataRow pDataRow = null; //数据表行变量
            DataColumn pDataCol = null; //数据表列变量
            IField pField = null;
            for (int i = 0; i < _curFeatureLayer.FeatureClass.Fields.FieldCount; i++)
            {
                pDataCol = new DataColumn();
                pField = _curFeatureLayer.FeatureClass.Fields.get_Field(i);
                pDataCol.ColumnName = pField.AliasName; //获取字段名作为列标题
                pDataCol.DataType = Type.GetType("System.Object");//定义列字段类型
                pFeatDT.Columns.Add(pDataCol); //在数据表中添加字段信息
            }

            IFeatureCursor pFeatureCursor = _curFeatureLayer.Search(null, true);
            pFeature = pFeatureCursor.NextFeature();
            while (pFeature != null)
            {
                pDataRow = pFeatDT.NewRow();
                //获取字段属性
                for (int k = 0; k < pFeatDT.Columns.Count; k++)
                {
                    pDataRow[k] = pFeature.get_Value(k);
                }

                pFeatDT.Rows.Add(pDataRow); //在数据表中添加字段属性信息
                pFeature = pFeatureCursor.NextFeature();
            }
            //释放指针
            System.Runtime.InteropServices.Marshal.ReleaseComObject(pFeatureCursor);

            //dataGridAttribute.BeginInit();
            dataGridView1.DataSource = pFeatDT;
            //dataGridAttribute.EndInit();
        }
        #endregion
    





        private void FormAtrribute_Load(object sender, EventArgs e)
        {
            try
            {
                pFeatureLayer = pLayer as IFeatureLayer;
                pFeatureClass = pFeatureLayer.FeatureClass;
                pLayerFields = pFeatureLayer as ILayerFields;
                DataSet ds = new DataSet("dsTest");
                DataTable dt = new DataTable(pFeatureLayer.Name);
                DataColumn dc = null;
                for (int i = 0; i < pLayerFields.FieldCount; i++)
                {
                    dc = new DataColumn(pLayerFields.get_Field(i).Name);
                    //if (pLayerFields.get_Field(i).Editable == true)
                    //    dc.ReadOnly = false;
                    //else
                    //    dc.ReadOnly = true;

                    dt.Columns.Add(dc);
                    dc = null;
                }
                IFeatureCursor pFeatureCursor = pFeatureClass.Search(null, false);
                IFeature pFeature = pFeatureCursor.NextFeature();
                while (pFeature != null)
                {
                    DataRow dr = dt.NewRow();
                    for (int j = 0; j < pLayerFields.FieldCount; j++)
                    {
                        if (pLayerFields.FindField(pFeatureClass.ShapeFieldName) == j)
                        {
                            dr[j] = pFeatureClass.ShapeType.ToString();
                        }
                        else
                        {
                            dr[j] = pFeature.get_Value(j);
                        }
                    }
                    dt.Rows.Add(dr);
                    pFeature = pFeatureCursor.NextFeature();
                }
                dataGridView1.DataSource = dt;
            }
            catch (Exception exc)
            {
                MessageBox.Show("读取属性表失败：" + exc.Message);
                this.Dispose();
            }
            finally
            {
                //this.Close();
            }
        }

        private void dataGridView1_RowHeaderMouseClick(object sender, System.Windows.Forms.DataGridViewCellMouseEventArgs e)
        {
            IQueryFilter pQuery = new QueryFilterClass();

            //读取表中左第一列列名和当前选中行做第一列字段名
            string col = this.dataGridView1.Columns[0].Name;
            string val = this.dataGridView1.CurrentCell.OwningRow.Cells[0].Value.ToString();
            //设置高亮要素的查询条件

            pQuery.WhereClause = col + "=" + val;
            //a.layer是我当前TOCControl控件中选中的图层
            IFeatureLayer pFeatureLayer = pLayer as IFeatureLayer;
            IFeatureSelection pFeatSelection = pFeatureLayer as IFeatureSelection;


            //查询出被选中Features要素   
            pFeatSelection.SelectFeatures(pQuery, esriSelectionResultEnum.esriSelectionResultNew, false);
            // 刷新         
            a.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection, null, null);
       
        }
   
    
    
    
    
    
    }
}
