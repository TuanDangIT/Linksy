using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Domain.Abstractions
{
    public abstract class BaseEntityWithMultitenancy : BaseEntity
    {
        public int UserId { get; private set; }
        protected BaseEntityWithMultitenancy(int userId)
        {
            UserId = userId;
        }
        protected BaseEntityWithMultitenancy()
        {
            
        }
    }
}
