using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Duikboot.Web.Repositories
{
    public class CapacityRepository
    {
        private readonly DuikbootContext Db = new DuikbootContext();


        public bool HasSpots(string day)
        {
            var selectedDay = Db.Capacity.Where(x => x.Name == day).FirstOrDefault();

            if (selectedDay == null)
            {
                return false;
            }
            else
            {
                return (selectedDay.Spots <= 0);
            }

        }

        public void UpdateSpots(string day)
        {
            var selectedDay = Db.Capacity.Where(x => x.Name == day).FirstOrDefault();


            if(selectedDay.Spots > 0)
            {
                selectedDay.Spots -= 1;
            }

            Db.SaveChanges();
        }

        public Dictionary<string, bool> GetAvailableDates()
        {
            var availableDays = new Dictionary<string, bool>()
            {
                {"zaterdag", HasSpots("zaterdag") },
                {"zondag", HasSpots("zondag") },
                {"maandag", HasSpots("maandag") },
                {"dinsdag", HasSpots("dinsdag") },
            };

            return availableDays;
        }



    }
}