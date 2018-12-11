using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Carto;
using 地信开发大作业.Class;
using 地信开发大作业.Class.EnumType;

namespace 地信开发大作业
{
    public partial class frmSymbol : Form
    {

        public ISymbol pSelSymbol;
        private IStyleGalleryItem pStyleGalleryItem;
        private ISymbologyStyleClass pSymStyleClass;

        public delegate void GetSelSymbolItemEventHandler(ref IStyleGalleryItem pStyleItem);
        public event GetSelSymbolItemEventHandler GetSelSymbolItem = null;

        string filepath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase;


        private EnumMapSurroundType _enumMapSurType = EnumMapSurroundType.None;
              
        public frmSymbol()
        {
            InitializeComponent();
        }

        private void frmSymbol_Load(object sender, EventArgs e)
        {

        }

        public void InitUI()
        {
            SymbologyCtrl.Clear();
            string StyleFilePath = OperatePageLayout.getPath(filepath) + "\\data2\\ESRI.ServerStyle";//载入系统符号库
            SymbologyCtrl.LoadStyleFile(StyleFilePath);
            switch (_enumMapSurType)
            {
                case Class.EnumType.EnumMapSurroundType.NorthArrow://根据选择，载入系统指北针符号库
                    SymbologyCtrl.StyleClass = esriSymbologyStyleClass.esriStyleClassNorthArrows;
                    pSymStyleClass = SymbologyCtrl.GetStyleClass(esriSymbologyStyleClass.esriStyleClassNorthArrows);
                    break;
                case Class.EnumType.EnumMapSurroundType.ScaleBar://根据选择，载入系统比例尺符号库
                    SymbologyCtrl.StyleClass = esriSymbologyStyleClass.esriStyleClassScaleBars;
                    pSymStyleClass = SymbologyCtrl.GetStyleClass(esriSymbologyStyleClass.esriStyleClassScaleBars);
                    break;
            }
            pSymStyleClass.UnselectItem();
        }

        private void SymbologyCtrl_OnMouseDown(object sender, ISymbologyControlEvents_OnMouseDownEvent e)
        {
            try
            {
                pStyleGalleryItem = SymbologyCtrl.HitTest(e.x, e.y);//用户选择需要符号              
            }
            catch (Exception ex)
            {

            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            GetSelSymbolItem(ref pStyleGalleryItem);//传递用户选择的值
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            GetSelSymbolItem(ref pStyleGalleryItem);//传递用户选择的值
            this.Close();
        }


        public EnumMapSurroundType EnumMapSurType
        {
            get { return _enumMapSurType; }
            set { _enumMapSurType = value; }
        }
    
    
    
    
    
    
    
    
    
    }
}
