using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.esriSystem;
using System.IO;

using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.DataSourcesFile;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.DataSourcesRaster;
using ESRI.ArcGIS.Display;

using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.SystemUI;
using ESRI.ArcGIS.CatalogUI;
using ESRI.ArcGIS.Output;
using ESRI.ArcGIS.NetworkAnalyst;

using 地信开发大作业;
using stdole;
using System.Diagnostics;

using 地信开发大作业.Class;
using 地信开发大作业.Class.EnumType;
using ESRI.ArcGIS.Geoprocessor;
using 地信开发大作业.控件界面设计 ;
namespace 地信开发大作业
{
    public partial class Form1 : Form
    {
        #region 变量声明及定义




        IPoint pPoint = null;//鼠标点击点
        //地图导出
        private frmPrintPreview frmPrintPreview = null; // 打印 
        private FormExportMap frmExpMap = null;
        private IEnvelope pEnv;             //记录数据视图的Extent
        //鹰眼同步参数
        private bool bCanDrag;              //鹰眼地图上的矩形框可移动的标志
        private IPoint pMoveRectPoint;
        //TOC左键移动图层参数
        private int toIndex;
        private Point pMoveLayerPoint;
        //toc右键参数
        ILayer SelectedLayer_TOC = null;
        private ILayer pMoveLayer;
        //右键属性表参数

        IFeatureLayer pTocFeatureLayer = null;            //点击的要素图层
        private FormAtrribute frmAttribute = null;
        //定义按钮变量
        string pMouseOperate = "";
        //长度，面积量算
        private FormMeasureResult frmMeasureResult = null;   //量算结果窗体
        private INewLineFeedback pNewLineFeedback;           //追踪线对象
        private INewPolygonFeedback pNewPolygonFeedback;     //追踪面对象
        private IPoint pPointPt = null;                      //鼠标点击点
        private IPoint pMovePt = null;                       //鼠标移动时的当前点
        private double dToltalLength = 0;                    //量测总长度
        private double dSegmentLength = 0;                   //片段距离
        private IPointCollection pAreaPointCol = new MultipointClass();  //面积量算时画的点进行存储；  


        //片段距离


        private string sMapUnits = "未知单位";             //地图单位变量
        private object missing = Type.Missing;
        //地图操作
        private OperatePageLayout m_OperatePageLayout = null;
        //地图整饰变量
        private frmSymbol frmSym = null;
        private EnumMapSurroundType _enumMapSurType = EnumMapSurroundType.None;
        private IStyleGalleryItem pStyleGalleryItem;
        private IPoint m_PointPt = null;
        private IPoint m_MovePt = null;
        private INewEnvelopeFeedback pNewEnvelopeFeedback;
        //最短路径相关变量
        IMapDocument mapDocument;
        public IFeatureWorkspace pFWorkspace;
        private string path = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
        //空间查询
        IGeometry selectGeo;
        #endregion

        public Form1()
        {
            ESRI.ArcGIS.RuntimeManager.Bind(ESRI.ArcGIS.ProductCode.EngineOrDesktop);
            InitializeComponent();
            m_OperatePageLayout = new OperatePageLayout();
        }
        private void frmShortPathSolver_Load(object sender, EventArgs e)
        {
        }

        #region 最短路径分析
        //加载站点
        private void addStops_Click(object sender, EventArgs e)
        {
            ICommand pCommand;
            pCommand = new AddNetStopsTool();
            pCommand.OnCreate(mMapControl.Object);
            mMapControl.CurrentTool = pCommand as ITool;
            pCommand = null;
        }
        //加载障碍点
        private void addBarriers_Click(object sender, EventArgs e)
        {
            ICommand pCommand;
            pCommand = new AddNetBarriesTool();
            pCommand.OnCreate(mMapControl.Object);
            mMapControl.CurrentTool = pCommand as ITool;
            pCommand = null;
        }
        //最短路径分析
        private void routeSolver_Click(object sender, EventArgs e)
        {
            ICommand pCommand;
            pCommand = new ShortPathSolveCommand();
            pCommand.OnCreate(mMapControl.Object);
            pCommand.OnClick();
            pCommand = null;
        }
        //清除分析
        private void 清除分析ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mMapControl.CurrentTool = null;
            try
            {
                string name = NetWorkAnalysClass.getPath(path) + "\\data\\HuanbaoGeodatabase.gdb";
                //打开工作空间
                pFWorkspace = NetWorkAnalysClass.OpenWorkspace(name) as IFeatureWorkspace;
                IGraphicsContainer pGrap = this.mMapControl.ActiveView as IGraphicsContainer;
                pGrap.DeleteAllElements();//删除所添加的图片要素
                IFeatureClass inputFClass = pFWorkspace.OpenFeatureClass("Stops");
                //删除站点要素
                if (inputFClass.FeatureCount(null) > 0)
                {
                    ITable pTable = inputFClass as ITable;
                    pTable.DeleteSearchedRows(null);
                }
                IFeatureClass barriesFClass = pFWorkspace.OpenFeatureClass("Barries");//删除障碍点要素
                if (barriesFClass.FeatureCount(null) > 0)
                {
                    ITable pTable = barriesFClass as ITable;
                    pTable.DeleteSearchedRows(null);
                }
                for (int i = 0; i < mMapControl.LayerCount; i++)//删除分析结果
                {
                    ILayer pLayer = mMapControl.get_Layer(i);
                    if (pLayer.Name == ShortPathSolveCommand.m_NAContext.Solver.DisplayName)
                    {
                        mMapControl.DeleteLayer(i);
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            this.mMapControl.Refresh();
        }
        #endregion

        #region 数据加载
        private void 加载MXDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mapDocument = new MapDocumentClass();
            try
            {
                System.Windows.Forms.OpenFileDialog openFileDialog;
                openFileDialog = new OpenFileDialog();
                openFileDialog.Title = "打开图层文件";
                openFileDialog.Filter = "map documents(*.mxd)|*.mxd";
                openFileDialog.ShowDialog();
                string filePath = openFileDialog.FileName;
                mapDocument.Open(filePath, "");
                for (int i = 0; i < mapDocument.MapCount; i++)
                {
                    mMapControl.Map = mapDocument.get_Map(i);
                }
                mMapControl.Refresh();
          

            }
            catch (Exception ex)
            {
                MessageBox.Show("加载失败" + ex.ToString());
            }
        }
        private void 加载Shpfile数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            ClearAllData();
            try
            {
                OpenFileDialog pOpenFileDialog = new OpenFileDialog();
                pOpenFileDialog.CheckFileExists = true;
                pOpenFileDialog.Title = "打开Shape文件";
                pOpenFileDialog.Filter = "Shape文件（*.shp）|*.shp";
                pOpenFileDialog.ShowDialog();

                ////获取文件路径
                //FileInfo pFileInfo = new FileInfo(pOpenFileDialog.FileName);
                //string pPath = pOpenFileDialog.FileName.Substring(0, pOpenFileDialog.FileName.Length - pFileInfo.Name.Length);
                //mainMapControl.AddShapeFile(pPath, pFileInfo.Name);

                IWorkspaceFactory pWorkspaceFactory;
                IFeatureWorkspace pFeatureWorkspace;
                IFeatureLayer pFeatureLayer;

                string pFullPath = pOpenFileDialog.FileName;
                if (pFullPath == "") return;

                int pIndex = pFullPath.LastIndexOf("\\");
                string pFilePath = pFullPath.Substring(0, pIndex); //文件路径
                string pFileName = pFullPath.Substring(pIndex + 1); //文件名

                //实例化ShapefileWorkspaceFactory工作空间，打开Shape文件
                pWorkspaceFactory = new ShapefileWorkspaceFactory();
                pFeatureWorkspace = (IFeatureWorkspace)pWorkspaceFactory.OpenFromFile(pFilePath, 0);
                //创建并实例化要素集
                IFeatureClass pFeatureClass = pFeatureWorkspace.OpenFeatureClass(pFileName);
                pFeatureLayer = new FeatureLayer();
                pFeatureLayer.FeatureClass = pFeatureClass;
                pFeatureLayer.Name = pFeatureLayer.FeatureClass.AliasName;

                ClearAllData();    //新增删除数据

                mMapControl.Map.AddLayer(pFeatureLayer);
                mMapControl.ActiveView.Refresh();
                //同步鹰眼
                SynchronizeEagleEye();
            }
            catch (Exception ex)
            {
                MessageBox.Show("图层加载失败！" + ex.Message);
            }

        }
        #endregion

        #region 鹰眼实现

