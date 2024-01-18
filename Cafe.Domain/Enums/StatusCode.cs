using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cafe.Domain.Enums
{
    public enum StatusCode
    {
        OK = 100, 
        InternalServerError = 200,
        NotFoundDishError = 505,
        UserAlreadyExists = 600,
        NotFoundRoleError = 705,
        NotFoundUserError = 605,
        OrderNotFound = 706,
    }
}
