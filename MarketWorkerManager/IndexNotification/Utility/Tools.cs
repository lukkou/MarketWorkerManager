using System;
using System.IO;

using IndexNotification.Common;

namespace IndexNotification.Utility
{
    class Tools
    {
        #region ディレクトリ操作
        /// <summary>
        /// 指定パスにディレクトリを作成する
        /// </summary>
        /// <param name="path">作成パス</param>
        public static void CreateDirectory(string path)
        {
            if (!IsDirectoryExists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        /// <summary>
        /// ディレクトリの存在を確認する
        /// </summary>
        /// <param name="path">対象パス</param>
        /// <returns></returns>
        public static bool IsDirectoryExists(string path)
        {
            if (Directory.Exists(path))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region テキスト操作
        /// <summary>
        /// コンソールに出力する文字列の作成
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static string ToConsoleString(string val)
        {
            return ">>>" + DateTime.Now.ToString() + ":" + val;
        }

        /// <summary>
        /// 東証より取得したExcelの値が-かをチェック
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsHyphen(string str)
        {
            bool result = false;

            if (!string.IsNullOrEmpty(str))
            {
                if (str == "-")
                {
                    result = true;
                }
            }

            return result;
        }

        /// <summary>
        /// 東証より取得したExcelの-をnullに置き換える
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int ReplaceHyphenToZero(string str)
        {
            int result = 0;
            if (!string.IsNullOrEmpty(str))
            {
                if (str != "-")
                {
                    result = Convert.ToInt32(str);
                }
            }

            return result;
        }

        /// <summary>
        /// 日付型文字列が正しい日付文字列かチェックを行い正しければ指定フォーマットに変換
        /// 不正な日付の場合は空文字
        /// </summary>
        /// <param name="val">日付文字列</param>
        /// <param name="setFormat">指定フォーマット 基本はyyyy/MM/dd</param>
        /// <returns>変換した文字列</returns>
        public static string ToDateFormatString(string val, string setFormat = "yyyy/MM/dd")
        {
            string result = string.Empty;
            DateTime valDate = new DateTime();

            if (string.IsNullOrEmpty(val) == false)
            {
                string formatString = Convert.ToInt32(val.Replace("/", string.Empty).Replace(" ", string.Empty)).ToString("0000/00/00");
                if (DateTime.TryParse(formatString, out valDate))
                {
                    result = valDate.ToString(setFormat);
                }
            }

            return result;
        }

        /// <summary>
        /// 日付文字列も日付型に変換
        /// 変換出来ない場合は1970/01/01
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static DateTime DateStringToDate(string val)
        {
            DateTime result = new DateTime();

            return result;
        }

        /// <summary>
        /// 通貨コードを国名に変換
        /// </summary>
        /// <param name="currencyCode"></param>
        /// <returns></returns>
        public static string CurrencyNameToCompanyName(string currencyCode)
        {
            string result = string.Empty;

            switch (currencyCode)
            {
                case Define.CurrencyCode.AU:
                    //オーストラリ
                    result = Define.CountryName.AU;
                    break;

                case Define.CurrencyCode.CA:
                    //カナダ
                    result = Define.CountryName.CA;
                    break;

                case Define.CurrencyCode.CH:
                    //スイス
                    result = Define.CountryName.CH;
                    break;

                case Define.CurrencyCode.EU:
                    //ユーロ圏
                    result = Define.CountryName.EU;
                    break;

                case Define.CurrencyCode.GB:
                    //イギリス
                    result = Define.CountryName.GB;
                    break;

                case Define.CurrencyCode.JP:
                    //日本
                    result = Define.CountryName.JP;
                    break;

                case Define.CurrencyCode.US:
                    //アメリカ
                    result = Define.CountryName.US;
                    break;
            }

            return result;
        }

        /// <summary>
        /// 国名を国旗絵文字に変換
        /// </summary>
        /// <param name="countryName"></param>
        /// <returns></returns>
        public static string CountryNameToCountryFlag(string countryName)
        {
            string result = string.Empty;

            switch (countryName)
            {
                case Define.CountryName.AU:
                    //オーストラリ
                    result = Define.CompanyFlag.AU;
                    break;

                case Define.CountryName.CA:
                    //カナダ
                    result = Define.CompanyFlag.CA;
                    break;

                case Define.CountryName.CH:
                    //スイス
                    result = Define.CompanyFlag.CH;
                    break;

                case Define.CountryName.EU:
                    //ユーロ圏
                    result = Define.CompanyFlag.EU;
                    break;

                case Define.CountryName.GB:
                    //イギリス
                    result = Define.CompanyFlag.GB;
                    break;

                case Define.CountryName.JP:
                    //日本
                    result = Define.CompanyFlag.JP;
                    break;

                case Define.CountryName.US:
                    //アメリカ
                    result = Define.CompanyFlag.US;
                    break;

                case Define.CountryName.DE:
                    //ドイツ
                    result = Define.CompanyFlag.DE;
                    break;

                case Define.CountryName.IT:
                    //イタリア
                    result = Define.CompanyFlag.IT;
                    break;

                case Define.CountryName.FR:
                    //フランス
                    result = Define.CompanyFlag.FR;
                    break;

                case Define.CountryName.ES:
                    //スペイン
                    result = Define.CompanyFlag.ES;
                    break;
            }

            return result;
        }

        #endregion

    }
}