        private void SynchronizeEagleEye()
        {
            if (EagleEyeMapControl2.LayerCount > 0)
            {
                EagleEyeMapControl2.ClearLayers();
            }
            //设置鹰眼和主地图的坐标系统一致
            EagleEyeMapControl2.SpatialReference = mMapControl.SpatialReference;
            for (int i = mMapControl.LayerCount - 1; i >= 0; i--)
            {
                //使鹰眼视图与数据视图的图层上下顺序保持一致
                ILayer pLayer = mMapControl.get_Layer(i);
                if (pLayer is IGroupLayer || pLayer is ICompositeLayer)
                {
                    ICompositeLayer pCompositeLayer = (ICompositeLayer)pLayer;
                    for (int j = pCompositeLayer.Count - 1; j >= 0; j--)
                    {
                        ILayer pSubLayer = pCompositeLayer.get_Layer(j);
                        IFeatureLayer pFeatureLayer = pSubLayer as IFeatureLayer;
                        if (pFeatureLayer != null)
                        {
                            //由于鹰眼地图较小，所以过滤点图层不添加
                            if (pFeatureLayer.FeatureClass.ShapeType != esriGeometryType.esriGeometryPoint
                                && pFeatureLayer.FeatureClass.ShapeType != esriGeometryType.esriGeometryMultipoint)
                            {
                                EagleEyeMapControl2.AddLayer(pLayer);
                            }
                        }
                    }
                }
                else
                {
                    IFeatureLayer pFeatureLayer = pLayer as IFeatureLayer;
                    if (pFeatureLayer != null)
                    {
                        if (pFeatureLayer.FeatureClass.ShapeType != esriGeometryType.esriGeometryPoint
                            && pFeatureLayer.FeatureClass.ShapeType != esriGeometryType.esriGeometryMultipoint)
                        {
                            EagleEyeMapControl2.AddLayer(pLayer);
                        }
                    }
                }
                //设置鹰眼地图全图显示  
                EagleEyeMapControl2.Extent = mMapControl.FullExtent;
                pEnv = mMapControl.Extent as IEnvelope;
                DrawRectangle(pEnv);
                EagleEyeMapControl2.ActiveView.Refresh();
            }
        }
        //在鹰眼地图上面画矩形框
        private void DrawRectangle(IEnvelope pEnvelope)
        {
            //在绘制前，清除鹰眼中之前绘制的矩形框
            IGraphicsContainer pGraphicsContainer = EagleEyeMapControl2.Map as IGraphicsContainer;
            IActiveView pActiveView = pGraphicsContainer as IActiveView;
            pGraphicsContainer.DeleteAllElements();
            //得到当前视图范围
            IRectangleElement pRectangleElement = new RectangleElementClass();
            IElement pElement = pRectangleElement as IElement;
            pElement.Geometry = pEnvelope;
            //设置矩形框（实质为中间透明度面）
            IRgbColor pColor = new RgbColorClass();
            pColor = GetRgbColor(255, 0, 0);
            pColor.Transparency = 255;
            ILineSymbol pOutLine = new SimpleLineSymbolClass();
            pOutLine.Width = 2;
            pOutLine.Color = pColor;

            IFillSymbol pFillSymbol = new SimpleFillSymbolClass();
            pColor = new RgbColorClass();
            pColor.Transparency = 0;
            pFillSymbol.Color = pColor;
            pFillSymbol.Outline = pOutLine;
            //向鹰眼中添加矩形框
            IFillShapeElement pFillShapeElement = pElement as IFillShapeElement;
            pFillShapeElement.Symbol = pFillSymbol;
            pGraphicsContainer.AddElement((IElement)pFillShapeElement, 0);
            //刷新
            pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
        }    
       
        private void mMapControl_OnExtentUpdated(object sender, IMapControlEvents2_OnExtentUpdatedEvent e)
        {
            //得到当前视图范围
            pEnv = (IEnvelope)e.newEnvelope;
            DrawRectangle(pEnv);
        }

        private void mMapControl_OnMapReplaced(object sender, IMapControlEvents2_OnMapReplacedEvent e)
        {
            SynchronizeEagleEye();
        }

        private void EagleEyeMapControl2_OnMouseDown(object sender, IMapControlEvents2_OnMouseDownEvent e)
        {
            if (EagleEyeMapControl2.Map.LayerCount > 0)
            {
                //按下鼠标左键移动矩形框
                if (e.button == 1)
                {
                    //如果指针落在鹰眼的矩形框中，标记可移动
                    if (e.mapX > pEnv.XMin && e.mapY > pEnv.YMin && e.mapX < pEnv.XMax && e.mapY < pEnv.YMax)
                    {
                        bCanDrag = true;
                    }
                    pMoveRectPoint = new PointClass();
                    pMoveRectPoint.PutCoords(e.mapX, e.mapY);  //记录点击的第一个点的坐标
                }
                //按下鼠标右键绘制矩形框
                else if (e.button == 2)
                {
                    IEnvelope pEnvelope = EagleEyeMapControl2.TrackRectangle();

                    IPoint pTempPoint = new PointClass();
                    pTempPoint.PutCoords(pEnvelope.XMin + pEnvelope.Width / 2, pEnvelope.YMin + pEnvelope.Height / 2);
                    mMapControl.Extent = pEnvelope;
                    //矩形框的高宽和数据试图的高宽不一定成正比，这里做一个中心调整
                    mMapControl.CenterAt(pTempPoint);
                }
            }
        }

        private void EagleEyeMapControl2_OnMouseMove(object sender, IMapControlEvents2_OnMouseMoveEvent e)
        {
            if (EagleEyeMapControl2.Map.LayerCount > 0)
            {
                if (e.mapX > pEnv.XMin && e.mapY > pEnv.YMin && e.mapX < pEnv.XMax && e.mapY < pEnv.YMax)
                {
                    //如果鼠标移动到矩形框中，鼠标换成小手，表示可以拖动
                    EagleEyeMapControl2.MousePointer = esriControlsMousePointer.esriPointerHand;
                    if (e.button == 2)  //如果在内部按下鼠标右键，将鼠标演示设置为默认样式
                    {
                        EagleEyeMapControl2.MousePointer = esriControlsMousePointer.esriPointerDefault;
                    }
                }
                else
                {
                    //在其他位置将鼠标设为默认的样式
                    EagleEyeMapControl2.MousePointer = esriControlsMousePointer.esriPointerDefault;
                }

                if (bCanDrag)
                {
                    double Dx, Dy;  //记录鼠标移动的距离
                    Dx = e.mapX - pMoveRectPoint.X;
                    Dy = e.mapY - pMoveRectPoint.Y;
                    pEnv.Offset(Dx, Dy); //根据偏移量更改 pEnv 位置
                    pMoveRectPoint.PutCoords(e.mapX, e.mapY);
                    DrawRectangle(pEnv);
                    mMapControl.Extent = pEnv;
                }
            }
        }

        private void EagleEyeMapControl2_OnMouseUp(object sender, IMapControlEvents2_OnMouseUpEvent e)
        {
            if (e.button == 1 && pMoveRectPoint != null)
            {
                if (e.mapX == pMoveRectPoint.X && e.mapY == pMoveRectPoint.Y)
                {
                    mMapControl.CenterAt(pMoveRectPoint);
                }
                bCanDrag = false;
            }
        }
  
        #endregion

        #region RGB相关函数设置

        private void ClearAllData()
        {
            if (mMapControl.Map != null && mMapControl.Map.LayerCount > 0)
            {
                //新建mainMapControl中Map
                IMap dataMap = new MapClass();
                dataMap.Name = "Map";
                mMapControl.DocumentFilename = string.Empty;
                mMapControl.Map = dataMap;

                //新建EagleEyeMapControl中Map
                IMap eagleEyeMap = new MapClass();
                eagleEyeMap.Name = "eagleEyeMap";
                EagleEyeMapControl2.DocumentFilename = string.Empty;
                EagleEyeMapControl2.Map = eagleEyeMap;
            }
        }
        /// <summary>
        /// 获取RGB颜色
        /// </summary>
        /// <param name="intR">红</param>
        /// <param name="intG">绿</param>
        /// <param name="intB">蓝</param>
        /// <returns></returns>
        private IRgbColor GetRgbColor(int intR, int intG, int intB)
        {
            IRgbColor pRgbColor = null;
            if (intR < 0 || intR > 255 || intG < 0 || intG > 255 || intB < 0 || intB > 255)
            {
                return pRgbColor;
            }
            pRgbColor = new RgbColorClass();
            pRgbColor.Red = intR;
            pRgbColor.Green = intG;
            pRgbColor.Blue = intB;
            return pRgbColor;
        }
        /// <summary>
        /// 获取地图单位
        /// </summary>
        /// <param name="_esriMapUnit"></param>
        /// <returns></returns>
        private string GetMapUnit(esriUnits _esriMapUnit)
        {
            string sMapUnits = string.Empty;
            switch (_esriMapUnit)
            {
                case esriUnits.esriCentimeters:
                    sMapUnits = "厘米";
                    break;
                case esriUnits.esriDecimalDegrees:
                    sMapUnits = "十进制";
                    break;
                case esriUnits.esriDecimeters:
                    sMapUnits = "分米";
                    break;
                case esriUnits.esriFeet:
                    sMapUnits = "尺";
                    break;
                case esriUnits.esriInches:
                    sMapUnits = "英寸";
                    break;
                case esriUnits.esriKilometers:
                    sMapUnits = "千米";
                    break;
                case esriUnits.esriMeters:
                    sMapUnits = "米";
                    break;
                case esriUnits.esriMiles:
                    sMapUnits = "英里";
                    break;
                case esriUnits.esriMillimeters:
                    sMapUnits = "毫米";
                    break;
                case esriUnits.esriNauticalMiles:
                    sMapUnits = "海里";
                    break;
                case esriUnits.esriPoints:
                    sMapUnits = "点";
                    break;
                case esriUnits.esriUnitsLast:
                    sMapUnits = "UnitsLast";
                    break;
                case esriUnits.esriUnknownUnits:
                    sMapUnits = "未知单位";
                    break;
                case esriUnits.esriYards:
                    sMapUnits = "码";
                    break;
                default:
                    break;
            }
            return sMapUnits;
        }

