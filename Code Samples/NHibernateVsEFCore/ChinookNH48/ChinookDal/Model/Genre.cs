using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChinookDal
{
    public class Genre
    {
        protected Genre()
        {

        }

        public Genre(string name)
        {
            this.Name = name;
        }

        public virtual int GenreId { get; set; }
        public virtual string Name { get; set; }
        public virtual IList<Track> Tracks { get; set; }
    }
}
