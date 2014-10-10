using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using RoslynDom.Common;
using System.ComponentModel.DataAnnotations;
namespace RoslynDom
{
   public class RDomForStatement : RDomBaseLoop<IForStatement>, IForStatement
   {

      public RDomForStatement(SyntaxNode rawItem, IDom parent, SemanticModel model)
         : base(rawItem, parent, model)
      { Initialize(); }

      [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
          "CA1811:AvoidUncalledPrivateCode", Justification = "Called via Reflection")]
      internal RDomForStatement(RDomForStatement oldRDom)
          : base(oldRDom)
      {
         Incrementor = oldRDom.Incrementor.Copy();
         Variable = oldRDom.Variable.Copy();
      }

      public override IEnumerable<IDom> Children
      {
         get
         {
            var list = new List<IDom>();
            list.Add(Incrementor);
            list.Add(Variable);
            list.AddRange(base.Children);
            return list;
         }
      }


      private IExpression incrementor ;
      public IExpression Incrementor { get {return incrementor; }
set {SetProperty(ref incrementor, value); }}
      public IVariableDeclaration Variable { get {return variable; }
set {SetProperty(ref variable, value); }}
      private IVariableDeclaration variable ;
   }
}