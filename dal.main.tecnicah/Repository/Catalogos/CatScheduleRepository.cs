using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using biz.main.tecnicah.Repository.Catalogos;
using dal.main.tecnicah.DBContext;
using dal.main.tecnicah.Repository.Generic;


namespace dal.main.tecnicah.Repository.Catalogos
{
    public class CatScheduleRepository :GenericRepository<biz.main.tecnicah.Entities.Schedule>, ICatScheduleRepository
    {
        public CatScheduleRepository(db_HemofiliaContext context) : base(context)
        { 
        }
    }
}
