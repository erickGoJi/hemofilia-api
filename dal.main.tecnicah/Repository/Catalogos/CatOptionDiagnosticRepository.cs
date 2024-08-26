using dal.main.tecnicah.DBContext;
using dal.main.tecnicah.Repository.Generic;
using biz.main.tecnicah.Repository.Catalogos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dal.main.tecnicah.Repository.Catalogos
{
    public class CatOptionDiagnosticRepository : GenericRepository<biz.main.tecnicah.Entities.CatOptionDiagnostic>, ICatOptionDiagnosticRepository
    {
        public CatOptionDiagnosticRepository(db_HemofiliaContext context) : base(context)
        { 
        }
    }
}
