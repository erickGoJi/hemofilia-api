using biz.main.tecnicah.Entities;
using biz.main.tecnicah.Repository.ObjectiveSignUp;
using dal.main.tecnicah.DBContext;
using dal.main.tecnicah.Repository.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dal.main.tecnicah.Repository.ObjectiveSignUp
{
    public class ObjectiveSignUpRepository : GenericRepository<biz.main.tecnicah.Entities.ObjectiveSignUp>, IObjectiveSignUpRepository
    {
        public ObjectiveSignUpRepository(db_HemofiliaContext context) : base(context)
        {
        }
    }
}
