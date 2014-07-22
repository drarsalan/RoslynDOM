﻿namespace RoslynDom.Common
{
    public interface ITypeMemberOrWhitespace 
    {
    }

    public interface ITypeMember : IDom, IMember, IHasAttributes, IHasAccessModifier, IHasStructuredDocumentation, ITypeMemberOrWhitespace 
    {
        MemberKind MemberKind { get; }
    }
    public interface ITypeMember<T> : ITypeMember, IDom<T>
        where T : ITypeMember<T>
    {
    }
}
