using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plum.Common
{
    public class BinaryKey<TPrimary, TSecondary>
    {
        #region Properties

        #region Properties - Public

        /// <summary>
        /// 第一个键
        /// </summary>
        public TPrimary Primary { get; private set; }

        /// <summary>
        /// 第二个键
        /// </summary>
        public TSecondary Secondary { get; private set; }

        #endregion Properties - Public

        #endregion Properties

        #region Methods

        #region Methods - Override

        /// <summary>
        /// 比较相等
        /// </summary>
        /// <param name="obj">对象实例</param>
        /// <returns>布尔值</returns>
        public override bool Equals(object obj)
        {
            if (GetType() != obj?.GetType())
            {
                return false;
            }
            var other = obj as BinaryKey<TPrimary, TSecondary>;

            return Primary.Equals(other.Primary) && Secondary.Equals(other.Secondary);
        }

        /// <summary>
        /// 计算哈希值
        /// </summary>
        /// <returns>哈希值</returns>
        public override int GetHashCode()
        {
            return Primary.GetHashCode() ^ Secondary.GetHashCode();
        }

        #endregion Methods - Override

        #endregion Methods

        #region Ctor

        public BinaryKey(TPrimary primary, TSecondary secondary)
        {
            Primary = primary;
            Secondary = secondary;
        }

        #endregion Ctor
    }
}