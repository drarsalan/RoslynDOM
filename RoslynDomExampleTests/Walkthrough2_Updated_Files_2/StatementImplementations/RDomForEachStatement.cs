using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using RoslynDom.Common;
using System.ComponentModel.DataAnnotations;
namespace RoslynDom
{
   public class RDomForEachStatement : RDomBaseLoop<IForEachStatement>, IForEachStatement
   {

      public RDomForEachStatement(SyntaxNode rawItem, IDom parent, SemanticModel model)
         : base(rawItem, parent, model)
      { Initialize(); }

      [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
        "CA1811:AvoidUncalledPrivateCode", Justification = "Called via Reflection")]
      internal RDomForEachStatement(RDomForEachStatement oldRDom)
          : base(oldRDom)
      { Variable = oldRDom.Variable; }

      public override IEnumerable<IDom> Children
      {
         get
         {
            var list = new List<IDom>();
            list.Add(Variable);
            list.AddRange(base.Children);
            return list;
         }
      }

      public IVariableDeclaration Variable { get {return variable; }
set {SetProperty(ref variable, value); }}
      private IVariableDeclaration variable ;
   }
}
