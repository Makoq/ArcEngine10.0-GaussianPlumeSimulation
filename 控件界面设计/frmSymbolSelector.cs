using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using 符号化.Class;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
namespace 地信开发大作业.控件界面设计
{
    public partial class frmSymbolSelector : Form
    {
        

        private IStyleGalleryItem pStyleGalleryItem;
        private ILegendClass pLegendClass;
        private ILayer pLayer;
        public ISymbol pSymbol;
        public Image pSymbolImage;
        string filepath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
        OperateMap m_OperateMap = new OperateMap();
        bool contextMenuMoreSymbolInitiated = false;

        #region 构造函数，初始化全局变量
        /// <summary>
        /// 构造函数，初始化全局变量
        /// </summary>
        /// <param name="tempLegendClass"></param>
        /// <param name="tempLayer"></param>
        public frmSymbolSelector(ILegendClass tempLegendClass, ILayer tempLayer)
        {
            InitializeComponent();
            this.pLegendClass = tempLegendClass;
            this.pLayer = tempLayer;
        }
        #endregion

        #region 初始化SymbologyControl的StyleClass,图层如果已有符号,则把符号添加到SymbologyControl中的第一个符号,并选中
        /// <summary>
        /// 初始化SymbologyControl的StyleClass,图层如果已有符号,则把符号添加到SymbologyControl中的第一个符号,并选中
        /// </summary>
        /// <param name="symbologyStyleClass"></param>
        private void SetFeatureClassStyle(esriSymbologyStyleClass symbologyStyleClass)
        {
            this.SymbologyCtr.StyleClass = symbologyStyleClass;
            ISymbologyStyleClass pSymbologyStyleClass = this.SymbologyCtr.GetStyleClass(symbologyStyleClass);
            if (this.pLegendClass != null)
            {
                IStyleGalleryItem currentStyleGalleryItem = new ServerStyleGalleryItem();
                currentStyleGalleryItem.Name = "当前符号";
                currentStyleGalleryItem.Item = pLegendClass.Symbol;
                pSymbologyStyleClass.AddItem(currentStyleGalleryItem, 0);
                this.pStyleGalleryItem = currentStyleGalleryItem;
            }
            pSymbologyStyleClass.SelectItem(0);
        }
        #endregion

        #region 初始化
            private void frmSymbolSelector_Load(object sender, EventArgs e)
        {
            //string path = Application.StartupPath + "\\ESRI.ServerStyle";

            string path = m_OperateMap.getPath(filepath) + "\\data\\Symbol\\ESRI.ServerStyle";

            this.SymbologyCtr.LoadStyleFile(path);  // 根据ESRI.ServerStyle的路径，将其载入SymbologyControl控件中        
            //确定图层的类型(点线面),设置好SymbologyControl的StyleClass,设置好各控件的可见性(visible) 
            IGeoFeatureLayer pGeoFeatureLayer = (IGeoFeatureLayer)pLayer;
            switch (((IFeatureLayer)pLayer).FeatureClass.ShapeType)
            {
                case ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPoint:
                    this.SetFeatureClassStyle(esriSymbologyStyleClass.esriStyleClassMarkerSymbols);
                    this.lblAngle.Visible = true;
                    this.nudAngle.Visible = true;
                    this.lblSize.Visible = true;
                    this.nudSize.Visible = true;
                    this.lblWidth.Visible = false;
                    this.nudWidth.Visible = false;
                    this.lblOutlineColor.Visible = false;
                    this.btnOutlineColor.Visible = false;
                    break;
                case ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPolyline:
                    this.SetFeatureClassStyle(esriSymbologyStyleClass.esriStyleClassLineSymbols);
                    this.lblAngle.Visible = false;
                    this.nudAngle.Visible = false;
                    this.lblSize.Visible = false;
                    this.nudSize.Visible = false;
                    this.lblWidth.Visible = true;
                    this.nudWidth.Visible = true;
                    this.lblOutlineColor.Visible = false;
                    this.btnOutlineColor.Visible = false;
                    break;
                case ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPolygon:
                    this.SetFeatureClassStyle(esriSymbologyStyleClass.esriStyleClassFillSymbols);
                    this.lblAngle.Visible = false;
                    this.nudAngle.Visible = false;
                    this.lblSize.Visible = false;
                    this.nudSize.Visible = false;
                    this.lblWidth.Visible = true;
                    this.nudWidth.Visible = true;
                    this.lblOutlineColor.Visible = true;
                    this.btnOutlineColor.Visible = true;
                    break;
                case ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryMultiPatch:
                    this.SetFeatureClassStyle(esriSymbologyStyleClass.esriStyleClassFillSymbols);
                    this.lblAngle.Visible = false;
                    this.nudAngle.Visible = false;
                    this.lblSize.Visible = false;
                    this.nudSize.Visible = false;
                    this.lblWidth.Visible = true;
                    this.nudWidth.Visible = true;
                    this.lblOutlineColor.Visible = true;
                    this.btnOutlineColor.Visible = true;
                    break;
                default:
                    this.Close();
                    break;
            }
        }
        #endregion
        #region 确定按钮
        /// <summary> 
        /// 确定按钮
        /// </summary> 
        /// <param name="sender"></param> 
        /// <param name="e"></param> 
        private void btnOK_Click(object sender, EventArgs e)
        {
            //取得选定的符号
            this.pSymbol = (ISymbol)pStyleGalleryItem.Item;
            //更新预览图像
            this.pSymbolImage = this.ptbPreview.Image;
            //关闭窗体
            this.Close();
        }
        #endregion
        #region 取消
        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancle_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion
        #region   双击符号同单击确定按钮，关闭符号选择器
        /// <summary>
        /// 双击符号同单击确定按钮，关闭符号选择器
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SymbologyCtr_OnDoubleClick(object sender, ISymbologyControlEvents_OnDoubleClickEvent e)
        {
            this.btnOK.PerformClick();
        }
        #endregion

