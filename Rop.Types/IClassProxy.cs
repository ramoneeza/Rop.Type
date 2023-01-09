using System;
using System.Collections.Generic;

namespace Rop.Types;

public interface IClassProxy
{
    RuntimeTypeHandle TypeHandle { get; }
    ITypeProxy Type { get; }
    IReadOnlyList<IPropertyProxy> GetPublicProperties();
}