using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Controls;
using System.Windows.Forms;

namespace 符号化.Class
{
    class OperateMap
    {
        #region 设置RGB
        /// <summary>
        /// 获取RGB颜色值
        /// </summary>
        /// <param name="intR"></param>
        /// <param name="intG"></param>
        /// <param name="intB"></param>
        /// <returns></returns>
        public IRgbColor GetRgbColor(int intR, int intG, int intB)
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
        #endregion
        #region 保存地图文档
        /// <summary>
        /// 保存地图文档
        /// </summary>
        /// <param name="m_FilePath"></param>
        /// <param name="m_SaveMap"></param>
        /// <returns></returns>
        public bool SaveMap(string m_FilePath, IMap m_SaveMap)
        {
            try
            {
                IMapDocument pMapDoc = new MapDocumentClass();
                IMxdContents pMxdC = m_SaveMap as IMxdContents;
                pMapDoc.New(m_FilePath);
                pMapDoc.ReplaceContents(pMxdC);
                if (pMapDoc.get_IsReadOnly(pMapDoc.DocumentFilename) == true)
                {
                    MessageBox.Show("本地图文档是只读的，不能保存!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
                pMapDoc.Save(pMapDoc.UsesRelativePaths, true);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }
        #endregion
        #region 获取地图要素类
        /// <summary>
        /// 获取地图要素类
        /// </summary>
        /// <param name="pLayer"></param>
        /// <param name="_lstFeatCls"></param>
        public void GetLstFeatCls(ILayer pLayer, ref List<IFeatureClass> _lstFeatCls)
        {
            try
            {
                ILayer pLyr = null;
                ICompositeLayer pComLyr = pLayer as ICompositeLayer;
                if (pComLyr == null)
                {
                    IFeatureLayer pFeatLyr = pLayer as IFeatureLayer;
                    if (!_lstFeatCls.Contains(pFeatLyr.FeatureClass))
                    {
                        _lstFeatCls.Add(pFeatLyr.FeatureClass);
                    }
                }
                else
                {
                    for (int i = 0; i < pComLyr.Count; i++)
                    {
                        pLyr = pComLyr.get_Layer(i);
                        GetLstFeatCls(pLyr, ref _lstFeatCls);
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
        #endregion
        #region 获取当前地图所有图层的要素类
        /// <summary>
        /// 获取当前地图中的图层
        /// </summary>
        /// <param name="pMap"></param>
        /// <returns></returns>
        public List<IFeatureClass> GetLstFeatCls(IMap pMap)
        {
            List<IFeatureClass> _lstFeatCls = null;
            try
            {
                ILayer pLayer = null;
                IFeatureLayer pFeatLyr = null;
                _lstFeatCls = new List<IFeatureClass>();
                for (int i = 0; i < pMap.LayerCount; i++)
                {
                    pLayer = pMap.get_Layer(i);
                    pFeatLyr = pLayer as IFeatureLayer;
                    GetLstFeatCls(pLayer, ref _lstFeatCls);
                }
            }
            catch (Exception ex)
            {


            }
            return _lstFeatCls;
        }
        #endregion
        #region 由名称获取图层
        /// <summary>
        /// 由名称获取图层
        /// </summary>
        /// <param name="pLayer"></param>
        /// <param name="sFeatLyrName"></param>
        /// <returns></returns>
        public IFeatureLayer GetFeatLyrByName(ILayer pLayer, string sFeatLyrName)
        {
            ILayer pLyr = null;
            IFeatureLayer pFeatureLyr = null;
            IFeatureLayer pFeatLyr = null;
            ICompositeLayer pComLyr = pLayer as ICompositeLayer;
            if (pComLyr == null)
            {
                pFeatLyr = pLayer as IFeatureLayer;
                if (pFeatLyr.FeatureClass.AliasName == sFeatLyrName)
                {
                    pFeatureLyr = pFeatLyr;
                    return pFeatureLyr;
                }
            }
            else
            {
                for (int i = 0; i < pComLyr.Count; i++)
                {
                    pLyr = pComLyr.get_Layer(i);
                    GetFeatLyrByName(pLyr, sFeatLyrName);
                }
            }
            return pFeatureLyr;
        }
        #endregion
        #region 据图层名称获取图层
        /// <summary>
        /// 根据图层名称获取图层  
        /// </summary>
        /// <param name="pMap"></param>
        /// <param name="sFeatLyrName"></param>
        /// <returns></returns>
        public IFeatureLayer GetFeatLyrByName(IMap pMap, string sFeatLyrName)
        {
            IFeatureLayer pFeatLyr = null;
            try
            {
                ILayer pLayer = null;
                for (int i = 0; i < pMap.LayerCount; i++)
                {
                    pLayer = pMap.get_Layer(i);
                    pFeatLyr = GetFeatLyrByName(pLayer, sFeatLyrName);
                    if (pFeatLyr != null) break;
                }
            }
            catch (Exception ex)
            {
            }
            return pFeatLyr;
        }
        #endregion
        #region 获取路径算法
        public string getPath(string path)
        {
            int t;
            for (t = 0; t < path.Length; t++)
            {
                if (path.Substring(t, 5) == "毕设源程序")
                {
                    break;
                }
            }
            string name = path.Substring(0, t + 5);
            return name;
        }
        #endregion
    }
}
