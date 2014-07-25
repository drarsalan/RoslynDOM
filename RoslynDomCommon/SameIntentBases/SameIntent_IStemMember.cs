﻿namespace RoslynDom.Common
{
    public class SameIntent_IStemMember : ISameIntent<IStemMember>
    {
      public bool SameIntent(IStemMember one, IStemMember other, bool skipPublicAnnotations)
        {
            if (one.StemMemberKind != other.StemMemberKind) { return false; }
            return true;
        }
    }
}
