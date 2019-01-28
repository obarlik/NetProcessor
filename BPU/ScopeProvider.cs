using System.Collections.Generic;

namespace BPU
{
    public abstract class ScopeProvider
    {
        public ScopeProvider()
        {
        }

        public abstract IEnumerable<Scope> GetScopes(Context context);
    }
}