        #region 把选中并设置好的符号在picturebox控件中预览
        /// <summary>
        /// 把选中并设置好的符号在picturebox控件中预览
        /// </summary>
        private void PreviewImage()
        {
            stdole.IPictureDisp picture = this.SymbologyCtr.GetStyleClass(this.SymbologyCtr.StyleClass).PreviewItem(pStyleGalleryItem, this.ptbPreview.Width, this.ptbPreview.Height);
            System.Drawing.Image image = System.Drawing.Image.FromHbitmap(new System.IntPtr(picture.Handle));
            this.ptbPreview.Image = image;
        }
        #endregion


        #region 当样式（Style）改变时，重新设置符号类型和控件的可视性
        /// <summary>
        /// 当样式（Style）改变时，重新设置符号类型和控件的可视性
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SymbologyCtr_OnStyleClassChanged(object sender, ISymbologyControlEvents_OnStyleClassChangedEvent e)
        {
            switch (((ISymbologyStyleClass)e.symbologyStyleClass).StyleClass)
            {
                case esriSymbologyStyleClass.esriStyleClassMarkerSymbols:
                    this.lblAngle.Visible = true;
                    this.nudAngle.Visible = true;
                    this.lblSize.Visible = true;
                    this.nudSize.Visible = true;
                    this.lblWidth.Visible = false;
                    this.nudWidth.Visible = false;
                    this.lblOutlineColor.Visible = false;
                    this.btnOutlineColor.Visible = false;
                    break;
                case esriSymbologyStyleClass.esriStyleClassLineSymbols:
                    this.lblAngle.Visible = false;
                    this.nudAngle.Visible = false;
                    this.lblSize.Visible = false;
                    this.nudSize.Visible = false;
                    this.lblWidth.Visible = true;
                    this.nudWidth.Visible = true;
                    this.lblOutlineColor.Visible = false;
                    this.btnOutlineColor.Visible = false;
                    break;
                case esriSymbologyStyleClass.esriStyleClassFillSymbols:
                    this.lblAngle.Visible = false;
                    this.nudAngle.Visible = false;
                    this.lblSize.Visible = false;
                    this.nudSize.Visible = false;
                    this.lblWidth.Visible = true;
                    this.nudWidth.Visible = true;
                    this.lblOutlineColor.Visible = true;
                    this.btnOutlineColor.Visible = true;
                    break;
            }
        }
        #endregion

        #region 将ArcGIS Engine中的IRgbColor接口转换至.NET中的Color结构
        /// <summary>
        /// 将ArcGIS Engine中的IRgbColor接口转换至.NET中的Color结构
        /// </summary>
        /// <param name="pRgbColor"></param>
        /// <returns></returns>
        public Color ConvertIRgbColorToColor(IRgbColor pRgbColor)
        {

            return ColorTranslator.FromOle(pRgbColor.RGB);

        }
        #endregion

