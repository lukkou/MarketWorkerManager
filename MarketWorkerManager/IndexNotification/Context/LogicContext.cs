using System;
using System.Data.Entity;

using IndexNotification.Common;
using IndexNotification.Logic;
using IndexNotification.Context;

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
        /// 経済指標ロジック
        /// </summary>
        public IndexCalendarLogic IndexCalendar { get; private set; }

        /// <summary>
        /// Twitterロジック
        /// </summary>
        public NotificationTweetLogic NotificationTweet { get; private set; }


        /// <summary>
        /// LogicContext
        /// </summary>
        public LogicContext()
        {
            db = new DatabaseContext();
            db.Database.Log = AppendLog;

            //各ロジックを生成
            IndexCalendar = new IndexCalendarLogic{Db = this.db};
            NotificationTweet = new NotificationTweetLogic();
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
