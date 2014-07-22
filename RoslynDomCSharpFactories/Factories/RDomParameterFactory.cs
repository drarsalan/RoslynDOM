﻿using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using RoslynDom.Common;
using System.Linq;

namespace RoslynDom.CSharp
{
    public class RDomParameterMiscFactory
            : RDomMiscFactory<RDomParameter, ParameterSyntax>
    {
        public override IEnumerable<IMisc> CreateFrom(SyntaxNode syntaxNode, IDom parent, SemanticModel model)
        {
            var syntax = syntaxNode as ParameterSyntax;
            var newItem = new RDomParameter(syntaxNode, parent,model);
            newItem.Name = newItem.TypedSymbol.Name;

            var attributes = RDomFactoryHelper.GetAttributesFrom(syntaxNode, newItem, model);
            newItem.Attributes.AddOrMoveAttributeRange(attributes);

            newItem.Type = new RDomReferencedType(newItem.TypedSymbol.DeclaringSyntaxReferences, newItem.TypedSymbol.Type);
            newItem.IsOut = newItem.TypedSymbol.RefKind == RefKind.Out;
            newItem.IsRef = newItem.TypedSymbol.RefKind == RefKind.Ref;
            newItem.IsParamArray = newItem.TypedSymbol.IsParams;
            newItem.IsOptional = newItem.TypedSymbol.IsOptional;
            newItem.Ordinal = newItem.TypedSymbol.Ordinal;

            return new IMisc[] { newItem };
        }

        public override IEnumerable<SyntaxNode> BuildSyntax(IMisc item)
        {
            var nameSyntax = SyntaxFactory.Identifier(item.Name);
            var itemAsT = item as IParameter;
            var syntaxType = (TypeSyntax)(RDomCSharpFactory.Factory.BuildSyntax(itemAsT.Type));

            var node = SyntaxFactory.Parameter(nameSyntax)
                        .WithType(syntaxType);

            var attributes = RDomFactoryHelper.BuildAttributeSyntax(itemAsT.Attributes);
            if (attributes.Any()) { node = node.WithAttributeLists(attributes.WrapInAttributeList()); }

            var modifiers = SyntaxFactory.TokenList();
            if (itemAsT.IsOut) { modifiers = modifiers.Add(SyntaxFactory.Token(SyntaxKind.OutKeyword)); }
            if (itemAsT.IsRef) { modifiers = modifiers.Add(SyntaxFactory.Token(SyntaxKind.RefKeyword)); }
            if (itemAsT.IsParamArray) { modifiers = modifiers.Add(SyntaxFactory.Token(SyntaxKind.ParamsKeyword)); }
            if (itemAsT.IsRef) { modifiers = modifiers.Add(SyntaxFactory.Token(SyntaxKind.RefKeyword)); }
            if (modifiers.Any()) { node = node.WithModifiers(modifiers); }
            // TODO:return new SyntaxNode[] { node.Format() };
            return new SyntaxNode[] { node };

        }
    }

}
