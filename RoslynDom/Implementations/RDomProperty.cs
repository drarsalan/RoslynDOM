﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using RoslynDom.Common;
using RoslynDom.Common;

namespace RoslynDom
{
    public class RDomProperty : RDomSyntaxNodeBase<PropertyDeclarationSyntax>, IProperty
    {
        internal RDomProperty(PropertyDeclarationSyntax rawItem) : base(rawItem) { }

        public IEnumerable<IAttribute> Attributes
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override string Name
        {
            get { return TypedRawItem.Identifier.NameFrom(); }
        }

        public override string QualifiedName
        {
            get { return TypedRawItem.Identifier.QualifiedNameFrom(); }
        }

        public override string BestInContextName
        {
            get { return TypedRawItem.Identifier.BestInContextNameFrom(); }
        }
        public Type ReturnType
        {
            get
            {
                throw new NotImplementedException();
            }
        }

    }
}
