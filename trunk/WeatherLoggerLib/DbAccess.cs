using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace WeatherLoggerLib
{
    /// <summary>
    /// Provides access to the Weather Data Base
    /// </summary>
    public class DbAccess : INotifyPropertyChanged
    {
        private int _count;

        public int Count
        {
            get { return _count; }
            set { _count = value; }
        }

        private DateTime _firstDate;

        public DateTime FirstDate
        {
            get { return _firstDate; }
            set { _firstDate = value; }
        }


        private DateTime _lastDate;

        public DateTime LastDate
        {
            get { return _lastDate; }
            set { _lastDate = value; }
        }

        private List<WeatherSnapshot> _snapshots;

        public List<WeatherSnapshot> WeatherSnapshotList
        {
            get { return _snapshots; }
            set { _snapshots = value; }
        }

        public DatabaseEntities1 Context { get; set; }

        public DbAccess()
        {
            Context = new DatabaseEntities1();
            Count = Context.WeatherSnapshots.Count();
            if (Count == 0)
            {
                FirstDate = DateTime.Now;
                LastDate = DateTime.Now;
            }
            else
            {

                FirstDate = Context.WeatherSnapshots.Min(tr => tr.Timestamp);
                LastDate = Context.WeatherSnapshots.Max(tr => tr.Timestamp);
            }
        }

        public void Add(List<WeatherSnapshot> listToAdd)
        {
            try
            {
                foreach (WeatherSnapshot snapshot in listToAdd)
                {

                    Context.WeatherSnapshots.AddObject(snapshot);
                }
            }
            catch (Exception e)
            {

                throw;
            }
            finally
            {
                try
                {
                    Context.SaveChanges();
                }
                catch (Exception e)
                {

                    throw;
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public List<WeatherSnapshot> Load(DateTime fromDate, DateTime toDate)
        {
            var result = from c in Context.WeatherSnapshots where c.Timestamp >= fromDate && c.Timestamp <= toDate select c;
            return result.ToList();
        }
    }
}
