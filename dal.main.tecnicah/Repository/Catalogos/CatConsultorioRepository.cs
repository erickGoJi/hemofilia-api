using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dal.main.tecnicah.DBContext;
using dal.main.tecnicah.Repository.Generic;
using biz.main.tecnicah.Repository.Catalogos;


namespace dal.main.tecnicah.Repository.Catalogos
{
    public class CatConsultorioRepository : GenericRepository<biz.main.tecnicah.Entities.CatConsultorio>, ICatConsultorioRepository
    {
        public CatConsultorioRepository(db_HemofiliaContext context) : base(context)
        {

        }
    }
}
