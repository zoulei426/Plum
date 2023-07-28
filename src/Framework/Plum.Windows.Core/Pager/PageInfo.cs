using Plum.Object;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Plum.Windows.Pager
{
    /// <summary>
    /// 分页信息
    /// </summary>
    [Serializable]
    [AddINotifyPropertyChangedInterface]
    public class PageInfo : BindableObject
    {
        /// <summary>
        /// 当前页码
        /// </summary>
        [DisplayName("当前页码")]
        public int PageIndex { get; set; }

        /// <summary>
        /// 页数
        /// </summary>
        [DisplayName("页数")]
        public int PageCount { get; set; }

        /// <summary>
        /// 每页数量
        /// </summary>
        [DisplayName("每页数量")]
        public int PageSize
        {
            get { return _PageSize; }
            set
            {
                _PageSize = value;
                PageCount = (int)((TotalCount + PageSize - 1) / PageSize);
                PageIndex = 1;
            }
        }

        private int _PageSize;

        /// <summary>
        /// 总数量
        /// </summary>
        [DisplayName("总数量")]
        public long TotalCount
        {
            get { return _TotalCount; }
            set
            {
                _TotalCount = value;
                PageCount = (int)((TotalCount + PageSize - 1) / PageSize);
            }
        }

        private long _TotalCount;

        /// <summary>
        /// 每页数量字典
        /// </summary>
        [DisplayName("每页数量字典")]
        public Dictionary<string, int> PageSizeDic { get; set; }

        public ObservableCollection<int> PageSizeCollection { get; set; }

        #region Ctor

        public PageInfo(int totalCount = 0, int pageSize = 10, int pageIndex = 1)
        {
            if (totalCount < 0) totalCount = 0;
            if (pageSize < 0) pageSize = 10;
            if (pageIndex < 0) pageIndex = 1;

            this.TotalCount = totalCount;
            this.PageSize = pageSize;
            this.PageIndex = pageIndex;

            PageCount = (int)((TotalCount + PageSize - 1) / PageSize);

            PageSizeCollection = new ObservableCollection<int>
            {
                10,20,50,100,200,500
            };
            this.PageSizeDic = new Dictionary<string, int>();
            PageSizeDic.Add("10", 10);
            PageSizeDic.Add("20", 20);
            PageSizeDic.Add("50", 50);
            PageSizeDic.Add("100", 100);
            PageSizeDic.Add("1000", 1000);
        }

        #endregion Ctor
    }
}