        /// <summary>
        /// 绘制多边形
        /// </summary>
        /// <param name="mapCtrl"></param>
        /// <returns></returns>
        public IPolygon DrawPolygon(AxMapControl mapCtrl)
        {
            IGeometry pGeometry = null;
            if (mapCtrl == null) return null;
            IRubberBand rb = new RubberPolygonClass();
            pGeometry = rb.TrackNew(mapCtrl.ActiveView.ScreenDisplay, null);
            return pGeometry as IPolygon;
        }
        #endregion

        #region 右键菜单
        private void axTOCControl1_OnMouseDown(object sender, ITOCControlEvents_OnMouseDownEvent e)
        {
            pMoveLayerPoint = new Point();

            if (e.button == 2)
            {
                esriTOCControlItem pItem = esriTOCControlItem.esriTOCControlItemNone;
                IBasicMap pMap = null;
                ILayer pLayer = null;
                object unk = null;
                object data = null;
                axTOCControl1.HitTest(e.x, e.y, ref pItem, ref pMap, ref pLayer, ref unk, ref data);
                pTocFeatureLayer = pLayer as IFeatureLayer;
                if (pItem == esriTOCControlItem.esriTOCControlItemLayer && pTocFeatureLayer != null)
                {

                    contextMenuStrip1.Show(Control.MousePosition);
                }
            }
            if (e.button == 1)
            {
                esriTOCControlItem pItem = esriTOCControlItem.esriTOCControlItemNone;
                IBasicMap pMap = null; object unk = null;
                object data = null; ILayer pLayer = null;
                axTOCControl1.HitTest(e.x, e.y, ref pItem, ref pMap, ref pLayer, ref unk, ref data);
                if (pLayer == null) return;

                pMoveLayerPoint.PutCoords(e.x, e.y);
                if (pItem == esriTOCControlItem.esriTOCControlItemLayer)
                {
                    if (pLayer is IAnnotationSublayer)
                    {
                        return;
                    }
                    else
                    {
                        pMoveLayer = pLayer;
                    }
                }
            }


        }
        #endregion

        #region 右键菜单项（打开属性表、删除）

