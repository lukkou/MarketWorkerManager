using System;
using System.Collections.Generic;
using System.Linq;

using IndexNotification.Common;
using IndexNotification.Models;
using IndexNotification.Context;

namespace IndexNotification.Logics
{
    class IndexCalendarLogic
    {
        public DatabaseContext Db { get; set; }

        /// <summary>
        /// 現在時刻から一時間後に発表される指標を取得する
        /// 一分ごとに動作させるため分で取得情報を決め打ちして良い
        /// </summary>
        /// <returns></returns>
        public List<IndexCalendar> GetIndexCalendarInfo()
        {
            List<IndexCalendar> result = new List<IndexCalendar>();

            DateTime before = DateTime.Now.AddMinutes(29);
            DateTime after = DateTime.Now.AddMinutes(31);

            result = Db.IndexCalendars.Where(
                                        x => x.MyReleaseDate >= before && 
                                        x.MyReleaseDate <= after && 
                                        x.Importance == Define.ImportanceHigh).ToList();

            if (result.Any())
            {
                //取得したGuidKey取得
                List<Guid> indexKeyList = result.Select(x => x.GuidKey).ToList();

                List<NotificationFlg> alreadyList = Db.NotificationFlgs.Where(x => indexKeyList.Contains(x.GuidKey)).ToList();

                if (alreadyList.Any())
                {
                    foreach(NotificationFlg item in alreadyList)
                    {
                        //つぶやきデータがあれば元データから削除
                        IndexCalendar tweetItem = result.Where(x => x.GuidKey == item.GuidKey).FirstOrDefault();
                        result.Remove(tweetItem);
                    }
                }
            }
            
            return result;
        }

        /// <summary>
        /// 未登録のつぶやき済みデータを登録
        /// </summary>
        /// <param name="list"></param>
        public void AddTweetFlg(List<IndexCalendar> list)
        {
            List<NotificationFlg> addList = new List<NotificationFlg>();
            foreach(IndexCalendar item in list)
            {
                NotificationFlg addItem = new NotificationFlg();
                addItem.GuidKey = item.GuidKey;
                addItem.IdKey = item.IdKey;
                addItem.TweetFlg = false;

                addList.Add(addItem);
            }

            Db.NotificationFlgs.AddRange(addList);
        }
    }
}
