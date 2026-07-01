using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChinookDal
{
  public  class Artist
    {
        public virtual int ArtistId { get; set; }
        public virtual string Name { get; set; }
        public virtual IList<Album> Albums { get; set; }
    }
}