        private void 打开属性表ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (frmAttribute == null || frmAttribute.IsDisposed)
            {
                frmAttribute = new FormAtrribute(mMapControl);
            }
            frmAttribute.CurFeatureLayer = pTocFeatureLayer;
            frmAttribute.InitUI();
            frmAttribute.ShowDialog();
        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (pTocFeatureLayer == null) return;
                DialogResult result = MessageBox.Show("是否删除[" + pTocFeatureLayer.Name + "]图层", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                if (result == DialogResult.OK)
                {
                    mMapControl.Map.DeleteLayer(pTocFeatureLayer);
                    EagleEyeMapControl2.Map.DeleteLayer(pTocFeatureLayer);
                }
                mMapControl.ActiveView.Refresh();
                EagleEyeMapControl2.ActiveView.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #endregion

        #region 图层拖拽移动
        private void axTOCControl1_OnMouseUp(object sender, ITOCControlEvents_OnMouseUpEvent e)
        {
            if (e.button == 1 && pMoveLayer != null && pMoveLayerPoint.Y != e.y)
            {
                esriTOCControlItem pItem = esriTOCControlItem.esriTOCControlItemNone;
                IBasicMap pBasicMap = null; object unk = null;
                object data = null; ILayer pLayer = null;
                axTOCControl1.HitTest(e.x, e.y, ref pItem, ref pBasicMap, ref pLayer, ref unk, ref data);
                IMap pMap = mMapControl.ActiveView.FocusMap;
                if (pItem == esriTOCControlItem.esriTOCControlItemLayer || pLayer != null)
                {
                    if (pMoveLayer != pLayer)
                    {
                        ILayer pTempLayer;
                        //获得鼠标弹起时所在图层的索引号
                        for (int i = 0; i < pMap.LayerCount; i++)
                        {
                            pTempLayer = pMap.get_Layer(i);
                            if (pTempLayer == pLayer)
                            {
                                toIndex = i;
                            }
                        }
                    }
                }
                //移动到最前面
                else if (pItem == esriTOCControlItem.esriTOCControlItemMap)
                {
                    toIndex = 0;
                }
                //移动到最后面
                else if (pItem == esriTOCControlItem.esriTOCControlItemNone)
                {
                    toIndex = pMap.LayerCount - 1;
                }
                pMap.MoveLayer(pMoveLayer, toIndex);
                mMapControl.ActiveView.Refresh();
                axTOCControl1.Update();
            }
        }
        #endregion

        #region 放大缩小全屏漫游上下视图
        //放大
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            mMapControl.CurrentTool = null;
            pMouseOperate = "ZoomIn";
            mMapControl.MousePointer = esriControlsMousePointer.esriPointerZoomIn;

        }
        //缩小
        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            mMapControl.CurrentTool = null;
            pMouseOperate = "ZoomOut";
            mMapControl.MousePointer = esriControlsMousePointer.esriPointerZoomOut;

        }
        //漫游
        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            mMapControl.CurrentTool = null;
            pMouseOperate = "Pan";
            mMapControl.MousePointer = esriControlsMousePointer.esriPointerPan;

        }
        //全屏
        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            mMapControl.Extent = mMapControl.FullExtent;

        }
        //前一视图
        IExtentStack pExtentStack;
        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            pExtentStack = mMapControl.ActiveView.ExtentStack;
            //判断是否可以回到前一视图，第一个视图没有前一视图
            if (pExtentStack.CanUndo())
            {
                pExtentStack.Undo();
                toolStripButton5.Enabled = true;
                if (!pExtentStack.CanUndo())
                {
                    toolStripButton5.Enabled = false;
                }
            }
            mMapControl.ActiveView.Refresh();
        }
        //后一视图
        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            pExtentStack = mMapControl.ActiveView.ExtentStack;
            //判断是否可以回到后一视图，最后一个视图没有后一视图
            if (pExtentStack.CanRedo())
            {
                pExtentStack.Redo();
                toolStripButton5.Enabled = true;
                if (!pExtentStack.CanRedo())
                {
                    toolStripButton5.Enabled = false;
                }
            }
            mMapControl.ActiveView.Refresh();
        }


        #endregion

        #region 布局视图与数据视图同步
        private void CopyToPageLayout()
        {
            IObjectCopy pObjectCopy = new ObjectCopyClass();
            object copyFromMap = mMapControl.Map;
            object copiedMap = pObjectCopy.Copy(copyFromMap);//复制地图到copiedMap中
            object copyToMap = axPageLayoutControl1.ActiveView.FocusMap;
            pObjectCopy.Overwrite(copiedMap, ref copyToMap); //复制地图
            axPageLayoutControl1.ActiveView.Refresh();
        }
        private void mMapControl_OnAfterScreenDraw(object sender, IMapControlEvents2_OnAfterScreenDrawEvent e)
        {
            IActiveView pActiveView = (IActiveView)axPageLayoutControl1.ActiveView.FocusMap;
            IDisplayTransformation displayTransformation = pActiveView.ScreenDisplay.DisplayTransformation;
            displayTransformation.VisibleBounds = mMapControl.Extent;
            axPageLayoutControl1.ActiveView.Refresh();
            CopyToPageLayout();
        }

        #endregion

        #region 距离、面积量测

        private void 距离量算ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mMapControl.CurrentTool = null;
            pMouseOperate = "MeasureLength";
            mMapControl.MousePointer = esriControlsMousePointer.esriPointerCrosshair;
            if (frmMeasureResult == null || frmMeasureResult.IsDisposed)
            {
                frmMeasureResult = new FormMeasureResult();
                frmMeasureResult.frmClosed += new FormMeasureResult.FormClosedEventHandler(frmMeasureResult_frmColsed);
                frmMeasureResult.lblMeasureResult.Text = "";
                frmMeasureResult.Text = "距离量测";
                frmMeasureResult.Show();
            }
            else
            {
                frmMeasureResult.Activate();
            }
        }

        private void 面积量算ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mMapControl.CurrentTool = null;
            pMouseOperate = "MeasureArea";
            mMapControl.MousePointer = esriControlsMousePointer.esriPointerCrosshair;
            if (frmMeasureResult == null || frmMeasureResult.IsDisposed)
            {
                frmMeasureResult = new FormMeasureResult();
                frmMeasureResult.frmClosed += new FormMeasureResult.FormClosedEventHandler(frmMeasureResult_frmColsed);
                frmMeasureResult.lblMeasureResult.Text = "";
                frmMeasureResult.Text = "面积量测";
                frmMeasureResult.Show();
            }
            else
            {
                frmMeasureResult.Activate();
            }
        }

        private void frmMeasureResult_frmColsed()
        {
            //清空线对象
            if (pNewLineFeedback != null)
            {
                pNewLineFeedback.Stop();
                pNewLineFeedback = null;
            }
            //清空面对象
            if (pNewPolygonFeedback != null)
            {
                pNewPolygonFeedback.Stop();
                pNewPolygonFeedback = null;
                pAreaPointCol.RemovePoints(0, pAreaPointCol.PointCount); //清空点集中所有点
            }
            //清空量算画的线、面对象
            mMapControl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewForeground, null, null);
            //结束量算功能
            pMouseOperate = string.Empty;
            mMapControl.MousePointer = esriControlsMousePointer.esriPointerDefault;
        }

        private void mMapControl_OnDoubleClick(object sender, IMapControlEvents2_OnDoubleClickEvent e)
        {
            #region 长度量算
            if (pMouseOperate == "MeasureLength")
            {
                if (frmMeasureResult != null)
                {
                    frmMeasureResult.lblMeasureResult.Text = "线段总长度为：" + dToltalLength + sMapUnits;
                }
                if (pNewLineFeedback != null)
                {
                    pNewLineFeedback.Stop();
                    pNewLineFeedback = null;
                    //清空所画的线对象
                    (mMapControl.Map as IActiveView).PartialRefresh(esriViewDrawPhase.esriViewForeground, null, null);
                }
                dToltalLength = 0;
                dSegmentLength = 0;
            }
            #endregion

            #region 面积量算
            if (pMouseOperate == "MeasureArea")
            {
                if (pNewPolygonFeedback != null)
                {
                    pNewPolygonFeedback.Stop();
                    pNewPolygonFeedback = null;
                    //清空所画的线对象
                    (mMapControl.Map as IActiveView).PartialRefresh(esriViewDrawPhase.esriViewForeground, null, null);
                }
                pAreaPointCol.RemovePoints(0, pAreaPointCol.PointCount); //清空点集中所有点
            }
            #endregion

        }

        #endregion

        #region 属性查询菜单项
        private void 属性查询ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //新创建属性查询窗体
            FormQueryByAttribute formQueryByAttribute = new FormQueryByAttribute();
            //将当前主窗体中MapControl控件中的Map对象赋值给FormQueryByAttribute窗体的CurrentMap属性
            formQueryByAttribute.CurrentMap = mMapControl.Map;
            //显示属性查询窗体
            formQueryByAttribute.Show();
        }
        #endregion

        #region 空间查询菜单项
        //多功能查询
        private void 多功能查询ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //新创建空间查询窗体
            FormQueryBySpatial formQueryBySpatial = new FormQueryBySpatial();
            //将当前主窗体中MapControl控件中的Map对象赋值给FormSelection窗体的CurrentMap属性
            formQueryBySpatial.CurrentMap = mMapControl.Map;
            //显示空间查询窗体
            formQueryBySpatial.Show();
        }
        //点查询
        private void 点查询ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mMapControl.MousePointer = esriControlsMousePointer.esriPointerCrosshair;

            pMouseOperate = "pointquery";
        }
        //多边形查询
        private void 多边形查询ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormAttribute2 frmAt = new FormAttribute2();

            frmAt.Show();


            mMapControl.MousePointer = esriControlsMousePointer.esriPointerCrosshair;

            selectGeo = mMapControl.TrackPolygon();
            if (null != selectGeo)
            {
                mMapControl.Map.SelectByShape(selectGeo, null, false);
                mMapControl.Refresh(esriViewDrawPhase.esriViewGeoSelection, null, null);
            }



            IFeatureLayer pFeatureLayer = mMapControl.get_Layer(0) as IFeatureLayer;
            List<IFeature> pFlist = GetSearchFeatures(pFeatureLayer, selectGeo);

            DataGridView dgv = frmAt.dataGridView2;
            dgv.RowCount = pFlist.Count + 1;

            dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Sunken;

            dgv.ColumnCount = pFlist[0].Fields.FieldCount;

            for (int m = 0; m < pFlist[0].Fields.FieldCount; m++)
            {
                dgv.Columns[m].HeaderText = pFlist[0].Fields.get_Field(m).AliasName;
            }

            for (int i = 0; i < pFlist.Count; i++)
            {

                IFeature pFeature = pFlist[i];
                for (int j = 0; j < pFeature.Fields.FieldCount; j++)
                {
                    dgv[j, i].Value = pFeature.get_Value(j).ToString();
                }

            }
        }
        //清除查询
        private void 清除查询ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IActiveView pActiveView = (IActiveView)(mMapControl.Map);
            pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection, mMapControl.get_Layer(0), null);
            mMapControl.Map.ClearSelection();
            pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection, mMapControl.get_Layer(0), null);

        }
        //多边形查询属性表内容
        public List<IFeature> GetSearchFeatures(IFeatureLayer pFeatureLayer, IGeometry pGeometry)
        {

            try
            {
                List<IFeature> pList = new List<IFeature>();

                ISpatialFilter pSpatialFilter = new SpatialFilter();
                IQueryFilter pQueryFilter = pSpatialFilter as ISpatialFilter;

                pSpatialFilter.Geometry = pGeometry;

                pSpatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelContains;

                IFeatureCursor pFeatureCursor = pFeatureLayer.Search(pQueryFilter, false);
                IFeature pFeature = pFeatureCursor.NextFeature();
                while (pFeature != null)
                {
                    pList.Add(pFeature);
                    pFeature = pFeatureCursor.NextFeature();
                }
                System.Runtime.InteropServices.Marshal.FinalReleaseComObject(pFeatureCursor);
                return pList;

            }
            catch (Exception Err)
            {
                MessageBox.Show(Err.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return null;
            }
        }

        #endregion

        #region 地图整饰菜单项

        private void frmSym_GetSelSymbolItem(ref IStyleGalleryItem pStyleItem)
        {
            pStyleGalleryItem = pStyleItem;
        }

        private void 比例尺ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _enumMapSurType = EnumMapSurroundType.ScaleBar;
            if (frmSym == null || frmSym.IsDisposed)
            {
                frmSym = new frmSymbol();
                frmSym.GetSelSymbolItem += new frmSymbol.GetSelSymbolItemEventHandler(frmSym_GetSelSymbolItem);
            }
            frmSym.EnumMapSurType = _enumMapSurType;
            frmSym.InitUI();
            frmSym.ShowDialog();
        }

        private void 指北针ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                _enumMapSurType = EnumMapSurroundType.NorthArrow;
                if (frmSym == null || frmSym.IsDisposed)
                {
                    frmSym = new frmSymbol();
                    frmSym.GetSelSymbolItem += new frmSymbol.GetSelSymbolItemEventHandler(frmSym_GetSelSymbolItem);
                }
                frmSym.EnumMapSurType = _enumMapSurType;
                frmSym.InitUI();
                frmSym.ShowDialog();
            }
            catch (Exception ex)
            {
            }
        }
        private void 图廓ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        private void 图例ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _enumMapSurType = EnumMapSurroundType.Legend;

        }

        private void axPageLayoutControl1_OnMouseDown(object sender, IPageLayoutControlEvents_OnMouseDownEvent e)
        {
            try
            {
                if (_enumMapSurType != EnumMapSurroundType.None)
                {
                    IActiveView pActiveView = null;
                    pActiveView = axPageLayoutControl1.PageLayout as IActiveView;
                    m_PointPt = pActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(e.x, e.y);
                    if (pNewEnvelopeFeedback == null)
                    {
                        pNewEnvelopeFeedback = new NewEnvelopeFeedbackClass();
                        pNewEnvelopeFeedback.Display = pActiveView.ScreenDisplay;
                        pNewEnvelopeFeedback.Start(m_PointPt);
                    }
                    else
                    {
                        pNewEnvelopeFeedback.MoveTo(m_PointPt);
                    }

                }
            }
            catch
            {
            }
        }

        private void axPageLayoutControl1_OnMouseMove(object sender, IPageLayoutControlEvents_OnMouseMoveEvent e)
        {
            try
            {
                if (_enumMapSurType != EnumMapSurroundType.None)
                {
                    if (pNewEnvelopeFeedback != null)
                    {
                        m_MovePt = (axPageLayoutControl1.PageLayout as IActiveView).ScreenDisplay.DisplayTransformation.ToMapPoint(e.x, e.y);
                        pNewEnvelopeFeedback.MoveTo(m_MovePt);
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void axPageLayoutControl1_OnMouseUp(object sender, IPageLayoutControlEvents_OnMouseUpEvent e)
        {
            if (_enumMapSurType != EnumMapSurroundType.None)
            {
                if (pNewEnvelopeFeedback != null)
                {
                    IActiveView pActiveView = null;
                    pActiveView = axPageLayoutControl1.PageLayout as IActiveView;
                    IEnvelope pEnvelope = pNewEnvelopeFeedback.Stop();
                    AddMapSurround(pActiveView, _enumMapSurType, pEnvelope);
                    pNewEnvelopeFeedback = null;
                    _enumMapSurType = EnumMapSurroundType.None;
                }
            }
        }

        /// <summary>
        /// 添加地图整饰要素
        /// </summary>
        /// <param name="pAV"></param>
        /// <param name="_enumMapSurroundType"></param>
        /// <param name="pEnvelope"></param>
        private void AddMapSurround(IActiveView pAV, EnumMapSurroundType _enumMapSurroundType, IEnvelope pEnvelope)
        {
            try
            {
                switch (_enumMapSurroundType)
                {
                    case EnumMapSurroundType.NorthArrow:
                        addNorthArrow(axPageLayoutControl1.PageLayout, pEnvelope, pAV);
                        break;
                    case EnumMapSurroundType.ScaleBar:
                        makeScaleBar(pAV, axPageLayoutControl1.PageLayout, pEnvelope);
                        break;
                    case EnumMapSurroundType.Legend:
                        MakeLegend(pAV, axPageLayoutControl1.PageLayout, pEnvelope);
                        break;
                }
            }
            catch (Exception ex)
            {
            }
        }

        #region 添加图例
        /// <summary>
        /// 添加图例
        /// </summary>
        /// <param name="activeView"></活动窗口>
        /// <param name="pageLayout"></布局窗口>
        /// <param name="pEnv"></包络线>
        private void MakeLegend(IActiveView pActiveView, IPageLayout pPageLayout, IEnvelope pEnv)
        {
            UID pID = new UID();
            pID.Value = "esriCarto.Legend";
            IGraphicsContainer pGraphicsContainer = pPageLayout as IGraphicsContainer;
            IMapFrame pMapFrame = pGraphicsContainer.FindFrame(pActiveView.FocusMap) as IMapFrame;
            IMapSurroundFrame pMapSurroundFrame = pMapFrame.CreateSurroundFrame(pID, null);//根据唯一标示符，创建与之对应MapSurroundFrame
            IElement pDeletElement = axPageLayoutControl1.FindElementByName("Legend");//获取PageLayout中的图例元素
            if (pDeletElement != null)
            {
                pGraphicsContainer.DeleteElement(pDeletElement);  //如果已经存在图例，删除已经存在的图例
            }
            //设置MapSurroundFrame背景
            ISymbolBackground pSymbolBackground = new SymbolBackgroundClass();
            IFillSymbol pFillSymbol = new SimpleFillSymbolClass();
            ILineSymbol pLineSymbol = new SimpleLineSymbolClass();
            pLineSymbol.Color = m_OperatePageLayout.GetRgbColor(0, 0, 0);
            pFillSymbol.Color = m_OperatePageLayout.GetRgbColor(240, 240, 240);
            pFillSymbol.Outline = pLineSymbol;
            pSymbolBackground.FillSymbol = pFillSymbol;
            pMapSurroundFrame.Background = pSymbolBackground;
            //添加图例
            IElement pElement = pMapSurroundFrame as IElement;
            pElement.Geometry = pEnv as IGeometry;
            IMapSurround pMapSurround = pMapSurroundFrame.MapSurround;
            ILegend pLegend = pMapSurround as ILegend;
            pLegend.ClearItems();
            pLegend.Title = "图例";
            for (int i = 0; i < pActiveView.FocusMap.LayerCount; i++)
            {
                ILegendItem pLegendItem = new HorizontalLegendItemClass();
                pLegendItem.Layer = pActiveView.FocusMap.get_Layer(i);//获取添加图例关联图层             
                pLegendItem.ShowDescriptions = false;
                pLegendItem.Columns = 1;
                pLegendItem.ShowHeading = true;
                pLegendItem.ShowLabels = true;
                pLegend.AddItem(pLegendItem);//添加图例内容
            }
            pGraphicsContainer.AddElement(pElement, 0);
            pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
        }
        #endregion
        #region 指北针
        /// <summary>
        /// 插入指北针
        /// </summary>
        /// <param name="pPageLayout"></param>
        /// <param name="pEnv"></param>
        /// <param name="pActiveView"></param>
        void addNorthArrow(IPageLayout pPageLayout, IEnvelope pEnv, IActiveView pActiveView)
        {
            IMap pMap = pActiveView.FocusMap;
            IGraphicsContainer pGraphicsContainer = pPageLayout as IGraphicsContainer;
            IMapFrame pMapFrame = pGraphicsContainer.FindFrame(pMap) as IMapFrame;
            if (pStyleGalleryItem == null) return;
            IMapSurroundFrame pMapSurroundFrame = new MapSurroundFrameClass();
            pMapSurroundFrame.MapFrame = pMapFrame;
            INorthArrow pNorthArrow = new MarkerNorthArrowClass();
            pNorthArrow = pStyleGalleryItem.Item as INorthArrow;
            pNorthArrow.Size = pEnv.Width * 50;
            pMapSurroundFrame.MapSurround = (IMapSurround)pNorthArrow;//根据用户的选取，获取相应的MapSurround            
            IElement pElement = axPageLayoutControl1.FindElementByName("NorthArrows");//获取PageLayout中的指北针元素
            if (pElement != null)
            {
                pGraphicsContainer.DeleteElement(pElement);  //如果存在指北针，删除已经存在的指北针
            }
            IElementProperties pElePro = null;
            pElement = (IElement)pMapSurroundFrame;
            pElement.Geometry = (IGeometry)pEnv;
            pElePro = pElement as IElementProperties;
            pElePro.Name = "NorthArrows";
            pGraphicsContainer.AddElement(pElement, 0);
            pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
        }
        #endregion
        #region  比例尺
        /// <summary>
        /// 比例尺
        /// </summary>
        /// <param name="pActiveView"></param>
        /// <param name="pPageLayout"></param>
        /// <param name="pEnv"></param>
        public void makeScaleBar(IActiveView pActiveView, IPageLayout pPageLayout, IEnvelope pEnv)
        {
            IMap pMap = pActiveView.FocusMap;
            IGraphicsContainer pGraphicsContainer = pPageLayout as IGraphicsContainer;
            IMapFrame pMapFrame = pGraphicsContainer.FindFrame(pMap) as IMapFrame;
            if (pStyleGalleryItem == null) return;
            IMapSurroundFrame pMapSurroundFrame = new MapSurroundFrameClass();
            pMapSurroundFrame.MapFrame = pMapFrame;
            pMapSurroundFrame.MapSurround = (IMapSurround)pStyleGalleryItem.Item;
            IElement pElement = axPageLayoutControl1.FindElementByName("ScaleBar");
            if (pElement != null)
            {
                pGraphicsContainer.DeleteElement(pElement);  //删除已经存在的比例尺
            }
            IElementProperties pElePro = null;
            pElement = (IElement)pMapSurroundFrame;
            pElement.Geometry = (IGeometry)pEnv;
            pElePro = pElement as IElementProperties;
            pElePro.Name = "ScaleBar";
            pGraphicsContainer.AddElement(pElement, 0);
            pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
        }
        #endregion
        private IRgbColor getRGB(int r, int g, int b)
        {
            IRgbColor pColor;
            pColor = new RgbColorClass();
            pColor.Red = r;
            pColor.Green = g;
            pColor.Blue = b;
            return pColor;
        }
        private IGeoFeatureLayer getGeoLayer(string layerName)
        {
            ILayer pLayer;
            IGeoFeatureLayer pGeoFeaterLayer;
            for (int i = 0; i < mMapControl.LayerCount; i++)
            {
                pLayer = mMapControl.get_Layer(i);
                if (pLayer != null && pLayer.Name == layerName)
                {
                    pGeoFeaterLayer = pLayer as IGeoFeatureLayer;
                    return pGeoFeaterLayer;

                }
            }
            return null;
        }


        #endregion


        #region 地图符号化渲染
        private IColorRamp CreateAlgorithmicColorRamp(int count)
        {
            IAlgorithmicColorRamp algColorRamp = new AlgorithmicColorRampClass();
            IRgbColor fromColor = new RgbColorClass();
            fromColor.Red = 255;
            fromColor.Green = 0;
            fromColor.Blue = 0;

            IRgbColor toColor = new RgbColorClass();
            toColor.Red = 0;
            toColor.Green = 0;
            toColor.Blue = 255;

            algColorRamp.ToColor = fromColor;
            algColorRamp.FromColor = toColor;


            algColorRamp.Algorithm = esriColorRampAlgorithm.esriCIELabAlgorithm;

            algColorRamp.Size = count;


            bool btrue = true;
            algColorRamp.CreateRamp(out btrue);
            return algColorRamp;


        }

        private void 单值专题图ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IGeoFeatureLayer geoFeatureLayer = getGeoLayer("Continents");
            IUniqueValueRenderer uniqueValueRenderer = new UniqueValueRendererClass();
            uniqueValueRenderer.FieldCount = 1;
            uniqueValueRenderer.set_Field(0, "continent");
            ISimpleFillSymbol simpleFillSymbol;

            IFeatureCursor featureCursor = geoFeatureLayer.FeatureClass.Search(null, false);
            IFeature feature;


            if (featureCursor != null)
            {
                IEnumColors enumColors = CreateAlgorithmicColorRamp(8).Colors;
                int fieldIndex = geoFeatureLayer.FeatureClass.Fields.FindField("continent");

                for (int i = 0; i < 8; i++)
                {
                    feature = featureCursor.NextFeature();
                    string nameValue = feature.get_Value(fieldIndex).ToString();
                    simpleFillSymbol = new SimpleFillSymbolClass();
                    simpleFillSymbol.Style = esriSimpleFillStyle.esriSFSSolid;
                    simpleFillSymbol.Color = enumColors.Next();
                    uniqueValueRenderer.AddValue(nameValue, "continent", simpleFillSymbol as ISymbol);
                }


            }

            geoFeatureLayer.Renderer = uniqueValueRenderer as IFeatureRenderer;
            mMapControl.ActiveView.Refresh();
        }

        private void 柱状专题图ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IGeoFeatureLayer geoFeatureLayer;
            IFeatureLayer featureLayer;
            ITable table;
            ICursor cursor;
            IRowBuffer rowBuffer;

            string filed1 = "sqmi";
            string filed2 = "sqkm";


            geoFeatureLayer = getGeoLayer("Continents");
            featureLayer = geoFeatureLayer as IFeatureLayer;
            table = featureLayer as ITable;
            geoFeatureLayer.ScaleSymbols = true;
            IChartRenderer chartRenderer = new ChartRendererClass();
            IRendererFields rendererFileds = chartRenderer as IRendererFields;
            rendererFileds.AddField(filed1, filed1);
            rendererFileds.AddField(filed2, filed2);
            int[] fieldIndexs = new int[2];
            fieldIndexs[0] = table.FindField(filed1);
            fieldIndexs[1] = table.FindField(filed2);


            double fieldValue = 0.0, maxValue = 0.0;
            cursor = table.Search(null, false);
            rowBuffer = cursor.NextRow();

            while (rowBuffer != null)
            {
                for (int i = 0; i < 2; i++)
                {
                    fieldValue = double.Parse(rowBuffer.get_Value(fieldIndexs[i]).ToString());
                    if (fieldValue > maxValue)
                    {

                        maxValue = fieldValue;
                    }

                }
                rowBuffer = cursor.NextRow();
            }

            IBarChartSymbol barChartSymbol = new BarChartSymbolClass();
            barChartSymbol.Width = 10;
            IMarkerSymbol markerSymbol = barChartSymbol as IMarkerSymbol;
            markerSymbol.Size = 50;
            IChartSymbol chartSymbol = barChartSymbol as IChartSymbol;
            chartSymbol.MaxValue = maxValue;
            ISymbolArray symbolArray = barChartSymbol as ISymbolArray;
            IFillSymbol fillSymbol = (IFillSymbol)new SimpleFillSymbolClass();
            fillSymbol.Color = getRGB(255, 0, 0);
            symbolArray.AddSymbol(fillSymbol as ISymbol);
            fillSymbol = new SimpleFillSymbolClass();
            fillSymbol.Color = getRGB(0, 255, 0);
            symbolArray.AddSymbol(fillSymbol as ISymbol);
            chartRenderer.ChartSymbol = barChartSymbol as IChartSymbol;
            fillSymbol = new SimpleFillSymbolClass();
            fillSymbol.Color = getRGB(0, 0, 255);
            chartRenderer.BaseSymbol = fillSymbol as ISymbol;
            chartRenderer.UseOverposter = false;
            chartRenderer.CreateLegend();
            geoFeatureLayer.Renderer = chartRenderer as IFeatureRenderer;
            mMapControl.ActiveView.Refresh();
        }

        private void 饼状专题图ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IGeoFeatureLayer geoFeatureLayer;
            IFeatureLayer featureLayer;
            ITable table;
            ICursor cursor;
            IRowBuffer rowBuffer;

            string filed1 = "sqmi";
            string filed2 = "sqkm";


            geoFeatureLayer = getGeoLayer("Continents");
            featureLayer = geoFeatureLayer as IFeatureLayer;
            table = featureLayer as ITable;
            geoFeatureLayer.ScaleSymbols = true;
            IChartRenderer chartRenderer = new ChartRendererClass();
            IRendererFields rendererFileds = chartRenderer as IRendererFields;
            rendererFileds.AddField(filed1, filed1);
            rendererFileds.AddField(filed2, filed2);

            int[] fieldIndexs = new int[2];
            fieldIndexs[0] = table.FindField(filed1);
            fieldIndexs[1] = table.FindField(filed2);


            double fieldValue = 0.0, maxValue = 0.0;
            cursor = table.Search(null, false);
            rowBuffer = cursor.NextRow();

            while (rowBuffer != null)
            {
                for (int i = 0; i < 2; i++)
                {
                    fieldValue = double.Parse(rowBuffer.get_Value(fieldIndexs[i]).ToString());
                    if (fieldValue > maxValue)
                    {

                        maxValue = fieldValue;
                    }

                }
                rowBuffer = cursor.NextRow();
            }

            IPieChartSymbol pPieChartSymbol = new PieChartSymbolClass();
            pPieChartSymbol.Clockwise = true;
            pPieChartSymbol.UseOutline = true;

            IMarkerSymbol markerSymbol = pPieChartSymbol as IMarkerSymbol;
            markerSymbol.Size = 55;
            IChartSymbol chartSymbol = pPieChartSymbol as IChartSymbol;
            chartSymbol.MaxValue = maxValue;
            ISymbolArray symbolArray = pPieChartSymbol as ISymbolArray;
            IFillSymbol fillSymbol = (IFillSymbol)new SimpleFillSymbolClass();
            fillSymbol.Color = getRGB(255, 0, 0);
            symbolArray.AddSymbol(fillSymbol as ISymbol);
            fillSymbol = new SimpleFillSymbolClass();
            fillSymbol.Color = getRGB(0, 255, 0);
            symbolArray.AddSymbol(fillSymbol as ISymbol);


            chartRenderer.ChartSymbol = pPieChartSymbol as IChartSymbol;
            fillSymbol = new SimpleFillSymbolClass();
            fillSymbol.Color = getRGB(0, 0, 255);
            chartRenderer.BaseSymbol = fillSymbol as ISymbol;
            chartRenderer.UseOverposter = false;

            chartRenderer.CreateLegend();
            geoFeatureLayer.Renderer = chartRenderer as IFeatureRenderer;
            mMapControl.ActiveView.Refresh();
        }

        private void 分级专题图ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IGeoFeatureLayer geoFeatureLayer;
            IFeatureLayer featureLayer;

            geoFeatureLayer = getGeoLayer("Continents");
            featureLayer = geoFeatureLayer as IFeatureLayer;

            IGeoFeatureLayer pGeoFeatureLayer = geoFeatureLayer as IGeoFeatureLayer;

            geoFeatureLayer.ScaleSymbols = true;

            IFeatureClass pFeatureClass = featureLayer.FeatureClass;
            IFeatureCursor pFeatureCursor = pFeatureClass.Search(null, false);
            IFeature pFeature = pFeatureCursor.NextFeature();

            IClassBreaksUIProperties pUIProperties;
            object dataValues;
            object dataFrequency;
            //double[] cb;


            int breakIndex;
            long ClassesCount;
            int numClass;
            numClass = 10;
            double[] Classes;
            //////////////////////////////////////////////////////////////////////
            /* We're going to retrieve frequency data from a population 
              field and then classify this data*/

            ITable pTable;
            pTable = pFeatureClass as ITable;
            IBasicHistogram pBasicHist = new BasicTableHistogramClass();
            ITableHistogram pTableHist;

            pTableHist = (ITableHistogram)pBasicHist;

            //Get values and frequencies for the population field into a table histogram object
            pTableHist.Field = "sqkm";
            pTableHist.Table = pTable;
            pBasicHist.GetHistogram(out dataValues, out dataFrequency);

            IClassifyGEN pClassifyGEN = new QuantileClass();
            pClassifyGEN.Classify(dataValues, dataFrequency, ref numClass);
            Classes = (double[])pClassifyGEN.ClassBreaks;
            ClassesCount = long.Parse(Classes.GetUpperBound(0).ToString());

            //Initialise a new class breaks renderer and supply the number of class breaks and the field to perform the class breaks on.
            IClassBreaksRenderer pClassBreaksRenderer = new ClassBreaksRendererClass();
            pClassBreaksRenderer.Field = "sqkm";
            //pClassBreaksRenderer.BreakCount = ClassesCount;
            pClassBreaksRenderer.MinimumBreak = Classes[0];
            pClassBreaksRenderer.SortClassesAscending = true;
            //设置着色对象的分级数目
            pClassBreaksRenderer.BreakCount = int.Parse(ClassesCount.ToString());

            //创建并设置随机色谱
            IAlgorithmicColorRamp pColorRamp = new AlgorithmicColorRampClass();
            pColorRamp.Algorithm = esriColorRampAlgorithm.esriCIELabAlgorithm;
            IEnumColors pEnumColors;
            IRgbColor pColor1 = new RgbColorClass();
            IRgbColor pColor2 = new RgbColorClass();
            pColor1.Red = 255;
            pColor1.Green = 210;
            pColor1.Blue = 210;
            pColor2.Red = 190;
            pColor2.Green = 0;
            pColor2.Blue = 170;
            pColorRamp.FromColor = pColor1;
            pColorRamp.ToColor = pColor2;
            pColorRamp.Size = numClass;
            bool ok = true;
            pColorRamp.CreateRamp(out ok);
            pEnumColors = pColorRamp.Colors;
            pEnumColors.Reset();// use this interface to set dialog properties 

            pUIProperties = pClassBreaksRenderer as IClassBreaksUIProperties;
            pUIProperties.ColorRamp = "Custom";

            ISimpleFillSymbol pSimpleMarkerSymbol = new SimpleFillSymbolClass();

            IColor pColor;
            int[] colors = new int[numClass];

            // be careful, indices are different for the diff lists    
            for (breakIndex = 0; breakIndex < ClassesCount; breakIndex++)
            {

                pClassBreaksRenderer.set_Label(breakIndex, Classes[breakIndex] + " - " + Classes[breakIndex + 1]);
                pUIProperties.set_LowBreak(breakIndex, Classes[breakIndex]);
                pSimpleMarkerSymbol = new SimpleFillSymbolClass();
                pColor = pEnumColors.Next();
                pSimpleMarkerSymbol.Color = pColor;
                colors[breakIndex] = pColor.RGB;

                pClassBreaksRenderer.set_Symbol(breakIndex, (ISymbol)pSimpleMarkerSymbol);
                pClassBreaksRenderer.set_Break(breakIndex, Classes[breakIndex + 1]);
            }

            //将等级图渲染对象与渲染图层挂钩
            pGeoFeatureLayer.Renderer = (IFeatureRenderer)pClassBreaksRenderer;
            //刷新地图和TOOCotrol
            IActiveView pActiveView = mMapControl.Map as IActiveView;
            pActiveView.Refresh();
        }

        #endregion

        #region 删除所有数据菜单项

        private void 删除所有数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //删除地图中所有的图层
                for (int i = mMapControl.LayerCount - 1; i >= 0; i--)
                {
                    mMapControl.DeleteLayer(i);

                    EagleEyeMapControl2.DeleteLayer(i);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("删除图层失败！！！" + e.ToString());
            }


        }


        #endregion

        #region 打印、导出、保存、另存
        private void 打印ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmPrintPreview = new frmPrintPreview(axPageLayoutControl1);
            frmPrintPreview.ShowDialog();
        }

        private void 导出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (frmExpMap == null || frmExpMap.IsDisposed)
            {
                frmExpMap = new FormExportMap(mMapControl);
            }
            frmExpMap.IsRegion = false;
            frmExpMap.GetGeometry = mMapControl.ActiveView.Extent;
            frmExpMap.Show();
            frmExpMap.Activate();
        }

        private void 保存ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                string sMxdFileName = mMapControl.DocumentFilename;
                IMapDocument pMapDocument = new MapDocumentClass();
                if (sMxdFileName != null && mMapControl.CheckMxFile(sMxdFileName))
                {
                    if (pMapDocument.get_IsReadOnly(sMxdFileName))
                    {
                        MessageBox.Show("本地图文档是只读的，不能保存!");
                        pMapDocument.Close();
                        return;
                    }
                }
                else
                {
                    SaveFileDialog pSaveFileDialog = new SaveFileDialog();
                    pSaveFileDialog.Title = "请选择保存路径";
                    pSaveFileDialog.OverwritePrompt = true;
                    pSaveFileDialog.Filter = "ArcMap文档（*.mxd）|*.mxd|ArcMap模板（*.mxt）|*.mxt";
                    pSaveFileDialog.RestoreDirectory = true;
                    if (pSaveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        sMxdFileName = pSaveFileDialog.FileName;
                    }
                    else
                    {
                        return;
                    }
                }

                pMapDocument.New(sMxdFileName);
                pMapDocument.ReplaceContents(mMapControl.Map as IMxdContents);
                pMapDocument.Save(pMapDocument.UsesRelativePaths, true);
                pMapDocument.Close();
                MessageBox.Show("保存地图文档成功!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void 另存ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog pSaveFileDialog = new SaveFileDialog();
                pSaveFileDialog.Title = "另存为";
                pSaveFileDialog.OverwritePrompt = true;
                pSaveFileDialog.Filter = "ArcMap文档（*.mxd）|*.mxd|ArcMap模板（*.mxt）|*.mxt";
                pSaveFileDialog.RestoreDirectory = true;
                if (pSaveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string sFilePath = pSaveFileDialog.FileName;

                    IMapDocument pMapDocument = new MapDocumentClass();
                    pMapDocument.New(sFilePath);
                    pMapDocument.ReplaceContents(mMapControl.Map as IMxdContents);
                    pMapDocument.Save(true, true);
                    pMapDocument.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #endregion

        #region Mapcontrol点击、移动函数

        private void mMapControl_OnMouseMove(object sender, IMapControlEvents2_OnMouseMoveEvent e)
        {

            sMapUnits = GetMapUnit(mMapControl.Map.MapUnits);
            toolStripStatusLabel1.Text = String.Format("当前坐标：X = {0:#.###} Y = {1:#.###} {2}", e.mapX, e.mapY, sMapUnits);
            pMovePt = (mMapControl.Map as IActiveView).ScreenDisplay.DisplayTransformation.ToMapPoint(e.x, e.y);

            #region 长度量算
            if (pMouseOperate == "MeasureLength")
            {
                if (pNewLineFeedback != null)
                {
                    pNewLineFeedback.MoveTo(pMovePt);
                }
                double deltaX = 0; //两点之间X差值
                double deltaY = 0; //两点之间Y差值

                if ((pPoint != null) && (pNewLineFeedback != null))
                {
                    deltaX = pMovePt.X - pPoint.X;
                    deltaY = pMovePt.Y - pPoint.Y;
                    dSegmentLength = Math.Round(Math.Sqrt((deltaX * deltaX) + (deltaY * deltaY)), 3);
                    dToltalLength = dToltalLength + dSegmentLength;
                    if (frmMeasureResult != null)
                    {
                        frmMeasureResult.lblMeasureResult.Text = String.Format(
                            "当前线段长度：{0:.###}{1};\r\n总长度为: {2:.###}{1}",
                            dSegmentLength, sMapUnits, dToltalLength);
                        dToltalLength = dToltalLength - dSegmentLength; //鼠标移动到新点重新开始计算
                    }
                    frmMeasureResult.frmClosed += new FormMeasureResult.FormClosedEventHandler(frmMeasureResult_frmColsed);
                }
            }
            #endregion

            #region 面积量算
            if (pMouseOperate == "MeasureArea")
            {
                if (pNewPolygonFeedback != null)
                {
                    pNewPolygonFeedback.MoveTo(pMovePt);
                }

                IPointCollection pPointCol = new Polygon();
                IPolygon pPolygon = new PolygonClass();
                IGeometry pGeo = null;

                ITopologicalOperator pTopo = null;
                for (int i = 0; i <= pAreaPointCol.PointCount - 1; i++)
                {
                    pPointCol.AddPoint(pAreaPointCol.get_Point(i), ref missing, ref missing);
                }
                pPointCol.AddPoint(pMovePt, ref missing, ref missing);

                if (pPointCol.PointCount < 3) return;
                pPolygon = pPointCol as IPolygon;

                if ((pPolygon != null))
                {
                    pPolygon.Close();
                    pGeo = pPolygon as IGeometry;
                    pTopo = pGeo as ITopologicalOperator;
                    //使几何图形的拓扑正确
                    pTopo.Simplify();
                    pGeo.Project(mMapControl.Map.SpatialReference);
                    IArea pArea = pGeo as IArea;

                    frmMeasureResult.lblMeasureResult.Text = String.Format(
                        "总面积为：{0:.####}平方{1};\r\n总长度为：{2:.####}{1}",
                        pArea.Area, sMapUnits, pPolygon.Length);
                    pPolygon = null;
                }
            }
            #endregion

        }

        private void mMapControl_OnMouseDown(object sender, IMapControlEvents2_OnMouseDownEvent e)
        {
            pPointPt = (mMapControl.Map as IActiveView).ScreenDisplay.DisplayTransformation.ToMapPoint(e.x, e.y);
            IActiveView pActiveView = mMapControl.ActiveView.FocusMap as IActiveView;
            IEnvelope objEnvelope = null;
            pPoint = pActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(e.x, e.y);

            if (e.button == 1)
            {

                IEnvelope pEnvelope = new EnvelopeClass();

                switch (pMouseOperate)
                {
                    case "ZoomIn":
                        pEnvelope = mMapControl.TrackRectangle();
                        //如果拉框范围为空则返回
                        if (pEnvelope == null || pEnvelope.IsEmpty || pEnvelope.Height == 0 || pEnvelope.Width == 0)
                        {
                            return;
                        }
                        //如果有拉框范围，则放大到拉框范围
                        pActiveView.Extent = pEnvelope;
                        pActiveView.Refresh();
                        break;
                    case "ZoomOut":
                        objEnvelope = mMapControl.TrackRectangle();
                        double mapWidth = objEnvelope.Width;
                        double mapHeight = objEnvelope.Height;
                        double x1 = pPoint.X;
                        double x2 = pPoint.X + mapWidth;
                        double y1 = pPoint.Y;
                        double y2 = pPoint.Y - mapHeight * 2;
                        objEnvelope.XMax = x2 + mapWidth * 2;
                        objEnvelope.XMin = x1 - mapWidth * 2;
                        objEnvelope.YMax = y2 - mapHeight * 2;
                        objEnvelope.YMin = y1 + mapHeight * 2;
                        mMapControl.Extent = objEnvelope;
                        break;
                    case "Pan":
                        mMapControl.Pan();
                        break;


                    case "MeasureLength":
                        //判断追踪线对象是否为空，若是则实例化并设置当前鼠标点为起始点
                        if (pNewLineFeedback == null)
                        {
                            //实例化追踪线对象
                            pNewLineFeedback = new NewLineFeedbackClass();
                            pNewLineFeedback.Display = (mMapControl.Map as IActiveView).ScreenDisplay;
                            //设置起点，开始动态线绘制
                            pNewLineFeedback.Start(pPoint);
                            dToltalLength = 0;
                        }
                        else //如果追踪线对象不为空，则添加当前鼠标点
                        {
                            pNewLineFeedback.AddPoint(pPoint);
                        }
                        //pGeometry = m_PointPt;
                        if (dSegmentLength != 0)
                        {
                            dToltalLength = dToltalLength + dSegmentLength;
                        }
                        break;
                    case "MeasureArea":
                        if (pNewPolygonFeedback == null)
                        {
                            //实例化追踪面对象
                            pNewPolygonFeedback = new NewPolygonFeedback();
                            pNewPolygonFeedback.Display = (mMapControl.Map as IActiveView).ScreenDisplay;
                            ;
                            pAreaPointCol.RemovePoints(0, pAreaPointCol.PointCount);
                            //开始绘制多边形
                            pNewPolygonFeedback.Start(pPointPt);
                            pAreaPointCol.AddPoint(pPointPt, ref missing, ref missing);
                        }
                        else
                        {
                            pNewPolygonFeedback.AddPoint(pPointPt);
                            pAreaPointCol.AddPoint(pPointPt, ref missing, ref missing);
                        }
                        break;
                    case "pointquery":
                        IPoint point = new PointClass();
                        point.X = e.mapX;
                        point.Y = e.mapY;
                        selectGeo = point as IGeometry;
                        if (null != selectGeo)
                        {
                            mMapControl.Map.SelectByShape(selectGeo, null, false);
                            mMapControl.Refresh(esriViewDrawPhase.esriViewGeoSelection, null, null);
                        }

                        break;

                    case "mutlinquery":

                        break;


                }

            }
            if (e.button == 2)
            {

                buffer buf = new buffer(mMapControl);
                buf.Show();

            }




        }

        #endregion

        #region 开发者简介
        private void 开发者简介ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormIntroduction f = new FormIntroduction();
            f.Show();
        }
        #endregion

        #region 数据视图与布局视图切换
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.tabControl1.SelectedIndex == 0)
            {
                axToolbarControl1.SetBuddyControl(mMapControl);
                axTOCControl1.SetBuddyControl(mMapControl);
            }
            if (this.tabControl1.SelectedIndex == 1)
            {
                axToolbarControl1.SetBuddyControl(axPageLayoutControl1);
                axTOCControl1.SetBuddyControl(axPageLayoutControl1);

                IActiveView pPageLayoutView = (IActiveView)axPageLayoutControl1.ActiveView.FocusMap;
                IDisplayTransformation pDisplayTransformation = pPageLayoutView.ScreenDisplay.DisplayTransformation;
                pDisplayTransformation.VisibleBounds = mMapControl.Extent;
                axPageLayoutControl1.ActiveView.Refresh();
            }
        }
        #endregion


        #region 添加图名

        public void AddMapName()
        {

            Form2 a = new Form2();

            IGraphicsContainer pLegendGraphicsContainer = axPageLayoutControl1.GraphicsContainer;


            IEnvelope envelope = new EnvelopeClass();
            envelope.PutCoords(-14, 29, 35, 22);

            ITextSymbol pTextSymbol = new TextSymbolClass();

            pTextSymbol.Size = 30;


            ITextElement pTextElement = new TextElementClass();

            pTextElement.Text = a.show();
            pTextElement.Symbol = pTextSymbol;
            IElement element = pTextElement as ESRI.ArcGIS.Carto.IElement;
            element.Geometry = envelope;
            pLegendGraphicsContainer.AddElement(element, 0);
            axPageLayoutControl1.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);

        }
        private void 图名ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 f2 = new Form2();

            AddMapName();
        }
        #endregion

        private void 加载图层ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ControlsAddDataCommand adddata = new ControlsAddDataCommandClass();
            adddata.OnCreate(mMapControl.Object);
            adddata.OnClick();
        }
     //双击打开符号选择器
        private void axTOCControl1_OnDoubleClick(object sender, ITOCControlEvents_OnDoubleClickEvent e)
        {
            esriTOCControlItem itemType = esriTOCControlItem.esriTOCControlItemNone;
            IBasicMap basicMap = null;
            ILayer layer = null;
            object unk = null;
            object data = null;
            axTOCControl1.HitTest(e.x, e.y, ref itemType, ref basicMap, ref layer, ref unk, ref data);
            if (e.button == 1)
            {
                if (itemType == esriTOCControlItem.esriTOCControlItemLegendClass)
                {
                    //取得图例
                    ILegendClass pLegendClass = ((ILegendGroup)unk).get_Class((int)data);
                    //创建符号选择器SymbolSelector实例
                    frmSymbolSelector SymbolSelectorFrm = new frmSymbolSelector(pLegendClass, layer);
                    if (SymbolSelectorFrm.ShowDialog() == DialogResult.OK)
                    {
                        //局部更新主Map控件
                        mMapControl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography, null, null);
                        //设置新的符号
                        pLegendClass.Symbol = SymbolSelectorFrm.pSymbol;
                        //更新主Map控件和图层控件
                        this.mMapControl.ActiveView.Refresh();
                        this.axTOCControl1.Refresh();
                    }
                }
            }
        }
        //鹰眼关闭按钮
        private void button1_Click(object sender, EventArgs e)
        {
           
        }

      
       





    }
}

