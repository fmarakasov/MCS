using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using CommonBase;

namespace MCDomain.Model
{

    public enum WellKnownPosts
    {
        [Description("Не определён")]
        Undefined = -1,
        [Description("Заместитель директора")]
        Director = 0
    }


    public partial class Post:  IObjectId
    {
        public bool IsWellKnownId()
        {
            var en = Enum.GetValues(typeof(WellKnownPosts));
            foreach (var ch in en)
            {
                if ((WellKnownPosts)Id == (WellKnownPosts)ch) return true;
            }
            return false;
        }

        /// <summary>
        /// Получает известную должность
        /// </summary>
        public WellKnownPosts WellKnownType
        {
            get
            {
                if (IsWellKnownId())
                    return (WellKnownPosts)Id;
                return WellKnownPosts.Undefined;
            }
        }
    }
}
