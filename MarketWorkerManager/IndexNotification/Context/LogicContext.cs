using System;
using System.Data.Entity;

using IndexNotification.Context;
using IndexNotification.Common;

namespace IndexNotification.Context
{
    class LogicContext
    {
        /// <summary>
        /// データベースコンテキスト
        /// </summary>
        private DatabaseContext db { get; set; }

        /// <summary>
        /// データベースコンテキストのトランザクション
        /// </summary>
        private DbContextTransaction tran { get; set; }

        /// <summary>
        /// LogicContext
        /// </summary>
        public LogicContext()
        {
            db = new DatabaseContext();
            db.Database.Log = AppendLog;
        }

        /// <summary>
        /// EntityFrameworkのトランザクション開始
        /// </summary>
        public void BeginTransaction()
        {
            tran = db.Database.BeginTransaction();
        }

        /// <summary>
        /// EntityFrameworkのコミット
        /// </summary>
        public void Commit()
        {
            if (tran != null)
            {
                tran.Commit();
                tran.Dispose();
            }
        }

        /// <summary>
        /// EntityFrameworkのロールバック
        /// </summary>
        public void Rollback()
        {
            if (tran != null)
            {
                tran.Rollback();
                tran.Dispose();
            }
        }

        /// <summary>
        /// EntityFrameworkログ出力オブジェクト
        /// </summary>
        /// <param name="msg"></param>
        private void AppendLog(string msg)
        {
            if (string.IsNullOrWhiteSpace(msg))
            {
                return;
            }

            string logMsg = msg.TrimEnd(new char[] { '\r', '\n' });
            Log.SqlLogger.Info("\n" + logMsg + "\n");
        }

        public void Dispose()
        {
            if (db != null)
            {
                db.Dispose();
            }
        }
    }
}
