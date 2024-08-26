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
    public  class CatDayRepository : GenericRepository<biz.main.tecnicah.Entities.Day>, ICatDayRepository
    {
        public CatDayRepository(db_HemofiliaContext context) : base(context)
        { 
        }
    }
}
