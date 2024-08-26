using biz.main.tecnicah.Entities;
using biz.main.tecnicah.Repository.Catalogos;
using dal.main.tecnicah.DBContext;
using dal.main.tecnicah.Repository.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dal.main.tecnicah.Repository.Catalogos
{
    public class CatRoleRepository : GenericRepository<CatRole>, ICatRoleRepository
    {
        public CatRoleRepository(db_HemofiliaContext context) : base(context)
        {
        }
    }
}