        #region 选中符号时触发的事件
        /// <summary>
        /// 选中符号时触发的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SymbologyCtr_OnItemSelected(object sender, ISymbologyControlEvents_OnItemSelectedEvent e)
        {
            pStyleGalleryItem = (IStyleGalleryItem)e.styleGalleryItem;
            Color color;
            switch (this.SymbologyCtr.StyleClass)
            {
                //点符号
                    
                case esriSymbologyStyleClass.esriStyleClassMarkerSymbols:
                    color = this.ConvertIRgbColorToColor(((IMarkerSymbol)pStyleGalleryItem.Item).Color as IRgbColor);
                    //设置按钮背景色
                    this.btnColor.BackColor = color;
                    //设置点符号角度和大小初始值
                    this.nudAngle.Value = (decimal)((IMarkerSymbol)this.pStyleGalleryItem.Item).Angle;
                    this.nudSize.Value = (decimal)((IMarkerSymbol)this.pStyleGalleryItem.Item).Size;
                    break;
                //线符号
                case esriSymbologyStyleClass.esriStyleClassLineSymbols:
                    color = this.ConvertIRgbColorToColor(((ILineSymbol)pStyleGalleryItem.Item).Color as IRgbColor);
                    //设置按钮背景色
                    this.btnColor.BackColor = color;
                    //设置线宽初始值
                    this.nudWidth.Value = (decimal)((ILineSymbol)this.pStyleGalleryItem.Item).Width;
                    break;
                //面符号
                case esriSymbologyStyleClass.esriStyleClassFillSymbols:
                    //如果面符号选中的颜色不为渐变色，则设置按钮背景颜色
                    if (((IFillSymbol)pStyleGalleryItem.Item).Color as IRgbColor != null)
                    {
                        color = this.ConvertIRgbColorToColor(((IFillSymbol)pStyleGalleryItem.Item).Color as IRgbColor);
                        //设置按钮背景色
                        this.btnColor.BackColor = color;
                    }
                    this.btnOutlineColor.BackColor =
                    this.ConvertIRgbColorToColor(((IFillSymbol)pStyleGalleryItem.Item).Outline.Color as IRgbColor);
                    //设置外框线宽度初始值
                    this.nudWidth.Value = (decimal)((IFillSymbol)this.pStyleGalleryItem.Item).Outline.Width;
                    break;
                default:
                    color = Color.Black;
                    break;
            }
            //预览符号
            this.PreviewImage();

        }
        #endregion


        #region 调整符号大小-点符号
        /// <summary>
        /// 调整符号大小-点符号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void nudSize_ValueChanged(object sender, EventArgs e)
        {
            ((IMarkerSymbol)this.pStyleGalleryItem.Item).Size = (double)this.nudSize.Value;
            this.PreviewImage();
        }
      #endregion


        #region 调整符号角度-点符号
        /// <summary>
        /// 调整符号角度-点符号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void nudAngle_ValueChanged(object sender, EventArgs e)
        {
            ((IMarkerSymbol)this.pStyleGalleryItem.Item).Angle = (double)this.nudAngle.Value;
            this.PreviewImage();
        }

        #endregion


        #region 调整符号宽度-限于线符号和面符号
        /// <summary>
        /// 调整符号宽度-限于线符号和面符号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void nudWidth_ValueChanged(object sender, EventArgs e)
        {
            switch (this.SymbologyCtr.StyleClass)
            {
                case esriSymbologyStyleClass.esriStyleClassLineSymbols:
                    ((ILineSymbol)this.pStyleGalleryItem.Item).Width = Convert.ToDouble(this.nudWidth.Value);
                    break;
                case esriSymbologyStyleClass.esriStyleClassFillSymbols:
                    //取得面符号的轮廓线符号
                    ILineSymbol pLineSymbol = ((IFillSymbol)this.pStyleGalleryItem.Item).Outline;
                    pLineSymbol.Width = Convert.ToDouble(this.nudWidth.Value);
                    ((IFillSymbol)this.pStyleGalleryItem.Item).Outline = pLineSymbol;
                    break;
            }
            this.PreviewImage();
        }

        #endregion

        #region 将.NET中的Color结构转换至于ArcGIS Engine中的IColor接口
        /// <summary>
        /// 将.NET中的Color结构转换至于ArcGIS Engine中的IColor接口
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public IColor ConvertColorToIColor(Color color)
        {
            IColor pColor = new RgbColorClass();
            pColor.RGB = color.B * 65536 + color.G * 256 + color.R;
            return pColor;
        }
        #endregion


