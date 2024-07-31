using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.IEntities
{
    public interface IReadableLink
    {
        string Url { get; }
        string LinkText { get; }
    }
}
