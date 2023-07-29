using Plum.Object;

namespace Plum.Windows.Mvvm
{
    public interface IDataPagerProvider
    {
        #region Methods

        //long Count();

        string GetDefaultOrderPropertyName();

        DataColumn[] GetProperties();

        //string GetSelectedItemDescription();

        //List<object> PagingAsync(int pageIndex, int pageSize, string sorting, string filter = null);

        Paged PagingAsync(int pageIndex, int pageSize, string sorting, string filter = null);

        string GetSumaryStatistic();

        #endregion Methods
    }
}