        #region 颜色按钮
        /// <summary>
        /// 颜色按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnColor_Click(object sender, EventArgs e)
        {
            //调用系统颜色对话框
            if (this.colorDialog1.ShowDialog() == DialogResult.OK)
            {
                //将颜色按钮的背景颜色设置为用户选定的颜色
                this.btnColor.BackColor = this.colorDialog1.Color;
                //设置符号颜色为用户选定的颜色
                switch (this.SymbologyCtr.StyleClass)
                {
                    //点符号
                    case esriSymbologyStyleClass.esriStyleClassMarkerSymbols:
                        ((IMarkerSymbol)this.pStyleGalleryItem.Item).Color = this.ConvertColorToIColor(this.colorDialog1.Color);
                        break;
                    //线符号
                    case esriSymbologyStyleClass.esriStyleClassLineSymbols:
                        ((ILineSymbol)this.pStyleGalleryItem.Item).Color = this.ConvertColorToIColor(this.colorDialog1.Color);
                        break;
                    //面符号
                    case esriSymbologyStyleClass.esriStyleClassFillSymbols:
                        ((IFillSymbol)this.pStyleGalleryItem.Item).Color = this.ConvertColorToIColor(this.colorDialog1.Color);
                        break;
                }
                //更新符号预览
                this.PreviewImage();
            }
        }


        #endregion


        #region 外框颜色按钮
        /// <summary>
        /// 外框颜色按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOutlineColor_Click(object sender, EventArgs e)
        {
            if (this.colorDialog1.ShowDialog() == DialogResult.OK)
            {
                //取得面符号中的外框线符号
                ILineSymbol pLineSymbol = ((IFillSymbol)this.pStyleGalleryItem.Item).Outline;
                //设置外框线颜色
                pLineSymbol.Color = this.ConvertColorToIColor(this.colorDialog1.Color);
                //重新设置面符号中的外框线符号
                ((IFillSymbol)this.pStyleGalleryItem.Item).Outline = pLineSymbol;
                //设置按钮背景颜色
                this.btnOutlineColor.BackColor = this.colorDialog1.Color;
                //更新符号预览
                this.PreviewImage();
            }
        }
        #endregion


        #region “更多符号”按下时触发的事件
        /// <summary>
        /// “更多符号”按下时触发的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void btnMoreSymbols_Click(object sender, EventArgs e)
        {
            if (this.contextMenuMoreSymbolInitiated == false)
            {
                string path = m_OperateMap.getPath(filepath) + "\\data\\Symbol\\Styles";
                //取得菜单项数量
                string[] styleNames = System.IO.Directory.GetFiles(path, "*.ServerStyle");
                ToolStripMenuItem[] symbolContextMenuItem = new ToolStripMenuItem[styleNames.Length + 1];
                //循环添加其它符号菜单项到菜单
                for (int i = 0; i < styleNames.Length; i++)
                {
                    symbolContextMenuItem[i] = new ToolStripMenuItem();
                    symbolContextMenuItem[i].CheckOnClick = true;
                    symbolContextMenuItem[i].Text = System.IO.Path.GetFileNameWithoutExtension(styleNames[i]);
                    if (symbolContextMenuItem[i].Text == "ESRI")
                    {
                        symbolContextMenuItem[i].Checked = true;
                    }
                    symbolContextMenuItem[i].Name = styleNames[i];
                }
                //添加“更多符号”菜单项到菜单最后一项
                symbolContextMenuItem[styleNames.Length] = new ToolStripMenuItem();
                symbolContextMenuItem[styleNames.Length].Text = "添加符号";
                symbolContextMenuItem[styleNames.Length].Name = "AddMoreSymbol";
                //添加所有的菜单项到菜单
                this.contextMenuStripMoreSymbol.Items.AddRange(symbolContextMenuItem);
                this.contextMenuMoreSymbolInitiated = true;
            }
            //显示菜单
            this.contextMenuStripMoreSymbol.Show(this.btnMoreSymbols.Location);

        }

        #endregion




        #region 更多符号”按钮弹出的菜单项单击事件
        /// <summary>
        /// “更多符号”按钮弹出的菜单项单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void contextMenuStripMoreSymbol_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            ToolStripMenuItem pToolStripMenuItem = (ToolStripMenuItem)e.ClickedItem;
            //如果单击的是“添加符号”
            if (pToolStripMenuItem.Name == "AddMoreSymbol")
            {
                //弹出打开文件对话框
                if (this.openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    //导入style file到SymbologyControl 
                    this.SymbologyCtr.LoadStyleFile(this.openFileDialog1.FileName);
                    //刷新axSymbologyControl控件
                    this.SymbologyCtr.Refresh();
                }
            }
            else//如果是其它选项
            {
                if (pToolStripMenuItem.Checked == false)
                {
                    this.SymbologyCtr.LoadStyleFile(pToolStripMenuItem.Name);
                    this.SymbologyCtr.Refresh();
                }
                else
                {
                    this.SymbologyCtr.RemoveFile(pToolStripMenuItem.Name);
                    this.SymbologyCtr.Refresh();
                }
            }
        }

        #endregion

  

   





    }
}
