using dal.main.tecnicah.DBContext;
using dal.main.tecnicah.Repository.Generic;
using biz.main.tecnicah.Repository.Calendario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dal.main.tecnicah.Repository.Calendario
{
    public class CalendarioRepository : GenericRepository<biz.main.tecnicah.Entities.Calendar>, ICalendarioRepository
    {
        public CalendarioRepository(db_HemofiliaContext context) : base(context)
        {
            
        }
        public List<biz.main.tecnicah.Entities.Calendar> Calendario(List<biz.main.tecnicah.Entities.Calendar> calendar)
        {
            List<biz.main.tecnicah.Entities.Calendar> lista = new List<biz.main.tecnicah.Entities.Calendar>();
            foreach (var item in calendar)
            {
                base.Add(item);
                lista.Add(item);
            }
            return lista;

        }
    }
}
